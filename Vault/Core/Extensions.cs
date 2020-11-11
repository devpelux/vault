using System.Windows.Media;

namespace Vault.Core
{
    public static class Extensions
    {
        public static Color Invert(this Color color)
        {
            return Color.FromRgb((byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));
        }
    }
}
