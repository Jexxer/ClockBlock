using System;
using System.Globalization;
using System.Windows.Data;

namespace ClockBlock.GUI.Converters
{
    /// <summary>
    /// The InverseBooleanConverter class inverts a boolean value.
    /// Useful for data binding scenarios where a control needs the opposite value of a boolean property.
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to its inverse.
        /// </summary>
        /// <param name="value">The boolean value to invert.</param>
        /// <param name="targetType">The type of the target property. Not used in this converter.</param>
        /// <param name="parameter">An optional parameter. Not used in this converter.</param>
        /// <param name="culture">The culture information. Not used in this converter.</param>
        /// <returns>The inverse of the boolean value, or false if the input is not a boolean.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }

        /// <summary>
        /// Not implemented. Converts back from the target to the source.
        /// </summary>
        /// <param name="value">The value to convert back.</param>
        /// <param name="targetType">The type of the source property. Not used in this converter.</param>
        /// <param name="parameter">An optional parameter. Not used in this converter.</param>
        /// <param name="culture">The culture information. Not used in this converter.</param>
        /// <returns>Throws NotImplementedException as this converter does not support two-way binding.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Dont think we will need this, but just in case.
            throw new NotImplementedException("InverseBooleanConverter does not support ConvertBack.");
        }
    }
}
