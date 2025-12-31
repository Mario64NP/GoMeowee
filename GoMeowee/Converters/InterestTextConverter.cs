using System.Globalization;

namespace GoMeowee.Converters;

public class InterestTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool IsInterested)
            return IsInterested ? "Interested ☑️" : "I'm interested";
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
