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
    private bool _isDragging = false;
    private bool _canMovePoints=false;
    public double SelectionTolerance { get; set; } = 20;
    
    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);
        Point clicked = e.GetPosition(this);

        double plotWidth = ActualWidth - MarginLeft - MarginRight;
        double plotHeight = ActualHeight - MarginTop - MarginBottom;
        RealWorldPoint realCoord = PixelToRealUnclamped(clicked);

        // Insertion Ctrl + clic gauche

        if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.ChangedButton == MouseButton.Left)
        {
            if (_mainCurvePoints.FindSurroundingPoints(realCoord, out IndexedRealWorldPoint p1, out IndexedRealWorldPoint p2))
            {
                InsertPointRequestEventArgs insertPointRequestEventArgs = new InsertPointRequestEventArgs(realCoord, p1);
                OnInsertPointRequest?.Invoke(this, insertPointRequestEventArgs);
                if (!insertPointRequestEventArgs.Cancel)
                {
                    _selectedPoint = _mainCurvePoints.InsertPoint(realCoord, p1);

                }
            }
            
        }

        //Sélection d’un point proche
        if (_mainCurvePoints != null)
        {
            IndexedRealWorldPoint? best = null;
            double minDist = double.MaxValue;

            _mainCurvePoints.EnumeratePoints(p =>
            {
                double dist = (RealToPixel(p, plotWidth, plotHeight) - clicked).LengthSquared;
                if (dist < minDist)
                {
                    minDist = dist;
                    best = p;
                }
            });

            if (best != null && minDist <= SelectionTolerance * SelectionTolerance)
            {
                _selectedPoint = best;
                OnPointSelectedHandler?.Invoke(this, new PointSelectedEventArgs(_selectedPoint));
                if (e.ChangedButton == MouseButton.Right)
                {
                    _contextMenu.PlacementTarget = this;
                    _contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
                    _contextMenu.IsOpen = true;
                }
                else if (e.ChangedButton == MouseButton.Left)
                {
                    _isDragging = true;
                    CaptureMouse();
                }
            }
            else
            {
                _selectedPoint = null;
                _isDragging = false;
                _contextMenu.IsOpen = false;
            }
        }

        //Invoke du click
        if (clicked.X >= MarginLeft && clicked.Y >= MarginTop && clicked.X <= ActualWidth - MarginRight && clicked.Y <= ActualHeight - MarginBottom)
        {
            RealWorldClick?.Invoke(realCoord);
        }

        _refreshVisual();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (CanMovePoints)
        {
            if (_isDragging && _selectedPoint != null)
            {
                Point pos = e.GetPosition(this);
                RealWorldPoint newReal = PixelToRealUnclamped(pos);
                double minX, maxX;
                if (_selectedPoint.PreviousPoint == null && _selectedPoint.NextPoint != null)
                {
                    //1er point
                    minX = this.XMin;
                    maxX = _selectedPoint.NextPoint.X;
                }
                else if (_selectedPoint.PreviousPoint != null && _selectedPoint.NextPoint != null)
                {
                    //point entre 2
                    minX = _selectedPoint.PreviousPoint.X;
                    maxX = _selectedPoint.NextPoint.X;
                }
                else if (_selectedPoint.PreviousPoint != null && _selectedPoint.NextPoint == null)
                {
                    //dernier point
                    minX = _selectedPoint.PreviousPoint.X;
                    maxX = this.XMax;
                }
                else
                {
                    //seul point
                    minX = this.XMin;
                    maxX = this.XMax;
                }



                // Vérifie contrainte d’abscisse entre voisins
                bool valid = newReal.X > minX && newReal.X < maxX && newReal.Y <= this.YMax && newReal.Y >= this.YMin;

                if (valid)
                {
                    _selectedPoint.UpdateFrom(newReal);
                    _refreshVisual();
                }

            }
        }
    }

    public bool BoundTriming
    {
        get;
        set;
    } = false;
    public bool CanMovePoints {
        get
        {
            return _canMovePoints && _curvesByKey.Count==1;
        }
        set
        {
            _canMovePoints=value;
        }
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);
        if (_isDragging)
        {
            _selectedPoint.EndUpdate();
            _isDragging = false;
            ReleaseMouseCapture();
        }
    }

   
}

