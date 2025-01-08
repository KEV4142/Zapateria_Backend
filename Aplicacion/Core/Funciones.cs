using System.Globalization;

namespace Aplicacion.Core;
public static class Funciones
{
    public static string ToProperCase(string text)
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(text.ToLower());
    }
}
