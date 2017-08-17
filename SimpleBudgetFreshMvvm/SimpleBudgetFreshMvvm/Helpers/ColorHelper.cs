using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleBudgetFreshMvvm.Helpers
{
    public static class ColorHelper
    {
        // Code for generating colors from: https://stackoverflow.com/a/43235/815711
        static Random _random = new Random();
        public static string GenerateRandomPleasingColor()
        {
            Color baseColor = Color.FromHex("#e9abbc");
            var red = _random.NextDouble();
            var green = _random.NextDouble();
            var blue = _random.NextDouble();

            red = (red + baseColor.R) / 2.0;
            green = (green + baseColor.G) / 2.0;
            blue = (blue + baseColor.B) / 2.0;
            
            return $"#{(int)(red * 255):X2}{(int)(green * 255):X2}{(int)(blue * 255):X2}";
        }
    }
}
