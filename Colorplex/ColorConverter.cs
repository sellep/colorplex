using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colorplex
{

    public static class ColorConverter
    {
        public static void FromHSL(double H, double S, double L, ref byte[] rgb)
        {
            FromHSLA(H, S, L, 1.0, ref rgb);
        }

        private static void FromHSLA(double H, double S, double L, double A, ref byte[] rgb)
        {
            double v;
            double r, g, b;
            if (A > 1.0)
                A = 1.0;

            r = L;   // default to gray
            g = L;
            b = L;
            v = (L <= 0.5) ? (L * (1.0 + S)) : (L + S - L * S);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = L + L - v;
                sv = (v - m) / v;
                H *= 6.0;
                sextant = (int)H;
                fract = H - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            rgb[0] = Convert.ToByte(r * 255.0f);
            rgb[1] = Convert.ToByte(g * 255.0f);
            rgb[2] = Convert.ToByte(b * 255.0f);
        }
    }
}
