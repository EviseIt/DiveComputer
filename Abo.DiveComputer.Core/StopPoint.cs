using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealWorldPlot.Interfaces;

namespace Abo.DiveComputer.Core
{
    public class StopPoint:RealWorldPoint
    {
        public StopPoint():base(0,0)
        {
            
        }
        public void AssignFrom(RealWorldPoint inter, BulhmanCompartment compartment)
        {
            this.X = inter.X;
            this.Y = inter.Y;
            this.Compartment = compartment;

        }

        public BulhmanCompartment Compartment { get; private set; }

        public StopPoint(double x, double y) : base(x, y)
        {
        }
    }
}
