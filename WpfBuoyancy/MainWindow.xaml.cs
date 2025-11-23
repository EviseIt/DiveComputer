using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Abo.Buoyancy;
using WpfBuoyancy.ViewModels;

namespace WpfBuoyancy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image _img;
        private readonly DiverViewModel _diverViewModel;

        public MainWindow()
        {



            InitializeComponent();


            arrowImageOverlay.Image = new BitmapImage(
                new Uri(@"E:\Evise-IT\Code\DiveComputer\WpfBuoyancy\Images\scuba-diver.png", UriKind.Absolute));

            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"scuba-diver.png");
            _img = new Image
            {
                Width = 1000,
                Height = 900,
                Stretch = Stretch.None,
                Source = new BitmapImage(new Uri(path))
            };

            Canvas.SetLeft(_img, 0);
            Canvas.SetTop(_img, 0);
            // Optional Z-order
            Panel.SetZIndex(_img, 1);

            //Stage.Children.Add(_img);


            _diverViewModel = new DiverViewModel();
            this.DataContext = _diverViewModel;
            arrowImageOverlay.DataContext=_diverViewModel;
            //arrowPanel.ItemsSource = _diverViewModel.Arrows;

            //_diverViewModel.PropertyChanged += DiverViewModel_PropertyChanged;
            //_diverViewModel.RequestDrawingSize = () => arrowPanel.RenderSize;
            //Stage.SizeChanged += Stage_SizeChanged;
            //InitializeComponent();
            //DiverViewModel diverViewModel = new DiverViewModel();
            //this.DataContext = new DiverViewModel();
            //arrowPanel.ItemsSource=new Arrow[]
            //{

            //    new Arrow { Start = new(20,20), End = new(20,180), Stroke = System.Windows.Media.Brushes.CornflowerBlue, Thickness = 2 },
            //    new Arrow { Start = new(50,150), End = new(260,150), Stroke = System.Windows.Media.Brushes.IndianRed, HeadLength=16, HeadAngle=25 }
            //};
        }

        private void Stage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _img.Width = e.NewSize.Width;
            _img.Height = e.NewSize.Height;
            //Stage.UpdateLayout();
            //Stage.InvalidateVisual();
            //arrowPanel.ItemsSource = _diverViewModel.Arrows;
            //if (_diverViewModel.Arrows != null)
            //{
            //    foreach (var arrow in _diverViewModel.Arrows)
            //    {
            //        arrow.OffsetX = Stage.ActualWidth / 2;
            //    }
            //}
        }

        private void DiverViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Arrows")
            {
               // arrowPanel.ItemsSource = _diverViewModel.Arrows;
                if (_diverViewModel.Arrows != null)
                {
                    foreach (var arrow in _diverViewModel.Arrows)
                    {
                     //   arrow.ImageOffsetX = Stage.ActualWidth / 2;
                        
                    }
                }
            }
        }
    }
}