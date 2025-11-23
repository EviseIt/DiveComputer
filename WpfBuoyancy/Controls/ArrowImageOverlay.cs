// ArrowImageOverlay.cs

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfBuoyancy.Controls
{
    public class ArrowImageOverlay : FrameworkElement
    {
        // ===== Image & styling =====
        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(ArrowImageOverlay),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register(nameof(ImageWidth), typeof(double), typeof(ArrowImageOverlay),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register(nameof(ImageHeight), typeof(double), typeof(ArrowImageOverlay),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double ArrowHeadSize
        {
            get => (double)GetValue(ArrowHeadSizeProperty);
            set => SetValue(ArrowHeadSizeProperty, value);
        }
        public static readonly DependencyProperty ArrowHeadSizeProperty =
            DependencyProperty.Register(nameof(ArrowHeadSize), typeof(double), typeof(ArrowImageOverlay),
                new FrameworkPropertyMetadata(8.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(ArrowImageOverlay),
                new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ArrowBrush
        {
            get => (Brush)GetValue(ArrowBrushProperty);
            set => SetValue(ArrowBrushProperty, value);
        }
        public static readonly DependencyProperty ArrowBrushProperty =
            DependencyProperty.Register(nameof(ArrowBrush), typeof(Brush), typeof(ArrowImageOverlay),
                new FrameworkPropertyMetadata(Brushes.Crimson, FrameworkPropertyMetadataOptions.AffectsRender));

        // ===== Arrows as a FreezableCollection so items inherit DataContext =====
        public FreezableCollection<ArrowSpec> Arrows
        {
            get => (FreezableCollection<ArrowSpec>)GetValue(ArrowsProperty);
            set => SetValue(ArrowsProperty, value);
        }

        public static readonly DependencyProperty ArrowsProperty =
            DependencyProperty.Register(
                nameof(Arrows),
                typeof(FreezableCollection<ArrowSpec>),
                typeof(ArrowImageOverlay),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnArrowsChanged));

        private static void OnArrowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (ArrowImageOverlay)d;

            if (e.OldValue is FreezableCollection<ArrowSpec> oldCol)
                oldCol.Changed -= owner.Arrows_Changed;

            if (e.NewValue is FreezableCollection<ArrowSpec> newCol)
                newCol.Changed += owner.Arrows_Changed;

            owner.InvalidateVisual();
        }

        private void Arrows_Changed(object? sender, EventArgs e) => InvalidateVisual();

        // ===== Interaction state =====
        private Matrix _view = Matrix.Identity;
        private Point? _lastPanPos;
        private const double MinScale = 0.1;
        private const double MaxScale = 10.0;

        public ArrowImageOverlay()
        {
            Focusable = true;
            ClipToBounds = true;
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);

            // Ensure collection exists (lets XAML add items without wrapping)
            if (Arrows == null)
                Arrows = new FreezableCollection<ArrowSpec>();

            MouseLeftButtonDown += (_, e) =>
            {
                if (e.ClickCount == 2)
                {
                    ResetView();
                    e.Handled = true;
                }
            };
        }

        // ===== Mouse interaction =====
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            Focus();

            double step = (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) ? 1.05 : 1.1;
            double factor = e.Delta > 0 ? step : 1.0 / step;

            ZoomAt(factor, e.GetPosition(this));
            e.Handled = true;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            _lastPanPos = e.GetPosition(this);
            Cursor = Cursors.Hand;
            CaptureMouse();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_lastPanPos.HasValue && e.LeftButton == MouseButtonState.Pressed)
            {
                Point pos = e.GetPosition(this);
                Vector delta = pos - _lastPanPos.Value;

                var m = _view;
                m.Translate(delta.X, delta.Y);
                _view = m;

                _lastPanPos = pos;
                InvalidateVisual();
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            _lastPanPos = null;
            Cursor = Cursors.Arrow;
            ReleaseMouseCapture();
        }

        private void ResetView()
        {
            _view = Matrix.Identity;
            InvalidateVisual();
        }

        private void ZoomAt(double factor, Point pivotScreen)
        {
            double current = _view.M11; // uniform scale
            double target = Math.Clamp(current * factor, MinScale, MaxScale);
            double effective = target / current;
            if (Math.Abs(effective - 1.0) < 1e-6)
                return;

            var m = _view;
            m.Translate(-pivotScreen.X, -pivotScreen.Y);
            m.Scale(effective, effective);
            m.Translate(pivotScreen.X, pivotScreen.Y);
            _view = m;

            InvalidateVisual();
        }

        // ===== Rendering =====
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (Image == null)
                return;

            dc.PushTransform(new MatrixTransform(_view));

            // Determine image size (DIPs)
            double imgW, imgH;
            if (ImageWidth > 0 && ImageHeight > 0)
            {
                imgW = ImageWidth;
                imgH = ImageHeight;
            }
            else if (Image is BitmapSource bmp)
            {
                double dpiX = bmp.DpiX > 0 ? bmp.DpiX : 96.0;
                double dpiY = bmp.DpiY > 0 ? bmp.DpiY : 96.0;
                imgW = bmp.PixelWidth * (96.0 / dpiX);
                imgH = bmp.PixelHeight * (96.0 / dpiY);
            }
            else
            {
                imgW = ImageWidth > 0 ? ImageWidth : 100;
                imgH = ImageHeight > 0 ? ImageHeight : 100;
            }

            if (imgW <= 0 || imgH <= 0)
            {
                dc.Pop();
                return;
            }

            // Center image (pre-transform)
            double x = (ActualWidth - imgW) / 2.0;
            double y = (ActualHeight - imgH) / 2.0;
            Rect imageRect = new Rect(x, y, imgW, imgH);

            // Draw image
            dc.DrawImage(Image, imageRect);

            // Draw arrows
            if (Arrows != null)
            {
                foreach (var spec in Arrows)
                {
                    if (spec is null)
                        continue;

                    double thickness = double.IsNaN(spec.StrokeThickness) ? StrokeThickness : spec.StrokeThickness;
                    double head = double.IsNaN(spec.ArrowHeadSize) ? ArrowHeadSize : spec.ArrowHeadSize;

                    Brush upBrush = spec.UpBrush ?? spec.Brush ?? ArrowBrush;
                    Brush downBrush = spec.DownBrush ?? spec.Brush ?? ArrowBrush;

                    var penUp = new Pen(upBrush, thickness) { StartLineCap = PenLineCap.Flat, EndLineCap = PenLineCap.Flat };
                    var penDown = new Pen(downBrush, thickness) { StartLineCap = PenLineCap.Flat, EndLineCap = PenLineCap.Flat };

                    // Hook point in image-local coords
                    Point hpImg = spec.HookIsNormalized
                        ? new Point(spec.HookPoint.X * imgW, spec.HookPoint.Y * imgH)
                        : spec.HookPoint;

                    Point origin = new Point(imageRect.X + hpImg.X, imageRect.Y + hpImg.Y);
                    double downLength = spec.DownLength;
                    double upLength = spec.UpLength;

                    Point upTip = new Point(origin.X, origin.Y - upLength);
                    Point dnTip = new Point(origin.X, origin.Y + downLength);

                    DrawArrow(dc, penUp, origin, upTip, head);
                    DrawArrow(dc, penDown, origin, dnTip, head);
                }
            }

            dc.Pop();
        }

        private static void DrawArrow(DrawingContext dc, Pen pen, Point from, Point to, double head)
        {
            dc.DrawLine(pen, from, to);

            Vector dir = to - from;
            if (dir.Length <= 0.0001)
                return;
            dir.Normalize();
            Vector n = new Vector(-dir.Y, dir.X);

            double overshoot = head * 0.3;
            Point tip = to + (dir * overshoot);

            Point a = tip - (dir * head) + (n * (head * 0.6));
            Point b = tip - (dir * head) - (n * (head * 0.6));

            var geo = new StreamGeometry();
            using (var g = geo.Open())
            {
                g.BeginFigure(tip, true, true);
                g.LineTo(a, true, true);
                g.LineTo(b, true, true);
            }
            geo.Freeze();
            dc.DrawGeometry(pen.Brush, null, geo);
        }

        protected override Size MeasureOverride(Size availableSize) => availableSize;
        protected override Size ArrangeOverride(Size finalSize) => finalSize;
    }
}
