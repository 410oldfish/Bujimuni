using UnityEngine;

namespace Lighten
{
    public static partial class Utility
    {
        public static class ColorHelper
        {
            private const float COLOR_1_OVER_255 = 1 / 255f;

            //颜色转换
            public static Color From255(int r, int g, int b, int a = 255)
            {
                return new Color(r * COLOR_1_OVER_255,
                    g * COLOR_1_OVER_255,
                    b * COLOR_1_OVER_255,
                    a * COLOR_1_OVER_255);
            }

            //16进制字串装换为UnityEngine.Color
            public static Color FromHex(string hex)
            {
                if (hex.Length == 6)
                {
                    int r = ToInt(hex.Substring(0, 2));
                    int g = ToInt(hex.Substring(2, 2));
                    int b = ToInt(hex.Substring(4, 2));
                    return From255(r, g, b);
                }
                if (hex.Length == 8)
                {
                    int r = ToInt(hex.Substring(0, 2));
                    int g = ToInt(hex.Substring(2, 2));
                    int b = ToInt(hex.Substring(4, 2));
                    int a = ToInt(hex.Substring(6, 2));
                    return From255(r, g, b, a);
                }

                return Color.white;
            }
            
            //UnityEngine.Color装换为16进制字串
            public static string ToHex(Color color)
            {
                int r = (int)(color.r * 255);
                int g = (int)(color.g * 255);
                int b = (int)(color.b * 255);
                int a = (int)(color.a * 255);
                var rhex = ToHex(r);
                var ghex = ToHex(g);
                var bhex = ToHex(b);
                var ahex = ToHex(a);
                if (string.IsNullOrEmpty(rhex))
                    rhex = "00";
                if (string.IsNullOrEmpty(ghex))
                    ghex = "00";
                if (string.IsNullOrEmpty(bhex))
                    bhex = "00";
                if (string.IsNullOrEmpty(ahex))
                    ahex = "00";
                return $"{rhex}{ghex}{bhex}{ahex}";
            }

            //16进制字串装换为整形数据
            private static int ToInt(string hex)
            {
                int num = 0;
                for (int i = 0; i < hex.Length; i++)
                {
                    int num2 = hex[i];
                    if (num2 >= 48 && num2 <= 57)
                    {
                        num2 -= 48;
                    }
                    else if (num2 >= 65 && num2 <= 70)
                    {
                        num2 -= 55;
                    }
                    else if (num2 >= 97 && num2 <= 102)
                    {
                        num2 -= 87;
                    }
                    else
                    {
                        num2 = 0;
                    }

                    num += num2 * (int)Mathf.Pow(16, hex.Length - i - 1);
                }

                return num;
            }

            //整形数据转换为16进制字串
            private static string ToHex(int value)
            {
                string str = string.Empty;
                while (value > 0)
                {
                    int num = value % 16;
                    str = num.ToString("X") + str;
                    value /= 16;
                }
                return str;
            }
        }

        //富文本上色
        public static string AddColor(this string str, Color color)
        {
            return $"<#{ColorHelper.ToHex(color)}>{str}</color>";
        }
    }
}