using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

namespace WebcamTry1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            var videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            var bitmap = eventArgs.Frame;
            var bytes = ImageToByte(bitmap);
        }

        public static Array2d ImageToByte(Bitmap img)
        {
            ImageConverter converter = new ImageConverter();
            var bytes = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return new Array2d(bytes, img.Width, img.Height);
        }
    }
}
