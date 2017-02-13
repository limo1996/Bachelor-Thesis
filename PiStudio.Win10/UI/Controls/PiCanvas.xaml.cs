﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using PiStudio.Win10.Data;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using PiStudio.Shared;
using Windows.Storage;
using System.IO;
using PiStudio.Win10;
using Windows.UI.Xaml;
using PiStudio.Shared.Data;

namespace PiStudio.Win10.UI.Controls
{
    public sealed partial class PiCanvas : UserControl, ISaveable
    {
        private List<SVGCurve> m_curves;
        private SVGCurve m_actualCurve;
        private uint m_pen;
        private bool m_isUnsavedChange = false;

        public PiCanvas()
        {
            this.InitializeComponent();
            m_canvas.Background = new SolidColorBrush(Colors.Transparent);
            ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;

            m_curves = new List<SVGCurve>();
            BrushThickness = 5;
            BrushColor = Colors.Black;

            m_canvas.PointerPressed += M_canvas_PointerPressed;
            m_canvas.PointerReleased += M_canvas_PointerReleased;
            m_canvas.PointerExited += M_canvas_PointerReleased;
            m_canvas.PointerMoved += M_canvas_PointerMoved;

            m_canvas.SizeChanged += (o, e) => OnSizeChanged(e);
        }

        public Color BrushColor { get; set; }
        public uint BrushThickness { get; set; }
        public ShapeType DrawingShape { get; set; }
        public bool IsEmpty { get { return m_curves.Count == 0; } }

        public bool HasUnsavedChange
        {
            get
            {
                return m_isUnsavedChange;
            }
        }

        public event EventHandler<PiCanvasContentChangedEventArgs> ContentChanged;

        #region EventHandlers
        private void M_canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (m_pen == e.Pointer.PointerId)
            {
                Point currPoint = e.GetCurrentPoint(m_canvas).Position;
                AddLine(currPoint, m_actualCurve.Data.Last(), BrushColor, BrushThickness);
                m_actualCurve.Data.Add(currPoint);
            }

            e.Handled = true;
        }

        private void M_canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (m_pen == e.Pointer.PointerId)
            {
                if (m_actualCurve != null && m_actualCurve.Data.Count > 0)
                {
                    m_curves.Add(m_actualCurve);
                    OnContentChanged(new PiCanvasContentChangedEventArgs() { Curve = m_actualCurve, Type = ContentChangedType.Added });
                }
            }

            //set pen ID to its default value
            m_pen = 0;

            e.Handled = true;
        }

        private void M_canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            m_pen = e.Pointer.PointerId;
            m_actualCurve = new SVGCurve()
            {
                Thickness = BrushThickness,
                Color = BrushColor,
                ShapeType = this.DrawingShape
            };
            m_actualCurve.Data.Add(e.GetCurrentPoint(m_canvas).Position);
            e.Handled = true;
        }
        #endregion

        private void AddLine(Point p1, Point p2, Color brushColor, float brushThickness)
        {
            Line line = new Line()
            {
                X1 = p1.X,
                X2 = p2.X,
                Y1 = p1.Y,
                Y2 = p2.Y,
                StrokeThickness = brushThickness,
                Stroke = new SolidColorBrush(brushColor),
                StrokeLineJoin = PenLineJoin.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                Fill = new SolidColorBrush(brushColor)
            };

            m_canvas.Children.Add(line);
        }

        private void AddRect(Point p1, Point p2, Color brushColor, float brushThickness)
        {
            Rectangle rect = new Rectangle()
            {
                Width = Math.Abs(p1.X - p2.X),
                Height = Math.Abs(p1.Y - p2.Y),
                StrokeThickness = brushThickness,
                Stroke = new SolidColorBrush(brushColor),
                StrokeLineJoin = PenLineJoin.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
            };
            Canvas.SetLeft(rect, Math.Min(p1.X, p2.X));
            Canvas.SetTop(rect, Math.Min(p1.Y, p2.Y));

            m_canvas.Children.Add(rect);
        }

        private void AddRectBack(Point p1, Point p2, Color brushColor, float brushThickness)
        {
            SVGCurve curve = new SVGCurve()
            {
                Thickness = brushThickness,
                Color = brushColor,
                ShapeType = ShapeType.Rectangle
            };
            curve.Data.Add(p1);
            curve.Data.Add(new Point(p2.X, p1.Y));
            curve.Data.Add(p2);
            curve.Data.Add(new Point(p1.X, p2.Y));
            curve.Data.Add(p1);
            m_curves.Add(curve);
        }

        private void ReloadCurves()
        {
            m_canvas.Children.Clear();
            foreach (var curve in m_curves)
            {
                for (int i = 0; i + 1 < curve.Data.Count; i++)
                    AddLine(curve.Data[i], curve.Data[i + 1], curve.Color, curve.Thickness);
            }
        }

        private void OnContentChanged(PiCanvasContentChangedEventArgs args)
        {
            m_isUnsavedChange = true;
            ContentChanged?.Invoke(this, args);
        }

        private void OnSizeChanged(SizeChangedEventArgs e)
        {
            var translateX = e.NewSize.Width / e.PreviousSize.Width;
            var translateY = e.NewSize.Height / e.PreviousSize.Height;

            foreach(var curve in m_curves)
            {
                for(int i = 0; i < curve.Data.Count; i++)
                {
                    Point p = new Point(curve.Data[i].X * translateX, curve.Data[i].Y * translateY);
                    curve.Data[i] = p;
                }
                curve.Thickness = (float)(curve.Thickness * translateX);
            }
            ReloadCurves();
        }

        public void Clear()
        {
            m_curves.Clear();
            ReloadCurves();
            OnContentChanged(new PiCanvasContentChangedEventArgs() { Type = ContentChangedType.Cleared });
            m_isUnsavedChange = false;
        }

        public void Undo()
        {
            if (m_curves.Count > 0)
            {
                var curve = m_curves[m_curves.Count - 1];
                m_curves.RemoveAt(m_curves.Count - 1);
                ReloadCurves();
                OnContentChanged(new PiCanvasContentChangedEventArgs() { Curve = curve, Type = ContentChangedType.Removed });
            }
            if (m_curves.Count == 0)
                m_isUnsavedChange = false;
        }

        public void SaveChanges()
        {
            m_isUnsavedChange = false;
        }

        public async Task Save(Stream stream)
        {
            m_isUnsavedChange = false;
            await SVGSaver.Save(stream.AsRandomAccessStream(), this.m_curves, this.Width, this.Height);
        }

        public void Dismiss()
        {
            Clear();
            m_isUnsavedChange = false;
        }
    }
}
