using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Colorplex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Thread _Thread;
        private volatile bool _Terminate = false;

        public MainWindow()
        {
            InitializeComponent();

            _Thread = new Thread(Rendering);
            _Thread.Start();
        }

        private void Rendering()
        {
            const int w = 1920;
            const int h = 1080;

            double i = 0.001;
            int j = 0;

            Func<Complex, Complex> transform = c =>
            {
                return Complex.Sin(i / c) / i;
            };

            while (!_Terminate)
            {
                byte[] data = ComplexColorWheel.ComputeComplexColorWheel(w, h, transform);

                BitmapSource bmp = BitmapHelper.Create(w, h, data);
                BitmapHelper.Write(System.IO.Path.Combine(@"C:\Temp\colorwheel", $"img{j}.png"), bmp);

                bmp.Freeze();

                Dispatcher.BeginInvoke(new Action(() => _Target.ImageSource = bmp));

                i += 0.001;
                j++;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _Terminate = true;
        }
    }
}
