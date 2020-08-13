using System;
using System.Drawing;

namespace Nestor.Tools.Helpers
{
    public static class ColorHelper
    {
        /// <summary>
        /// Converti une couleur à partir de son code Hexa
        /// </summary>
        /// <param name="hexColor">valeur hexa de la couleur</param>
        /// <returns>Objet Color</returns>
        //public static Color FromHexa(string hexColor)
        //{
        //    return ColorTranslator.FromHtml(hexColor);
        //}

        /// <summary>
        /// Converti une couleur en code HTML
        /// </summary>
        /// <param name="color">couleur à transformer</param>
        /// <returns></returns>
        //public static string ToHtml(this Color color)
        //{
        //    return ColorTranslator.ToHtml(color);
        //}

        /// <summary>
        ///     Retourne la couleur donnée plus fonçée
        /// </summary>
        /// <param name="color">couleur d'origine</param>
        /// <returns>couleur plus foncée</returns>
        public static Color Dark(this Color color)
        {
            return ChangeColorBrightness(color, -0.5f);
        }

        /// <summary>
        ///     Retourne la couleur en plus clair
        /// </summary>
        /// <param name="color">Couleur d'origine</param>
        /// <returns>couleur éclaircie</returns>
        public static Color Light(this Color color)
        {
            return ChangeColorBrightness(color, 0.5f);
        }

        /// <summary>
        ///     Obtient la luminosité
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int GetLuminance(this Color source)
        {
            return (int) (0.2126 * source.R + 0.7152 * source.G + 0.0722 * source.B);
        }

        /// <summary>
        ///     Obtient si selon une couleur d'entrée, la couleur noire ou blanche sera la plus lisible
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Color GetBlackOrWhite(this Color source)
        {
            return GetLuminance(source) > 128 ? Color.Black : Color.White;
        }

        public static Color GetContrast(this Color source, bool preserveOpacity)
        {
            var inputColor = source;
            //if RGB values are close to each other by a diff less than 10%, then if RGB values are lighter side, decrease the blue by 50% (eventually it will increase in conversion below), if RBB values are on darker side, decrease yellow by about 50% (it will increase in conversion)
            var avgColorValue = (byte) ((source.R + source.G + source.B) / 3);
            var diff_r = Math.Abs(source.R - avgColorValue);
            var diff_g = Math.Abs(source.G - avgColorValue);
            var diff_b = Math.Abs(source.B - avgColorValue);
            if (diff_r < 20 && diff_g < 20 && diff_b < 20) //The color is a shade of gray
            {
                if (avgColorValue < 123) //color is dark
                    inputColor = Color.FromArgb(50, 230, 220);
                else
                    inputColor = Color.FromArgb(50, 255, 255);
            }

            var sourceAlphaValue = source.A;
            if (!preserveOpacity)
                sourceAlphaValue =
                    Math.Max(source.A, (byte) 127); //We don't want contrast color to be more than 50% transparent ever.
            var rgb = new RGB {R = inputColor.R, G = inputColor.G, B = inputColor.B};
            var hsb = ConvertToHSB(rgb);

            hsb.H = hsb.H < 180 ? hsb.H + 180 : hsb.H - 180;
            rgb = ConvertToRGB(hsb);

            return Color.FromArgb(sourceAlphaValue, (byte) rgb.R, (byte) rgb.G,
                (byte) rgb.B); // new Color { A = sourceAlphaValue, R = rgb.R, G = (byte)rgb.G, B = (byte)rgb.B };
        }

        /// <summary>
        ///     Transforme une couleur en rgba sous forme de chaine de caractère
        /// </summary>
        /// <param name="color">Couleur à transformer</param>
        /// <returns>Renvoi une couleur sous forme de chaine de caractère en forme rgba</returns>
        public static string ToRGBA(this Color color)
        {
            return string.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, color.A);
        }

        internal static RGB ConvertToRGB(HSB hsb)
        {
            // By: <a href="http://blogs.msdn.com/b/codefx/archive/2012/02/09/create-a-color-picker-for-windows-phone.aspx" title="MSDN" target="_blank">Yi-Lun Luo</a>
            var chroma = hsb.S * hsb.B;
            var hue2 = hsb.H / 60;
            var x = chroma * (1 - Math.Abs(hue2 % 2 - 1));
            var r1 = 0d;
            var g1 = 0d;
            var b1 = 0d;
            if (hue2 >= 0 && hue2 < 1)
            {
                r1 = chroma;
                g1 = x;
            }
            else if (hue2 >= 1 && hue2 < 2)
            {
                r1 = x;
                g1 = chroma;
            }
            else if (hue2 >= 2 && hue2 < 3)
            {
                g1 = chroma;
                b1 = x;
            }
            else if (hue2 >= 3 && hue2 < 4)
            {
                g1 = x;
                b1 = chroma;
            }
            else if (hue2 >= 4 && hue2 < 5)
            {
                r1 = x;
                b1 = chroma;
            }
            else if (hue2 >= 5 && hue2 <= 6)
            {
                r1 = chroma;
                b1 = x;
            }

            var m = hsb.B - chroma;
            return new RGB
            {
                R = r1 + m,
                G = g1 + m,
                B = b1 + m
            };
        }

        internal static HSB ConvertToHSB(RGB rgb)
        {
            // By: <a href="http://blogs.msdn.com/b/codefx/archive/2012/02/09/create-a-color-picker-for-windows-phone.aspx" title="MSDN" target="_blank">Yi-Lun Luo</a>
            var r = rgb.R;
            var g = rgb.G;
            var b = rgb.B;

            var max = Max(r, g, b);
            var min = Min(r, g, b);
            var chroma = max - min;
            var hue2 = 0d;
            if (chroma != 0)
            {
                if (max == r)
                    hue2 = (g - b) / chroma;
                else if (max == g)
                    hue2 = (b - r) / chroma + 2;
                else
                    hue2 = (r - g) / chroma + 4;
            }

            var hue = hue2 * 60;
            if (hue < 0) hue += 360;
            var brightness = max;
            double saturation = 0;
            if (chroma != 0) saturation = chroma / brightness;
            return new HSB
            {
                H = hue,
                S = saturation,
                B = brightness
            };
        }

        private static double Max(double d1, double d2, double d3)
        {
            if (d1 > d2) return Math.Max(d1, d3);
            return Math.Max(d2, d3);
        }

        private static double Min(double d1, double d2, double d3)
        {
            if (d1 < d2) return Math.Min(d1, d3);
            return Math.Min(d2, d3);
        }

        /// <summary>
        ///     Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">
        ///     The brightness correction factor. Must be between -1 and 1.
        ///     Negative values produce darker colors.
        /// </param>
        /// <returns>
        ///     Corrected <see cref="Color" /> structure.
        /// </returns>
        private static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            var red = (float) color.R;
            var green = (float) color.G;
            var blue = (float) color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int) green, (int) blue);
        }

        #region Structs

        internal struct RGB
        {
            internal double R;
            internal double G;
            internal double B;
        }

        internal struct HSB
        {
            internal double H;
            internal double S;
            internal double B;
        }

        #endregion
    }
}