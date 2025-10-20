using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealWorldPlot.Interfaces;

namespace Abo.DiveComputer.Core
{
    public class DiveProfile:RealWorldPoints
    {
        public DiveProfile()
        {
            
        }

        public DiveProfile(RealWorldPoints realWorldPoints)
        {
            assignFrom(realWorldPoints);
        }

        public double MaxAmbiantPressure
        {
            get
            {
                return Math.Abs(MinWorldY) / 10 + BulhmanCompartments.PSurfaceBar;

            }
        }

    }
}
