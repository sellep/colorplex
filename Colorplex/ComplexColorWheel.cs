using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Colorplex
{

    public class ComplexColorWheel
    {

        private class ComputationRequest
        {

            public ComputationRequest(int id, int w, int h, int ph, byte[] data, Func<Complex, Complex> transform)
            {
                Id = id;
                Width = w;
                Height = h;
                PartialHeight = ph;
                Data = data;
                Transform = transform;
            }

            public int Id { get; }

            public int Width { get; }

            public int Height { get; }

            public int PartialHeight { get; }

            public byte[] Data { get; }

            public Func<Complex, Complex> Transform { get; set; }
        }

        public static byte[] ComputeComplexColorWheel(int w, int h, Func<Complex, Complex> transform = null)
        {
            byte[] data = new byte[w * h * 3];

            Thread[] worker = new Thread[Environment.ProcessorCount];

            for (int t = 0; t < worker.Length; t++)
            {
                worker[t] = new Thread(Compute);
                worker[t].Start(new ComputationRequest(t, w, h, h / worker.Length, data, transform));
            }

            for (int t = 0; t < worker.Length; t++)
            {
                worker[t].Join();
            }

            return data;
        }

        private static void Compute(object data)
        {
            ComputationRequest request = (ComputationRequest) data;
            Complex c;

            byte[] rgb = new byte[3];

            for (int y = 0; y < request.PartialHeight; y++)
            {
                for (int x = 0; x < request.Width; x++)
                {
                    c = ComplexHelper.ConvertToComplex(request.Width, request.Height, x, request.Id * request.PartialHeight + y);

                    if (request.Transform != null)
                    {
                        c = request.Transform(c);
                    }

                    float angle = ComplexHelper.GetAngle(c);
                    float arg = ComplexHelper.GetArgument(c);

                    if (float.IsNaN(angle) || float.IsNaN(arg))
                    {
                        rgb[0] = 0;
                        rgb[1] = 0;
                        rgb[2] = 0;
                    }
                    else
                    {
                        ColorConverter.FromHSL(angle / 360, 1, 1 - (float)Math.Pow(2, -arg), ref rgb);
                    }

                    request.Data[((request.Id * request.PartialHeight + y) * request.Width + x) * 3 + 0] = rgb[2];
                    request.Data[((request.Id * request.PartialHeight + y) * request.Width + x) * 3 + 1] = rgb[1];
                    request.Data[((request.Id * request.PartialHeight + y) * request.Width + x) * 3 + 2] = rgb[0];
                }
            }
        }
    }
}
