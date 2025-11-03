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
        public MainWindow()
        {



            InitializeComponent();
            DiverViewModel diverViewModel = new DiverViewModel();
            this.DataContext = diverViewModel;
            arrowPanel.ItemsSource = diverViewModel.Arrows;

            diverViewModel.PropertyChanged += DiverViewModel_PropertyChanged;
            diverViewModel.RequestDrawingSize= () => arrowPanel.RenderSize;
            //InitializeComponent();
            //DiverViewModel diverViewModel = new DiverViewModel();
            //this.DataContext = new DiverViewModel();
            //arrowPanel.ItemsSource=new Arrow[]
            //{

            //    new Arrow { Start = new(20,20), End = new(20,180), Stroke = System.Windows.Media.Brushes.CornflowerBlue, Thickness = 2 },
            //    new Arrow { Start = new(50,150), End = new(260,150), Stroke = System.Windows.Media.Brushes.IndianRed, HeadLength=16, HeadAngle=25 }
            //};
        }

        private void DiverViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Arrows")
            {
               DiverViewModel diverViewModel=(DiverViewModel)sender;
               arrowPanel.ItemsSource = diverViewModel.Arrows;
            }
        }
    }
}