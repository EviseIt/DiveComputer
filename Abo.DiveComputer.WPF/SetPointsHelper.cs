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

            plotMValues.AppendStaticData(compartment.MValues.Points, new PenInfo(Colors.Crimson, 1.5, new Double[] { 5.0, 5.0, 5.0 }));

            plotMValues.AppendStaticData(N2AmbiantPressure.GetInstance().Points, new PenInfo(Colors.Blue, 1.5, new Double[] { 5.0, 5.0, 5.0 }));

            plotMValues.AppendStaticData(compartment.GradientFactor.Points, new PenInfo(Colors.Aqua, 1.5, new Double[] { 5.0, 5.0, 5.0 }));

        }

        public void SetPoints(BulhmanCompartments compartments)
        {
            var start = DateTime.Now;
            foreach (var compartment in compartments)
            {
                _plotsTn2ByCompartment[compartment].SetPoints(compartments.GetTensions(compartment), new PenInfo(_compartmentsViewModel[compartment].Color, _penInfo.Width));
                _plotsMValueDataByCompartment[compartment].SetPoints(compartments.GetMValuesData(compartment), _penInfo, false);
                _plotsMValueDataByCompartment[compartment].XMax = compartments.GetMValuesData(compartment).MaxWorldX;
            }
            var end = DateTime.Now;
            Console.WriteLine($"SetPoints took {end - start} ms for all compartments.");


        }
    }
}
