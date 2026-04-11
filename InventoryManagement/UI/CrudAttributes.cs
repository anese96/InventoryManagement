using System;

namespace InventoryManagement.UI
{
    /// <summary>
    /// Overrides the label text displayed next to a field in generated forms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormLabelAttribute : Attribute
    {
        public string Label { get; }
        public FormLabelAttribute(string label) => Label = label;
    }

    /// <summary>
    /// Specifies the type of control to generate for a property.
    /// </summary>
    public enum FormControlType
    {
        TextBox,
        NumericTextBox,   // TextBox with numeric-only key filter
        DecimalTextBox,   // TextBox with decimal key filter + N2 format on leave
        CheckBox,
        ComboBox,         // Requires DataSource set separately via ICrudFormContext
        DatePicker,
        ReadOnly          // Displayed as a disabled TextBox (useful for Id, RefProduct, etc.)
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FormControlAttribute : Attribute
    {
        public FormControlType ControlType { get; }
        /// <summary>Optional key used by ICrudFormContext to resolve a ComboBox data source.</summary>
        public string DataSourceKey { get; }

        public FormControlAttribute(FormControlType controlType, string dataSourceKey = null)
        {
            ControlType = controlType;
            DataSourceKey = dataSourceKey;
        }
    }

    /// <summary>
    /// Marks a property to be excluded from generated forms and list columns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormSkipAttribute : Attribute { }

    /// <summary>
    /// Marks a property as the display member when the object is shown in a parent ComboBox.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormDisplayAttribute : Attribute { }

    /// <summary>
    /// Specifies extra layout hints for a field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormLayoutAttribute : Attribute
    {
        /// <summary>0 = left column, 1 = right column (2-column layouts).</summary>
        public int Column { get; set; } = 0;
        public int Order { get; set; } = int.MaxValue;
    }
}
