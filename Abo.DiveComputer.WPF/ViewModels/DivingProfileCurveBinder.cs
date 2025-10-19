//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using RealWorldPlot.Interfaces;

//namespace Abo.DiveComputer.WPF.ViewModels
//{
//    public  class DivingProfileCurveBinder: INotifyPropertyChanged
//    {


//        public RealWorldPlotter.RealWorldPlot DiveProfilePlot { get; set; }
//        public RealWorldPoints DiveProfile { get; set; }

//        private double _xMax = 60;
//        private double _xMin = 0;
//        private double _yMin = -30;
//        private double _yMax = 0;
//        public double XMin
//        {
//            get { return _xMin; }
//            set
//            {
//                if (_xMin != value)
//                {
//                    _xMin = value;
//                    OnPropertyChanged();
//                    DiveProfilePlot.XMin = _xMin;
//                    DiveProfilePlot.RoundXMin(_xMin);
//                }
//            }
//        }

//        public double XMax
//        {
//            get { return _xMax; }
//            set
//            {
//                if (_xMax != value)
//                {
//                    _xMax = value;
//                    OnPropertyChanged();
//                    DiveProfilePlot.XMax=_xMax;
//                    DiveProfilePlot.RoundXMax(_xMax);

//                }
//            }
//        }

//        public double YMin
//        {
//            get { return _yMin; }
//            set
//            {
//                if (_yMin != value)
//                {
//                    _yMin = value;
//                    OnPropertyChanged();
//                    DiveProfilePlot.YMin=_yMin;
//                    DiveProfilePlot.RoundYMin(_yMin);
//                }
//            }
//        }

//        public double YMax
//        {
//            get { return _yMax; }
//            set
//            {
//                if (_yMax != value)
//                {
//                    _yMax = value;
//                    OnPropertyChanged();
//                    DiveProfilePlot.YMax=_yMax;
//                    DiveProfilePlot.RoundYMax(_yMax);
//                }
//            }
//        }

//        public event PropertyChangedEventHandler? PropertyChanged;

//        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }

//    }
//}
