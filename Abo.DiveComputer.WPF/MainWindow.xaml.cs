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
using Abo.DiveComputer.Core;
using Abo.DiveComputer.WPF.Classes;
using Abo.DiveComputer.WPF.ViewModels;
using RealWorldPlot.Interfaces;
using RealWorldPlot.Interfaces.GeometryHelpers;
using RealWorldPlotter;
using static System.Net.Mime.MediaTypeNames;

namespace Abo.DiveComputer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SetPointsHelper _setPointsHelper;
        private readonly BulhmanCompartments _compartments;
        private RealWorldPoints _currentDiveProfile;
        private CompartmentsViewModel _compartmentsViewModel;
        private GradientFactorViewModel _gradientFactorViewModel;

        public MainWindow()
        {

            InitializeComponent();
            this.Loaded += (s, a) =>
            {
                _gradientFactorViewModel = (GradientFactorViewModel)this.Resources["GradientFactorViewModel"];
                _gradientFactorViewModel.GradientFactorChanged += (s, a) =>
                {
                    _compartments.GradientFactorsSettings=_gradientFactorViewModel.GradientFactorsSettings;
                };

            };
            BulhmanCompartments.DEBUG = true;
            Diver diver = new Diver();
          
            _compartments = diver.Compartments;
            _compartmentsViewModel = new CompartmentsViewModel(_compartments);
            _setPointsHelper = new SetPointsHelper(_compartmentsViewModel, new PenInfo(Colors.CornflowerBlue, 1.0));
            _setPointsHelper.Register(T0, plotC0Tn2, plotC0MValues, _compartments[0]);
            _setPointsHelper.Register(T1, plotC1Tn2, plotC1MValues, _compartments[1]);
            _setPointsHelper.Register(T1b, plotC1bTn2, plotC1bMValues, _compartments[2]);
            _setPointsHelper.Register(T2, plotC2Tn2, plotC2MValues, _compartments[3]);
            _setPointsHelper.Register(T3, plotC3Tn2, plotC3MValues, _compartments[4]);
            _setPointsHelper.Register(T4, plotC4Tn2, plotC4MValues, _compartments[5]);
            _setPointsHelper.Register(T5, plotC5Tn2, plotC5MValues, _compartments[6]);
            _setPointsHelper.Register(T6, plotC6Tn2, plotC6MValues, _compartments[7]);
            _setPointsHelper.Register(T7, plotC7Tn2, plotC7MValues, _compartments[8]);
            _setPointsHelper.Register(T8, plotC8Tn2, plotC8MValues, _compartments[9]);
            _setPointsHelper.Register(T9, plotC9Tn2, plotC9MValues, _compartments[10]);
            _setPointsHelper.Register(T10, plotC10Tn2, plotC10MValues, _compartments[11]);
            _setPointsHelper.Register(T11, plotC11Tn2, plotC11MValues, _compartments[12]);
            _setPointsHelper.Register(T12, plotC12Tn2, plotC12MValues, _compartments[13]);
            _setPointsHelper.Register(T13, plotC13Tn2, plotC13MValues, _compartments[14]);
            _setPointsHelper.Register(T14, plotC14Tn2, plotC14MValues, _compartments[15]);
            _setPointsHelper.Register(T15, plotC15Tn2, plotC15MValues, _compartments[16]);
            plot.XAxisLabel = "Temps (minutes)";
            plot.YAxisLabel = "Profondeur (mètres)";
            plot.OnInsertPointRequest += _onInsertPointRequest;
            plot.OnPointSelectedHandler += _onPointSelectedHandler;
            RealWorldPoints diveProfile = new RealWorldPoints();

            plot.SelectionVisualInfo.Color = Colors.DarkOrange;
            plot.XGraduation = new Graduation(0, 5);
            plot.YGraduation = new Graduation(0, 2);

            diveProfile.AddNewPoint(0, 0);
            diveProfile.AddNewPoint(1, -20);
            diveProfile.AddNewPoint(60, -20);
            diveProfile.AddNewPoint(63, 0);
            diveProfile.AddNewPoint(80, 0);

         
            var zz = diveProfile.Sample(5.0 / 60.0);




            plot.SetPoints(diveProfile, new PenInfo(Colors.CornflowerBlue, 1.0));
            diveProfile.OnDataChanged += Points_OnDataChanged;

            _computeAll(diveProfile);
            plot.RealWorldClick += p =>
            {

                // DiveComputer diveComputer = new DiveComputer(_dive);

                //MessageBox.Show($"Coordonnées : X = {p.X:F2}, Y = {p.Y:F2}");
            };


            //plot2.XGraduation = new Graduation(1, 1);
            //plot2.YGraduation = new Graduation(1, 1);

            _currentDiveProfile= diveProfile;

        }

        private void _onPointSelectedHandler(object sender, PointSelectedEventArgs point)
        {
            //plot3.SetPoints(   _dive.GetTensionHistoryOfPoint(point.SelectedPoint), new PenInfo(Colors.DarkGreen, 1.0));
        }

        private void  _computeAll(RealWorldPoints diveProfile)
        {
            _compartments.ComputeDiveProfile(diveProfile);
            _setPointsHelper.SetPoints();
            NDL.SetPoints(this._compartments.Ndl,new PenInfo(Color.FromRgb(0,0,255),1));


            All.XMin = 0;
            All.XMax = 0;
            All.YMin = 1;
            All.YMax = 0;
            All.BeginAppendPoints();
            foreach (var compartment in _compartments)
            {
                CompartmentViewModel compartmentViewModel=_compartmentsViewModel[compartment];
                All.AppendPoints(compartment,_compartments.GetTensions(compartment), new PenInfo(compartmentViewModel.Color, 1));
            }
            All.EndBeginAppendPoints();


        }
        private void Points_OnDataChanged(RealWorldPoints diveProfile)
        {
            _currentDiveProfile = diveProfile;
          

        }

        private void _onInsertPointRequest(object sender, InsertPointRequestEventArgs point)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            plot.XMax = 40;
        }

        private async void BtnCompute_OnClick(object sender, RoutedEventArgs e)
        {
            _compartments.GradientFactorsSettings=_gradientFactorViewModel.GradientFactorsSettings;
            await _computeAsync();
        }

        private async Task _computeAsync()
        {
            wait.Visibility = Visibility.Visible;
            AsyncCompute asyncCompute = new AsyncCompute();
            asyncCompute.OnComputationDone += AsyncCompute_OnComputationDone;
            asyncCompute.Compute(_currentDiveProfile, _compartments);

        }

        private void AsyncCompute_OnComputationDone(object? sender, EventArgs e)
        {
            if (Dispatcher.CheckAccess())
            {
                wait.Visibility = Visibility.Hidden;
                _setPointsHelper.SetPoints();
                NDL.SetPoints(this._compartments.Ndl, new PenInfo(Color.FromRgb(0, 0, 255), 1));


                All.XMin = 0;
                All.XMax = 0;
                All.YMin = 1;
                All.YMax = 0;
                All.BeginAppendPoints();
                foreach (var compartment in _compartments)
                {
                    CompartmentViewModel compartmentViewModel = _compartmentsViewModel[compartment];
                    All.AppendPoints(compartment,_compartments.GetTensions(compartment), new PenInfo(compartmentViewModel.Color, 1));
                }
                All.EndBeginAppendPoints();
            }
            else
            {
                // On est dans un thread de fond → on repasse par le Dispatcher
                Dispatcher.Invoke(() => AsyncCompute_OnComputationDone(sender, e));
            }
        }

        
    }


}