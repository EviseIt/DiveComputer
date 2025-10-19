using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealWorldPlot.Interfaces;

namespace Abo.DiveComputer.Core
{

    public class Computer
    {
        public Computer(Diver diver)
        {
            this.Diver=diver;
        }

        public Diver Diver { get;  }

        public void AddDiveData(IndexedRealWorldPoint indexedRealWorldPoint, DirectorCompartment directorCompartment)
        {
            
        }

        public void AddDiveAirData(IndexedRealWorldPoint p0, double initPressureBar)
        {
            
        }
    }
}
