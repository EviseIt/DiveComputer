using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Abo.DiveComputer.Core;

namespace Abo.DiveComputer.WPF.ViewModels
{
    public class GasSettingsViewModel
    {
        private int _o2Percentage;
        public delegate void GasSettingsChangedEventHandler(object sender, GasSettingsChangedEventArgs e);

        public event GasSettingsChangedEventHandler GasSettingsChanged;
        public int O2Percentage
        {
            get => _o2Percentage;
            set
            {
                if (value != _o2Percentage)
                {
                    _o2Percentage = value;
                    GasSettingsChanged?.Invoke(this, new GasSettingsChangedEventArgs(_o2Percentage));
                }
            }
        }
        public GasSettings GasSettings
        {
            get
            {
                return new GasSettings() { O2Percentage = _o2Percentage };
            }
        }

        public GasSettingsViewModel()
        {
            _o2Percentage=GasSettings.Air.O2Percentage;
        }
    }

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
