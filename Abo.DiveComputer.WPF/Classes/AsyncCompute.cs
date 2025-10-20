using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abo.DiveComputer.Core;
using RealWorldPlot.Interfaces;

namespace Abo.DiveComputer.WPF.Classes
{
    internal class AsyncCompute
    {
        private readonly Thread _thread;
        private DiveProfile _diveProfile;
        private BulhmanCompartments _compartments;
        public event EventHandler OnComputationDone;
        

        public AsyncCompute()
        {
            _thread = new Thread(_run);
        }

        private void _run()
        {
           _compartments.ComputeDiveProfile(_diveProfile);
            OnComputationDone.Invoke(this,new EventArgs());
        }

        public void Compute(DiveProfile diveProfile, BulhmanCompartments compartments)
        {
            _diveProfile=diveProfile;
            _compartments = compartments;
            _thread.Start();
        }
    }
}
