using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Colorplex
{

    public struct ComplexHelper
    {

        public static Complex ConvertToComplex(int w, int h, int x, int y)
        {
            return new Complex((x - ((float)w / 2)) / w * 2, (y - ((float)h / 2)) / h * 2);
        }

        public static float GetAngle(Complex c)
        {
            float angle = (float)(Math.Atan2(c.Imaginary, c.Real) * 180 / Math.PI);

            if (angle < 0)
                return angle + 360;

            return angle;
        }

        public static float GetArgument(Complex c)
        {
            float arg = (float)(Math.Sqrt(c.Real * c.Real + c.Imaginary * c.Imaginary) / Math.Sqrt(2));
            return arg;
        }
    }
}
