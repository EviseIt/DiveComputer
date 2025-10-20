using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Abo.DiveComputer.Core;
using Abo.DiveComputer.WPF.ViewModels;
using RealWorldPlot.Interfaces;
using RealWorldPlot.Interfaces.GeometryHelpers;
using RealWorldPlotter;

namespace Abo.DiveComputer.WPF
{
    public class SetPointsHelper
    {
        public SetPointsHelper(CompartmentsViewModel compartmentsViewModel, PenInfo penInfo)
        {
            _penInfo = penInfo;
            _compartmentsViewModel = compartmentsViewModel;
        }
        private readonly Dictionary<BulhmanCompartment, RealWorldPlotter.RealWorldPlot> _plotsTn2ByCompartment = new();

        private readonly Dictionary<BulhmanCompartment, RealWorldPlotter.RealWorldPlot> _plotsMValueDataByCompartment = new();
        private readonly PenInfo _penInfo;
        private readonly CompartmentsViewModel _compartmentsViewModel;

        public void Register(TabItem tabItem, RealWorldPlotter.RealWorldPlot plotTn2, RealWorldPlotter.RealWorldPlot plotMValues, BulhmanCompartment compartment)
        {
            tabItem.DataContext = _compartmentsViewModel[compartment];

            _plotsTn2ByCompartment.Add(compartment, plotTn2);
            _plotsMValueDataByCompartment.Add(compartment, plotMValues);

            plotTn2.XAxisLabel = "Temps (minutes)";
            plotTn2.YAxisLabel = "Tension N2 (bar)";

            plotMValues.XAxisLabel = "Pression ambiante";
            plotMValues.YAxisLabel = "Tension N2 (bar)";

            plotMValues.XMin = 1;
            plotMValues.XMax = 7;
            plotMValues.YMin = 0;
            plotMValues.YMax = 6;

            plotMValues.XGraduation = new Graduation(1, 1);
            plotMValues.YGraduation = new Graduation(0.8, 1);

       
        }

        public void SetPoints()
        {
            var start = DateTime.Now;
            var compartments = _compartmentsViewModel.Compartments;
            _compartmentsViewModel.Refresh();
            foreach (var compartmentViewModel in _compartmentsViewModel)
            {
                BulhmanCompartment compartment = compartmentViewModel.Compartment;
                _plotsTn2ByCompartment[compartment].SetPoints(compartments.GetTensions(compartment), new PenInfo(_compartmentsViewModel[compartment].Color, _penInfo.Width));
                _plotsMValueDataByCompartment[compartment].SetPoints(compartments.GetMValuesData(compartment), _penInfo, false);
                _plotsMValueDataByCompartment[compartment].XMax = compartments.GetMValuesData(compartment).MaxWorldX;



                _plotsMValueDataByCompartment[compartment].ClearStaticData();

                var zz = compartments.DiveProfile;

                GfGraphViewModel gfGraphViewModel= compartmentViewModel.GetGfGraphViewModel();


                if (_compartmentsViewModel.Compartments.StopPoint != null)
                {
                    //Aficher le palier
                    _plotsMValueDataByCompartment[compartment].AppendStaticData(_compartmentsViewModel.Compartments.StopPoint, new PenInfo(Colors.Red, 2.0));
                }
                _plotsMValueDataByCompartment[compartment].AppendStaticData(gfGraphViewModel.MValuesPoints, gfGraphViewModel.MValuesPoints.PenInfo);
                _plotsMValueDataByCompartment[compartment].AppendStaticData(gfGraphViewModel.AmbiantPoints, gfGraphViewModel.AmbiantPoints.PenInfo);
                _plotsMValueDataByCompartment[compartment].AppendStaticData(gfGraphViewModel.GradientFactor, gfGraphViewModel.GradientFactor.PenInfo);

              

            }
            var end = DateTime.Now;
            Console.WriteLine($"SetPoints took {end - start} ms for all compartments.");


        }
    }
}
