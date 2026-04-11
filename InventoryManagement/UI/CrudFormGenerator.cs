using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace InventoryManagement.UI
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Colour / font design tokens — mirrors the style used in AjouterProduit
    // ─────────────────────────────────────────────────────────────────────────
    internal static class CrudTheme
    {
        public static readonly Color HeaderBg    = Color.FromArgb(44,  62,  80);
        public static readonly Color HeaderFg    = Color.White;
        public static readonly Color ButtonSave  = Color.FromArgb(46, 204, 113);
        public static readonly Color ButtonCancel= Color.FromArgb(231, 76,  60);
        public static readonly Color FooterBg    = Color.FromArgb(236, 240, 241);
        public static readonly Color InputBorder = Color.FromArgb(189, 195, 199);
        public static readonly Color LabelFg     = Color.FromArgb(44,  62,  80);
        public static readonly Color RequiredFg  = Color.FromArgb(192,  57,  43);

        public static Font   FontHeader  => new Font("Segoe UI", 15, FontStyle.Bold);
        public static Font   FontLabel   => new Font("Segoe UI", 10);
        public static Font   FontInput   => new Font("Segoe UI", 10);
        public static Font   FontButton  => new Font("Segoe UI", 10, FontStyle.Bold);
        public static Font   FontRequired=> new Font("Segoe UI", 8, FontStyle.Italic);
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Internal descriptor built from reflection + attributes
    // ─────────────────────────────────────────────────────────────────────────
    internal class FieldDescriptor
    {
        public PropertyInfo  Property    { get; set; }
        public string        Label       { get; set; }
        public FormControlType ControlType { get; set; }
        public string        DataSourceKey { get; set; }
        public bool          IsRequired  { get; set; }
        public int           Column      { get; set; }
        public int           Order       { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Result returned after the user clicks Save
    // ─────────────────────────────────────────────────────────────────────────
    public class CrudFormResult<T>
    {
        public bool   Saved    { get; internal set; }
        public T      Instance { get; internal set; }
        public string Error    { get; internal set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  The main generator
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Generates Add / Edit dialogs for any DTO or model class by reading
    /// <see cref="FormLabelAttribute"/>, <see cref="FormControlAttribute"/>,
    /// <see cref="FormSkipAttribute"/>, <see cref="FormLayoutAttribute"/> and
    /// standard DataAnnotations attributes.
    /// </summary>
    public static class CrudFormGenerator
    {
        // ── Public entry points ──────────────────────────────────────────────

        /// <summary>Shows a modal Add form. Returns the filled-in instance if saved.</summary>
        public static CrudFormResult<T> ShowAddForm<T>(
            string title = null,
            ICrudFormContext context = null) where T : new()
        {
            return ShowForm<T>(default, title ?? ("Ajouter " + FriendlyName<T>()), context);
        }

        /// <summary>Shows a modal Edit form pre-populated with <paramref name="existing"/>.</summary>
        public static CrudFormResult<T> ShowEditForm<T>(
            T existing,
            string title = null,
            ICrudFormContext context = null) where T : new()
        {
            return ShowForm<T>(existing, title ?? ("Modifier " + FriendlyName<T>()), context);
        }

        // ── Core form builder ────────────────────────────────────────────────

        public static CrudFormResult<T> ShowForm<T>(
            T model, string title, ICrudFormContext context) where T : new()
        {
            var result = new CrudFormResult<T>();

            var descriptors = BuildDescriptors<T>();

            // Split into columns
            var col0 = descriptors.Where(d => d.Column == 0).OrderBy(d => d.Order).ToList();
            var col1 = descriptors.Where(d => d.Column == 1).OrderBy(d => d.Order).ToList();
            bool twoColumns = col1.Any();

            // Map: PropertyName → Control (for read-back)
            var controlMap = new Dictionary<string, Control>();

            // ── Form shell ───────────────────────────────────────────────────
            int colCount = twoColumns ? 2 : 1;
            int formWidth = twoColumns ? 900 : 500;
            int formHeight = Math.Max(col0.Count, col1.Count) * 48 + 200;
            formHeight = Math.Max(formHeight, 300);

            var form = new Form
            {
                Text            = title,
                Width           = formWidth,
                Height          = formHeight,
                StartPosition   = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox     = false,
                MinimizeBox     = false,
                BackColor       = Color.White
            };

            // ── TableLayoutPanel: header / body / footer ─────────────────────
            var mainLayout = new TableLayoutPanel
            {
                Dock        = DockStyle.Fill,
                ColumnCount = 1,
                RowCount    = 3
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            form.Controls.Add(mainLayout);

            // Header
            var header = new Label
            {
                Text      = "  " + title,
                Font      = CrudTheme.FontHeader,
                ForeColor = CrudTheme.HeaderFg,
                BackColor = CrudTheme.HeaderBg,
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainLayout.Controls.Add(header, 0, 0);

            // Body
            var bodyLayout = new TableLayoutPanel
            {
                Dock        = DockStyle.Fill,
                ColumnCount = colCount,
                RowCount    = 1,
                Padding     = new Padding(20, 12, 20, 8)
            };
            for (int c = 0; c < colCount; c++)
                bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / colCount));
            mainLayout.Controls.Add(bodyLayout, 0, 1);

            // Left / right panels
            var leftPanel = new FlowLayoutPanel
            {
                Dock          = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents  = false,
                AutoScroll    = true
            };
            bodyLayout.Controls.Add(leftPanel, 0, 0);

            FlowLayoutPanel rightPanel = null;
            if (twoColumns)
            {
                rightPanel = new FlowLayoutPanel
                {
                    Dock          = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents  = false,
                    AutoScroll    = true
                };
                bodyLayout.Controls.Add(rightPanel, 1, 0);
            }

            // Build fields
            int lblW = 130, fldW = twoColumns ? 270 : 310;
            AddFields(col0, leftPanel,  lblW, fldW, model, context, controlMap);
            if (twoColumns)
                AddFields(col1, rightPanel, lblW, fldW, model, context, controlMap);

            // Footer
            var footer = new FlowLayoutPanel
            {
                Dock          = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding       = new Padding(0, 12, 20, 0),
                BackColor     = CrudTheme.FooterBg
            };
            mainLayout.Controls.Add(footer, 0, 2);

            var btnCancel = CreateButton("❌  Annuler", CrudTheme.ButtonCancel);
            btnCancel.Click += (s, e) =>
            {
                result.Saved = false;
                form.DialogResult = DialogResult.Cancel;
                form.Close();
            };

            var btnSave = CreateButton("💾  Enregistrer", CrudTheme.ButtonSave);
            btnSave.Click += (s, e) =>
            {
                try
                {
                    var instance = model != null
                        ? model          // edit mode — mutate the same object
                        : Activator.CreateInstance<T>();

                    ReadBack(instance, controlMap, descriptors);

                    ValidateAll(descriptors, controlMap);

                    result.Saved    = true;
                    result.Instance = instance;
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            footer.Controls.Add(btnCancel);
            footer.Controls.Add(btnSave);

            form.ShowDialog();
            return result;
        }

        // ── Field builder helpers ────────────────────────────────────────────

        private static void AddFields<T>(
            IList<FieldDescriptor> descriptors,
            FlowLayoutPanel panel,
            int lblW, int fldW,
            T model,
            ICrudFormContext context,
            Dictionary<string, Control> controlMap)
        {
            foreach (var desc in descriptors)
            {
                var rowPanel = new Panel
                {
                    Size   = new Size(lblW + fldW + 16, 42),
                    Margin = new Padding(0, 0, 0, 6)
                };

                // Label
                var label = new Label
                {
                    Text      = desc.Label + (desc.IsRequired ? " *" : ""),
                    Size      = new Size(lblW, 30),
                    Location  = new Point(0, 6),
                    Font      = CrudTheme.FontLabel,
                    ForeColor = desc.IsRequired ? CrudTheme.RequiredFg : CrudTheme.LabelFg,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                // Control
                Control ctrl = CreateFieldControl(desc, model, context, lblW, fldW);
                ctrl.Location = new Point(lblW, 4);
                ctrl.Width    = fldW;

                controlMap[desc.Property.Name] = ctrl;

                rowPanel.Controls.Add(label);
                rowPanel.Controls.Add(ctrl);
                panel.Controls.Add(rowPanel);
            }
        }

        private static Control CreateFieldControl<T>(
            FieldDescriptor desc, T model, ICrudFormContext context, int lblW, int fldW)
        {
            object currentValue = model != null ? desc.Property.GetValue(model) : null;

            switch (desc.ControlType)
            {
                case FormControlType.CheckBox:
                {
                    var cb = new CheckBox
                    {
                        Font    = CrudTheme.FontInput,
                        Checked = currentValue is bool b && b,
                        Height  = 28
                    };
                    return cb;
                }

                case FormControlType.ComboBox:
                {
                    var combo = new ComboBox
                    {
                        Font          = CrudTheme.FontInput,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Height        = 30
                    };
                    if (context != null && !string.IsNullOrEmpty(desc.DataSourceKey))
                    {
                        var items = context.GetComboBoxItems(desc.DataSourceKey);
                        combo.DisplayMember = context.GetDisplayMember(desc.DataSourceKey);
                        combo.ValueMember   = context.GetValueMember(desc.DataSourceKey);
                        combo.DataSource    = items;

                        if (currentValue != null)
                        {
                            // Try to pre-select the matching item
                            foreach (var item in combo.Items)
                            {
                                var valProp = item.GetType().GetProperty(combo.ValueMember);
                                if (valProp != null && Equals(valProp.GetValue(item), currentValue))
                                {
                                    combo.SelectedItem = item;
                                    break;
                                }
                            }
                        }
                    }
                    return combo;
                }

                case FormControlType.DatePicker:
                {
                    var dtp = new DateTimePicker
                    {
                        Font   = CrudTheme.FontInput,
                        Format = DateTimePickerFormat.Short,
                        Height = 30
                    };
                    if (currentValue is DateTime dt)
                        dtp.Value = dt;
                    return dtp;
                }

                case FormControlType.ReadOnly:
                {
                    var tb = new TextBox
                    {
                        Font     = CrudTheme.FontInput,
                        Text     = currentValue?.ToString() ?? "",
                        ReadOnly = true,
                        Height   = 28,
                        BackColor= Color.FromArgb(245, 246, 250)
                    };
                    return tb;
                }

                case FormControlType.NumericTextBox:
                {
                    var tb = new TextBox
                    {
                        Font   = CrudTheme.FontInput,
                        Text   = currentValue?.ToString() ?? "0",
                        Height = 28
                    };
                    tb.KeyPress += (s, e) =>
                    {
                        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                            e.Handled = true;
                    };
                    return tb;
                }

                case FormControlType.DecimalTextBox:
                {
                    var tb = new TextBox
                    {
                        Font   = CrudTheme.FontInput,
                        Text   = currentValue != null
                            ? Convert.ToDecimal(currentValue).ToString("N2")
                            : "0.00",
                        Height = 28
                    };
                    tb.KeyPress += (s, e) =>
                    {
                        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                            && e.KeyChar != '.' && e.KeyChar != ',')
                            e.Handled = true;
                        if ((e.KeyChar == '.' || e.KeyChar == ',')
                            && (((TextBox)s).Text.Contains('.') || ((TextBox)s).Text.Contains(',')))
                            e.Handled = true;
                    };
                    tb.Leave += (s, e) =>
                    {
                        var box = (TextBox)s;
                        string normalized = box.Text.Replace(",", ".");
                        if (decimal.TryParse(normalized,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out decimal v))
                            box.Text = v.ToString("N2");
                    };
                    return tb;
                }

                default: // TextBox
                {
                    var tb = new TextBox
                    {
                        Font   = CrudTheme.FontInput,
                        Text   = currentValue?.ToString() ?? "",
                        Height = 28
                    };
                    return tb;
                }
            }
        }

        // ── Read-back (Control → instance) ───────────────────────────────────

        private static void ReadBack<T>(
            T instance,
            Dictionary<string, Control> controlMap,
            IList<FieldDescriptor> descriptors)
        {
            foreach (var desc in descriptors)
            {
                if (!controlMap.TryGetValue(desc.Property.Name, out var ctrl)) continue;
                if (desc.ControlType == FormControlType.ReadOnly) continue;

                var propType = desc.Property.PropertyType;
                object value = null;

                if (ctrl is CheckBox cb)
                {
                    value = cb.Checked;
                }
                else if (ctrl is ComboBox combo)
                {
                    // Return the selected ValueMember value
                    if (combo.SelectedItem != null && !string.IsNullOrEmpty(combo.ValueMember))
                    {
                        var valProp = combo.SelectedItem.GetType().GetProperty(combo.ValueMember);
                        value = valProp?.GetValue(combo.SelectedItem);
                    }
                    else
                    {
                        value = combo.SelectedValue;
                    }
                }
                else if (ctrl is DateTimePicker dtp)
                {
                    value = dtp.Value;
                }
                else
                {
                    // TextBox variants
                    var text = ctrl.Text;
                    value = ConvertText(text, propType);
                }

                // Handle nullable types
                if (value != null && propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var inner = Nullable.GetUnderlyingType(propType);
                    value = Convert.ChangeType(value, inner);
                }
                else if (value != null && propType != typeof(string))
                {
                    try { value = Convert.ChangeType(value, propType); } catch { /* best effort */ }
                }

                desc.Property.SetValue(instance, value);
            }
        }

        private static object ConvertText(string text, Type propType)
        {
            var underlying = Nullable.GetUnderlyingType(propType) ?? propType;

            if (string.IsNullOrWhiteSpace(text))
                return propType.IsValueType && Nullable.GetUnderlyingType(propType) == null
                    ? Activator.CreateInstance(propType)
                    : null;

            if (underlying == typeof(int))
                return int.TryParse(text, out var i) ? i : (object)0;
            if (underlying == typeof(decimal))
            {
                string normalized = text.Replace(",", ".");
                return decimal.TryParse(normalized,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out var d) ? d : 0m;
            }
            if (underlying == typeof(double))
                return double.TryParse(text, out var db) ? db : 0.0;
            if (underlying == typeof(bool))
                return bool.TryParse(text, out var bl) && bl;
            if (underlying == typeof(DateTime))
                return DateTime.TryParse(text, out var dt) ? dt : DateTime.Now;

            return text;
        }

        // ── Validation ───────────────────────────────────────────────────────

        private static void ValidateAll(
            IList<FieldDescriptor> descriptors,
            Dictionary<string, Control> controlMap)
        {
            foreach (var desc in descriptors)
            {
                if (!desc.IsRequired) continue;
                if (!controlMap.TryGetValue(desc.Property.Name, out var ctrl)) continue;

                bool empty = ctrl is TextBox tb && string.IsNullOrWhiteSpace(tb.Text)
                          || ctrl is ComboBox cb && cb.SelectedIndex < 0;

                if (empty)
                    throw new Exception($"Le champ « {desc.Label} » est obligatoire.");
            }
        }

        // ── Descriptor builder ───────────────────────────────────────────────

        private static IList<FieldDescriptor> BuildDescriptors<T>()
        {
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var result = new List<FieldDescriptor>();
            int order = 0;

            foreach (var prop in props)
            {
                // Skip properties decorated with [FormSkip]
                if (prop.GetCustomAttribute<FormSkipAttribute>() != null) continue;
                // Skip navigation/collection properties
                if (prop.PropertyType.IsClass
                    && prop.PropertyType != typeof(string)
                    && !prop.PropertyType.IsValueType) continue;
                if (prop.PropertyType.IsGenericType
                    && typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)) continue;

                var labelAttr   = prop.GetCustomAttribute<FormLabelAttribute>();
                var controlAttr = prop.GetCustomAttribute<FormControlAttribute>();
                var layoutAttr  = prop.GetCustomAttribute<FormLayoutAttribute>();
                var requiredAttr= prop.GetCustomAttribute<RequiredAttribute>();

                // Determine control type heuristically if not specified
                FormControlType ctrlType = FormControlType.TextBox;
                string dsKey = null;

                if (controlAttr != null)
                {
                    ctrlType = controlAttr.ControlType;
                    dsKey    = controlAttr.DataSourceKey;
                }
                else
                {
                    var underlying = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    if (underlying == typeof(bool))
                        ctrlType = FormControlType.CheckBox;
                    else if (underlying == typeof(decimal) || underlying == typeof(double) || underlying == typeof(float))
                        ctrlType = FormControlType.DecimalTextBox;
                    else if (underlying == typeof(int) && prop.Name == "Id")
                        ctrlType = FormControlType.ReadOnly;
                    else if (underlying == typeof(int))
                        ctrlType = FormControlType.NumericTextBox;
                    else if (underlying == typeof(DateTime))
                        ctrlType = FormControlType.DatePicker;
                }

                result.Add(new FieldDescriptor
                {
                    Property      = prop,
                    Label         = labelAttr?.Label ?? SplitCamelCase(prop.Name),
                    ControlType   = ctrlType,
                    DataSourceKey = dsKey,
                    IsRequired    = requiredAttr != null,
                    Column        = layoutAttr?.Column ?? 0,
                    Order         = layoutAttr?.Order  ?? order++
                });
            }

            return result;
        }

        // ── UI helpers ───────────────────────────────────────────────────────

        private static Button CreateButton(string text, Color backColor)
        {
            var btn = new Button
            {
                Text      = text,
                Size      = new Size(150, 40),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = CrudTheme.FontButton,
                Cursor    = Cursors.Hand,
                Margin    = new Padding(6, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private static string SplitCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var result = System.Text.RegularExpressions.Regex.Replace(
                input, "([A-Z])", " $1").Trim();
            return char.ToUpper(result[0]) + result.Substring(1);
        }

        private static string FriendlyName<T>()
            => SplitCamelCase(typeof(T).Name.Replace("Dto", "").Replace("DTO", ""));
    }
}
