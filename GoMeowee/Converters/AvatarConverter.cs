using System.Globalization;

namespace GoMeowee.Converters;

/// <summary>
/// Converts an avatar URL to an ImageSource, returning a default avatar when null or empty.
/// </summary>
public class AvatarConverter : IValueConverter
{
    private const string DefaultAvatar = "defaultavatar.png";

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string url && !string.IsNullOrWhiteSpace(url))
            return url;

        return DefaultAvatar;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
