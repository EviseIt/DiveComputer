using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abo.DiveComputer.Core;

namespace Abo.DiveComputer.WPF.ViewModels
{
    public class GradientFactorViewModel
    {
        private int _low;
        private int _high;





        public delegate void GradientFactorChangedEventHandler(object sender, GradientFactorChangedEventArgs e);

        public event GradientFactorChangedEventHandler GradientFactorChanged;

        public int Low
        {
            get => _low;
            set
            {
                if (value != _low)
                {
                    _low = value;
                    GradientFactorChanged?.Invoke(this, new GradientFactorChangedEventArgs(_low, High));
                }
            }
        }
        public int High
        {
            get => _high;
            set
            {
                if (value != _high)
                {
                    _high = value;
                    GradientFactorChanged?.Invoke(this, new GradientFactorChangedEventArgs(Low, _high));
                }
            }
        }

        public GradientFactorsSettings GradientFactorsSettings
        {
            get
            {
                return new GradientFactorsSettings(){High = _high,Low = _low};
            }
        }
        public GradientFactorViewModel()
        {
            _low = GradientFactorsSettings.Default.Low;
            _high = GradientFactorsSettings.Default.High;
        }
    }
}
