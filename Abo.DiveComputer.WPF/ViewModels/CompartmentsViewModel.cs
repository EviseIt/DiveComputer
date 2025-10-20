using System;
using System.Collections;
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
    public class CompartmentsViewModel:IEnumerable<CompartmentViewModel>
    {
        private readonly Dictionary<BulhmanCompartment, CompartmentViewModel> _viewModelsByCompartment = new();

        public CompartmentsViewModel(BulhmanCompartments compartments)
        {
            this.Compartments = compartments;
            SolidColorBrush[] brushes = PaletteGenerator.GenerateDistinct(compartments.Count());
            int i = 0;
            foreach (var compartment in compartments)
            {
                _viewModelsByCompartment.Add(compartment, new CompartmentViewModel(compartment, brushes[i]));
                i++;
            }
        }

        public BulhmanCompartments Compartments { get; }

        public CompartmentViewModel this[BulhmanCompartment compartment]
        {
            get=> _viewModelsByCompartment [compartment];
        }

        public IEnumerator<CompartmentViewModel> GetEnumerator()
        {
            return _viewModelsByCompartment.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Refresh()
        {
           //TODO
        }
    }
}
