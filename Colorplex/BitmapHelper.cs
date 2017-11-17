using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Colorplex
{

    public static class BitmapHelper
    {

        public static BitmapSource Create(int w, int h, byte[] data)
        {
            BitmapSource bmp = BitmapSource.Create(w, h, 96, 96, PixelFormats.Bgr24, null, data, 3 * w);
            return bmp;
        }

        public static void Write(string file, BitmapSource image)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (Stream fs = File.OpenWrite(file))
            {
                encoder.Save(fs);
            }
        }
    }
}
