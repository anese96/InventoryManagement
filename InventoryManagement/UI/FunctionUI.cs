using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI
{
    public class FunctionUI 
    {
        private static readonly Random random = new();
        public void AddFormField(Panel parent, string labelText, int labelW, int fieldW, int fieldH, int spacing, out TextBox textBox, string format = "", string defaultValue = "0", bool isNumeric = true)
        {
            Panel p = new Panel { Size = new Size(labelW + fieldW + 20, fieldH + 5), Margin = new Padding(0, 0, 0, spacing) };

            Label label = new Label
            {
                Text = labelText,
                Size = new Size(labelW, fieldH),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10)
            };

            textBox = new TextBox
            {
                Size = new Size(fieldW, fieldH),
                Location = new Point(labelW, 0),
                Text = defaultValue,
                Font = new Font("Segoe UI", 10)
            };

            if (isNumeric)
            {

                textBox.KeyPress += (s, e) =>
                {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
                    {
                        e.Handled = true;
                    }

                    if ((e.KeyChar == '.' || e.KeyChar == ',') && ((s as TextBox).Text.Contains(".") || (s as TextBox).Text.Contains(",")))
                    {
                        e.Handled = true;
                    }
                };
            }


            if (!string.IsNullOrEmpty(format))
            {
                textBox.Leave += (s, e) => {
                    TextBox tb = s as TextBox;

                    decimal? val = ParseDecimal(tb.Text);
                    if (val.HasValue)
                    {
                        tb.Text = val.Value.ToString(format);
                    }
                };
            }


            //textBox.KeyDown += Control_KeyDown;
            //textBox.Enter += Control_Enter;
            //textBox.Leave += Control_Leave;

            p.Controls.Add(label);
            p.Controls.Add(textBox);
            parent.Controls.Add(p);
        }
        public void AddComboBoxField(FlowLayoutPanel panel, string labelText, int lblWidth, int ctrlWidth, int spacing, out ComboBox comboBox)
        {
            Panel pnl = new Panel { Size = new Size(lblWidth + ctrlWidth + 20, 35), Margin = new Padding(0, 0, 0, spacing) };
            Label lbl = new Label { Text = labelText, Size = new Size(lblWidth, 30), TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10) };

            comboBox = new ComboBox
            {
                Size = new Size(ctrlWidth, 30),
                Location = new Point(lblWidth, 0),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

            pnl.Controls.Add(lbl);
            pnl.Controls.Add(comboBox);
            panel.Controls.Add(pnl);
            //comboBox.KeyDown += Control_KeyDown;
            //comboBox.Enter += Control_Enter;
            //comboBox.Leave += Control_Leave;
        }

        public decimal? ParseDecimal(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            // Try parsing with CurrentCulture first (handles N2 format with thousands separators)
            if (decimal.TryParse(text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out decimal result))
            {
                return result;
            }

            // Fallback: Remove spaces (common method) and replace comma with dot for Invariant
            string normalized = text.Replace(" ", "").Replace(",", ".");
            if (decimal.TryParse(normalized, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal resultInv))
            {
                return resultInv;
            }
            return 0;
        }


        //public string GenerateBarcode()
        //{
        //    // Simple generation based on timestamp to ensure uniqueness
        //    return DateTime.Now.ToString("yyMMddHHmmss");
        //}
        public  string GenerateBarcode()
        {
            // Préfixe GS1 Algérie
            string prefix = "613";

            // Génère 9 chiffres aléatoires
            string body = string.Concat(
                Enumerable.Range(0, 9)
                          .Select(_ => random.Next(10).ToString()));

            // 12 premiers chiffres
            string code12 = prefix + body;

            // Calcul du chiffre de contrôle
            int checkDigit = CalculateEAN13CheckDigit(code12);

            return code12 + checkDigit;
        }
        private static int CalculateEAN13CheckDigit(string code12)
        {
            if (code12.Length != 12)
                throw new ArgumentException("Le code doit contenir 12 chiffres.");

            int sum = 0;

            for (int i = 0; i < 12; i++)
            {
                int digit = code12[i] - '0';

                // Position impaire = x1, paire = x3
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            return (10 - (sum % 10)) % 10;
        }

        internal void BtnCancel_Click(object? sender, EventArgs e)
        {
            Form form = (sender as Control)?.FindForm();
            form?.Close();
        }
    }
}
