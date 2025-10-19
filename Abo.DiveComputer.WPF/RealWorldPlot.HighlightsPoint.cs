using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using RealWorldPlot.Interfaces;

namespace RealWorldPlotter;

public partial class RealWorldPlot : Canvas,INotifyPropertyChanged
{
    private readonly SpotCollection _spotCollection = new();
    public void SpotPoint(RealWorldPoint realWorldPoint,Color color,double radius)
    {
        if (_curvesByKey.Count > 0)
        {
            if (_spotCollection != null && !_spotCollection.Contains(realWorldPoint))
            {
                _spotCollection.AddSpot(realWorldPoint, color, radius);
            }
        }
    }
    //public void Highlight(Guid pointIndex)
    //{
    //    if (_curvesByKey.Count > 0)
    //    {
    //        IndexedRealWorldPoint? indexedRealWorldPoint=  _curvesByKey[MainCurve.Main].Points.FindById(pointIndex);
    //        if (indexedRealWorldPoint != null && !_highlightedPoints.Contains(indexedRealWorldPoint))
    //        {
    //            _highlightedPoints.Add(indexedRealWorldPoint)
    //        }
    //    }
    //}

}