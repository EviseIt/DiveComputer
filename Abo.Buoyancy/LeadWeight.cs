using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abo.Buoyancy
{
    /// <summary>
    /// Plombs de plongée
    /// </summary>
    public partial class LeadWeight:IImmersible
    {
        private const decimal _leadDensityKgPerDm3 = 11.350m;
        public void Process(int depthMeter, int barLeft, decimal volumeVessie)
        {
            
        }

        public decimal VolumeDm3
        {
            get => WeightKg / _leadDensityKgPerDm3;
        }
    }
}


