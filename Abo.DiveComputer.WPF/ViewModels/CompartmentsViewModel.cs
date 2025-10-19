using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Abo.DiveComputer.Core;
using Abo.DiveComputer.WPF.Classes;

namespace Abo.DiveComputer.WPF.ViewModels
{
    public class CompartmentsViewModel
    {
        private readonly Dictionary<BulhmanCompartment, CompartmentViewModel> _viewModelsByCompartment = new();

        public CompartmentsViewModel(BulhmanCompartments compartments)
        {
            SolidColorBrush[] brushes = PaletteGenerator.GenerateDistinct(compartments.Count());
            int i = 0;
            foreach (var compartment in compartments)
            {
                _viewModelsByCompartment.Add(compartment, new CompartmentViewModel(compartment, brushes[i]));
                i++;
            }
        }

        public CompartmentViewModel this[BulhmanCompartment compartment]
        {
            get=> _viewModelsByCompartment [compartment];
        }
    }
}
