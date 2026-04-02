// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace InteractiveDataDisplay.WPF
{
    /// <summary>
    /// A plot to draw simple line.
    /// </summary>
    [Description("Plots line graph")]
    public class LineGraph : Plot
    {
        private Polyline polyline;
        private double dDisplayMin;
        private double dDisplayMax;

        /// <summary>
        /// Gets or sets line graph points.
        /// </summary>
        [Category("InteractiveDataDisplay")]
        [Description("Line graph points")]
        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }
        public ObservableCollection<Point> OCPointsAll = new ObservableCollection<Point>();
        
        private static void PointsPropertyChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineGraph linePlot = (LineGraph)d;
            if (linePlot != null)
            {
                InteractiveDataDisplay.WPF.Plot.SetPoints(linePlot.polyline, (PointCollection)e.NewValue);
            }
        }
        //private PlotBase masterPlot = null;

        /// <summary>
        /// Initializes a new instance of <see cref="LineGraph"/> class.
        /// </summary>
        public delegate void HideEdge(double dStart, double dEnd);
        public static event HideEdge EventHideEdge;
        public LineGraph()
        {
            polyline = new Polyline 
            { 
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeLineJoin = PenLineJoin.Round
            };
            MouseNavigation.EventRangeChanged += MouseNavigation_EventRangeChanged;
            //masterPlot = PlotBase.FindMaster(this);
            BindingOperations.SetBinding(polyline, Polyline.StrokeThicknessProperty, new Binding("StrokeThickness") { Source = this });
            BindingOperations.SetBinding(this, PlotBase.PaddingProperty, new Binding("StrokeThickness") { Source = this, Converter = new LineGraphThicknessConverter() });

            Children.Add(polyline);
        }

        private void MouseNavigation_EventRangeChanged(double dMin, double dMax)
        {
            //EventHideEdge(dMin, dMax);
            //if (OCPointsAll.Count > 0)
            //{
            //    ObservablePoints=FiltratePoints(dMin,dMax);
            //}
            ////throw new NotImplementedException();
        }

        static LineGraph()
        {
            PointsProperty.OverrideMetadata(typeof(LineGraph), new PropertyMetadata(new PointCollection(), PointsPropertyChangedHandler) );
        }

        /// <summary>
        /// Updates data in <see cref="Points"/> and causes a redrawing of line graph.
        /// </summary>
        /// <param name="x">A set of x coordinates of new points.</param>
        /// <param name="y">A set of y coordinates of new points.</param>
        public void Plot(IEnumerable x, IEnumerable y)
        {
            if (x == null)
                throw new ArgumentNullException("x");
            if (y == null)
                throw new ArgumentNullException("y");

            var points = new PointCollection();
            var enx = x.GetEnumerator();
            var eny = y.GetEnumerator();
            while (true)
            {
                var nx = enx.MoveNext();
                var ny = eny.MoveNext();
                if (nx && ny)
                    points.Add(new Point(Convert.ToDouble(enx.Current, CultureInfo.InvariantCulture), 
                        Convert.ToDouble(eny.Current, CultureInfo.InvariantCulture)));
                else if (!nx && !ny)
                    break;
                else
                    throw new ArgumentException("x and y have different lengthes");
            }

            Points = points;
        }

        /// <summary>
        /// Updates data in <see cref="Points"/> and causes a redrawing of line graph.
        /// In this version a set of x coordinates is a sequence of integers starting with zero.
        /// </summary>
        /// <param name="y">A set of y coordinates of new points.</param>
        public void PlotY(IEnumerable y)
        {
            if (y == null)
                throw new ArgumentNullException("y");
            int x = 0;
            var en = y.GetEnumerator();
            var points = new PointCollection();
            while (en.MoveNext())
                points.Add(new Point(x++, Convert.ToDouble(en.Current, CultureInfo.InvariantCulture)));

            Points = points;
        }

        #region Description
        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
           DependencyProperty.Register("Description",
           typeof(string),
           typeof(LineGraph),
           new PropertyMetadata(null,
               (s, a) =>
               {
                   var lg = (LineGraph)s;
                   ToolTipService.SetToolTip(lg, a.NewValue);
               }));

        /// <summary>
        /// Gets or sets description text for line graph. Description text appears in default
        /// legend and tooltip.
        /// </summary>
        [Category("InteractiveDataDisplay")]
        public string Description
        {
            get
            {
                return (string)GetValue(DescriptionProperty);
            }
            set
            {
                SetValue(DescriptionProperty, value);
            }
        }

        #endregion

        #region Thickness
        /// <summary>
        /// Identifies the <see cref="Thickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
           DependencyProperty.Register("StrokeThickness",
           typeof(double),
           typeof(LineGraph),
           new PropertyMetadata(1.0));

        /// <summary>
        /// Gets or sets the line thickness.
        /// </summary>
        /// <remarks>
        /// The default stroke thickness is 1.0
        /// </remarks>
        [Category("Appearance")]
        public double StrokeThickness
        {
            get
            {
                return (double)GetValue(StrokeThicknessProperty);
            }
            set
            {
                SetValue(StrokeThicknessProperty, value);
            }
        }
        #endregion

        #region Stroke

        /// <summary>
        /// Identifies the <see cref="Stroke"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
           DependencyProperty.Register("Stroke",
           typeof(Brush),
           typeof(LineGraph),
           new PropertyMetadata(new SolidColorBrush(Colors.Black), OnStrokeChanged));

        private static void OnStrokeChanged(object target, DependencyPropertyChangedEventArgs e)
        {
            LineGraph lineGraph = (LineGraph)target;
            lineGraph.polyline.Stroke = e.NewValue as Brush;
        }

        /// <summary>
        /// Gets or sets the brush to draw the line.
        /// </summary>
        /// <remarks>
        /// The default color of stroke is black
        /// </remarks>
        [Category("Appearance")]
        public Brush Stroke
        {
            get
            {
                return (Brush)GetValue(StrokeProperty);
            }
            set
            {
                SetValue(StrokeProperty, value);
            }
        }
        #endregion

        #region StrokeDashArray

        private static DoubleCollection EmptyDoubleCollection
        {
            get
            {
                var result = new DoubleCollection(0);
                result.Freeze();
                return result;
            }
        }

        /// <summary>
        /// Identifies the <see cref="StrokeDashArray"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register("StrokeDashArray",
                typeof(DoubleCollection),
                typeof(LineGraph),
                new PropertyMetadata(EmptyDoubleCollection, OnStrokeDashArrayChanged));

        private static void OnStrokeDashArrayChanged(object target, DependencyPropertyChangedEventArgs e)
        {
            LineGraph lineGraph = (LineGraph)target;
            lineGraph.polyline.StrokeDashArray = e.NewValue as DoubleCollection;
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="Double"/> values that indicate the pattern of dashes and gaps that is used to draw the line.
        /// </summary>
        [Category("Appearance")]
        public DoubleCollection StrokeDashArray
        {
            get
            {
                return (DoubleCollection)GetValue(StrokeDashArrayProperty);
            }
            set
            {
                SetValue(StrokeDashArrayProperty, value);
            }
        }
        #endregion
        private ObservableCollection<Point> FiltratePoints(double dMin,double dMax)
        {
            int k = 0;
            int h = 0;
            int iInterval = GetFilterInterval(dMin, dMax);
            ObservableCollection<Point> OPoints = new ObservableCollection<Point>();
            //ObservablePoints = new ObservableCollection<Point>();
            for (int i = 0; i < OCPointsAll.Count; i++)
            {
                if(i==OCPointsAll.Count-1)
                {
                    OPoints.Add(OCPointsAll[i]);
                }
                if((OCPointsAll[i].X >= dMin)&& (OCPointsAll[i].X <= dMax))
                {
                    if ((k++)%iInterval==0)
                    {
                        OPoints.Add(OCPointsAll[i]);
                    }
                    //k++;
                    //OPoints.Remove(OCPointsAll[i]);
                }
                else
                {
                    if ((h++) % 50 == 0)
                    {
                        OPoints.Add(OCPointsAll[i]);
                    }
                }
            }
            return OPoints;
        }
        private ObservableCollection<Point> FiltratePoints()
        {
            return FiltratePoints(OCPointsAll[0].X, OCPointsAll[OCPointsAll.Count-1].X);
        }
        private int GetFilterInterval(double dMin, double dMax)
        {
            int iInterval = 1;
            double d = dMax - dMin;
            if(d>1000)
            {
                iInterval = 25;
            }
            else if(d>200)
            {
                iInterval = 10;
            }
            else
            {
                iInterval = 1;
            }
            return iInterval;
        }


        public ObservableCollection<Point> ObservablePoints
        {
            get { return (ObservableCollection<Point>)GetValue(ObservablePointsProperty); }
            set { SetValue(ObservablePointsProperty, value); }
        }

        public static readonly DependencyProperty ObservablePointsProperty =
            DependencyProperty.RegisterAttached(
                "ObservablePoints",
                typeof(ObservableCollection<Point>),
                typeof(LineGraph),
                new PropertyMetadata(
                    new ObservableCollection<Point>(),
                    (d, e) =>
                    {
                        var linePlot = (LineGraph)d;
                        
                        var updateAction = new NotifyCollectionChangedEventHandler(
                        (o, args) =>
                                {
                                    if (linePlot != null)
                                    {
                                    //    linePlot.OCPointsAll = ;
                                    //    //linePlot.ObservablePoints = new PointCollection((ObservableCollection<Point>)linePlot.FiltratePoints());
                                    //    linePlot.ObservablePoints = new ObservableCollection<Point>();
                                    //    linePlot.ObservablePoints = linePlot.FiltratePoints();
                                        InteractiveDataDisplay.WPF.Plot.SetPoints(linePlot.polyline, new PointCollection((ObservableCollection<Point>)o));
                                    }
                                });

                        if (e.OldValue != null)
                        {
                            var coll = (INotifyCollectionChanged)e.OldValue;
                            coll.CollectionChanged -= updateAction;
                        }

                        if (e.NewValue != null)
                        {
                            var coll = (INotifyCollectionChanged)e.NewValue;
                            coll.CollectionChanged += updateAction;
                            if (linePlot != null)
                            {
               //                 linePlot.OCPointsAll = ((ObservableCollection<Point>)e.NewValue);
                                ////linePlot.ObservablePoints = new PointCollection((ObservableCollection<Point>)linePlot.FiltratePoints());
                                //linePlot.ObservablePoints = new ObservableCollection<Point>();
                                //linePlot.ObservablePoints = linePlot.FiltratePoints();
                                //SetPoints(linePlot.polyline, new PointCollection(linePlot.ObservablePoints));
                                InteractiveDataDisplay.WPF.Plot.SetPoints(linePlot.polyline, new PointCollection((ObservableCollection<Point>)e.NewValue));
                                //InteractiveDataDisplay.WPF.Plot.SetPoints(linePlot.polyline, new PointCollection((ObservableCollection<Point>)linePlot.FiltratePoints()));
                            }
                        }
                    }));
    }
    
    internal class LineGraphThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double thickness = (double)value;
            return new Thickness(thickness / 2.0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

