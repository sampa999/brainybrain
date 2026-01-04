using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace WebcamWpf
{
    public partial class MainWindow : Window
    {
        private VideoCapture _capture;
        private System.Windows.Threading.DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            LoadCameras();
        }

        private void LoadCameras()
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    using (var test = new VideoCapture(i))
                    {
                        if (test.IsOpened)
                            comboCameras.Items.Add($"Camera {i}");
                    }
                }
                catch { }
            }

            if (comboCameras.Items.Count > 0)
                comboCameras.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            int index = comboCameras.SelectedIndex;

            _capture?.Dispose();
            _capture = new VideoCapture(index);

            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(33); // ~30 FPS
            _timer.Tick += UpdateFrame;
            _timer.Start();
        }

        private void UpdateFrame(object sender, EventArgs e)
        {
            if (_capture == null || !_capture.IsOpened)
                return;

            using (Mat frame = _capture.QueryFrame())
            {
                if (frame != null)
                {
                    imagePreview.Source = ConvertToBitmapSource(frame);
                }
            }
        }

        private BitmapSource ConvertToBitmapSource(Mat mat)
        {
            using (var image = mat.ToImage<Bgr, byte>())
            {
                return BitmapSource.Create(
                    image.Width, image.Height,
                    96, 96,
                    System.Windows.Media.PixelFormats.Bgr24,
                    null,
                    image.Bytes,
                    image.Width * 3
                );
            }
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            if (_capture == null || !_capture.IsOpened)
                return;

            using (Mat frame = _capture.QueryFrame())
            {
                string filename = $"capture_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                frame.Save(filename);
                MessageBox.Show($"Saved: {filename}");
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _timer?.Stop();
            _capture?.Dispose();
            base.OnClosing(e);
        }
    }
}
