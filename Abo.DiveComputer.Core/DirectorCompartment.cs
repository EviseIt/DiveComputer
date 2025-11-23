using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abo.DiveComputer.Core
{
    public class DirectorCompartment
    {
        public double MaxN2Tension { get; private set; }
        public BulhmanCompartment Compartment { get; private set; }

        public static DirectorCompartment None
        {
            get => new DirectorCompartment();
        }

        public void ComputeDirector(BulhmanCompartment compartment, double n2Tension,double ndl)
        {
            if (n2Tension > MaxN2Tension)
            {
                MaxN2Tension = n2Tension;
                Compartment = compartment;
            }

            if (ndl < this.Ndl && ndl>=0)
            {
                this.Ndl = ndl;
                System.Diagnostics.Debug.WriteLine("####"+ndl);
            }

            
        }

        public double Ndl { get; private set; } = double.PositiveInfinity;
    }
}
