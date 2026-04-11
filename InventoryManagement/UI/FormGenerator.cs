using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI
{
    // ┌──────────────────────────────────────────────────────────────────────┐
    // │ LEGACY — kept for backward compatibility.                            │
    // │ New code should use CrudFormGenerator.ShowAddForm<T>() /             │
    // │ CrudFormGenerator.ShowEditForm<T>() and GenericListForm<T>.          │
    // └──────────────────────────────────────────────────────────────────────┘

    public static class FormGenerator
    {
        public static Form GenerateForm<T>(T model = default)
        {
            var form = new Form
            {
                Width = 400,
                Height = 400,
                Text = typeof(T).Name + " Form"
            };

            var properties = typeof(T).GetProperties();
            int top = 20;

            foreach (var prop in properties)
            {
                var label = new Label
                {
                    Text = prop.Name,
                    Left = 20,
                    Top = top,
                    Width = 100
                };

                Control input = CreateControl(prop, model);

                input.Left = 130;
                input.Top = top;
                input.Width = 200;
                input.Tag = prop;

                form.Controls.Add(label);
                form.Controls.Add(input);

                top += 40;
            }

            var btnSave = new Button
            {
                Text = "Save",
                Left = 130,
                Top = top
            };

            btnSave.Click += (s, e) =>
            {
                var instance = Activator.CreateInstance<T>();

                foreach (Control ctrl in form.Controls)
                {
                    if (ctrl.Tag is PropertyInfo prop)
                    {
                        object value = GetControlValue(ctrl, prop.PropertyType);

                        ValidateProperty(prop, value);

                        prop.SetValue(instance, value);
                    }
                }

                MessageBox.Show("Saved!");
            };

            form.Controls.Add(btnSave);
            return form;
        }

        private static Control CreateControl(PropertyInfo prop, object model)
        {
            var type = prop.PropertyType;

            if (type == typeof(bool))
            {
                var checkBox = new CheckBox();
                if (model != null)
                    checkBox.Checked = (bool)prop.GetValue(model);
                return checkBox;
            }

            var textBox = new TextBox();
            if (model != null)
                textBox.Text = prop.GetValue(model)?.ToString();

            return textBox;
        }

        private static object GetControlValue(Control ctrl, Type type)
        {
            if (ctrl is CheckBox cb)
                return cb.Checked;

            var text = ctrl.Text;

            if (type == typeof(int))
                return int.Parse(text);

            if (type == typeof(double))
                return double.Parse(text);

            return text;
        }

        private static void ValidateProperty(PropertyInfo prop, object value)
        {
            var required = prop.GetCustomAttribute<RequiredAttribute>();
            if (required != null && (value == null || string.IsNullOrEmpty(value.ToString())))
                throw new Exception($"{prop.Name} is required");
        }
    }
}
