using System.Collections.Generic;

namespace InventoryManagement.UI
{
    /// <summary>
    /// Implement this interface and pass it to <see cref="CrudFormGenerator"/> so that
    /// ComboBox fields can be populated with lookup data from your services.
    /// </summary>
    public interface ICrudFormContext
    {
        /// <summary>
        /// Return a list of items to bind to a ComboBox identified by <paramref name="dataSourceKey"/>.
        /// Each item should expose a display property (annotated with [FormDisplay]) and a value property (Id).
        /// </summary>
        IList<object> GetComboBoxItems(string dataSourceKey);

        /// <summary>Display member name used for ComboBox (e.g. "Name").</summary>
        string GetDisplayMember(string dataSourceKey);

        /// <summary>Value member name used for ComboBox (e.g. "Id").</summary>
        string GetValueMember(string dataSourceKey);
    }
}
