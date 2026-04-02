
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
//using static NPOI.XSSF.UserModel.Charts.XSSFLineChartData<Tx, Ty>;
//using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace WpfInteractiveDataDisplayTest1
{
    class MainViewModle : NotificationObject
    {
        public ICommand CMutify { set; get; }
        public ICommand ChangeDisplay { set; get; }
        public ICommand CommandExport { set; get; }
        public ICommand ChangeDisplay1 { set; get; }
        DispatcherTimer dt = new DispatcherTimer();
        public delegate void ChangeVisible(int iIndex);
        public static event ChangeVisible EventChangeVisible;
        public ObservableCollection<Point> PointsFromMyDatamodel { get; set; }
        //public ObservableCollection<Point>[] PointsData { get; set; }
        //private ObservableCollection<Point>[] pointsDataDisplay;
        //public ObservableCollection<Point>[] PointsDataDisplay
        //{
        //    get { return pointsDataDisplay; }
        //    set
        //    {
        //        pointsDataDisplay = value;
        //        RaisePropertyChanged("PointsDataDisplay");
        //        //EventHideEdge(dStartXDisplay, dEndXDisplay);
        //        Filtrator();
        //    }
        //}
        public ObservableCollection<Point> PD1 { get; set; }
        public ObservableCollection<Point> PD2 { get; set; }
        public const int iLines = 15;
        public const int iNumbers = 300;
        private double i_Height;
        public double iHeight
        {
            get { return i_Height; }
            set
            {
                i_Height = value;
                RaisePropertyChanged("iHeight");
            }
        }
        private double i_Width;
        public double iWidth
        {
            get { return i_Width; }
            set
            {
                i_Width = value;
                RaisePropertyChanged("iWidth");
            }
        }
        private double i_OriginX;
        public double iOriginX
        {
            get { return i_OriginX; }
            set
            {
                i_OriginX = value;
                RaisePropertyChanged("iOriginX");
            }
        }
        private double i_OriginY;
        public double iOriginY
        {
            get { return i_OriginY; }
            set
            {
                i_OriginY = value;
                RaisePropertyChanged("iOriginY");
            }
        }
        private double d_CurrentX;
        public double dCurrentX
        {
            get { return d_CurrentX; }
            set
            {
                d_CurrentX = value;
                RaisePropertyChanged("dCurrentX");
            }
        }
        private double d_CurrentY;
        public double dCurrentY
        {
            get { return d_CurrentY; }
            set
            {
                d_CurrentY = value;
                RaisePropertyChanged("dCurrentY");
            }
        }
        public ClassData[] PDatas { get; set; }
        //public delegate void HideEdge(double dStart,double dEnd);
        //public static event HideEdge EventHideEdge;
        //private double dStartXDisplay, dEndXDisplay;
        //private double dEdge;
        private PlotModel _myPlotModel;
        // 关键：使用 ObservableCollection 来存储数据点
        public ObservableCollection<DataPoint> DataPoints { get; set; }
        // 1. 定义 PlotModel 属性，并在 Setter 中通知 View 刷新
        public PlotModel MyPlotModel
        {
            get => _myPlotModel;
            set
            {
                _myPlotModel = value;
                // 假设 SetProperty 是你的通知方法
                //SetProperty(ref _myPlotModel, value);
                RaisePropertyChanged("MyPlotModel");
            }
        }
        public ObservableCollection<PlotModel> PlotModels { get; set; }
        public MainViewModle()               //构造函数
        {
            CMutify = new DelegateCommand(mutiEvent);
            ChangeDisplay = new DelegateCommand(ChangeDisplayFun);
            CommandExport = new DelegateCommand(ExportFun);
            ChangeDisplay1 = new DelegateCommand(ChangeDisplayFun1);
            //InteractiveDataDisplay.WPF.MouseNavigation.EventRangeChanged += MouseNavigation_EventRangeChanged;

            InitialDisplay();
            InitialOxyPlot();
            //InitTimer();
            //Chart.EventHideEdge += MainViewModle_EventHideEdge;
            //Plot.EventHideEdge += MainViewModle_EventHideEdge;
            //LineGraph.EventHideEdge += MainViewModle_EventHideEdge;
            //PlotBase.EventHideEdge += MainViewModle_EventHideEdge;
            //PointsDataDisplay[0].CollectionChanged += MainViewModle_CollectionChanged;

        }
        private void InitialOxyPlot()
        {
            // 2. 初始化 PlotModel
            x = new double[iNumbers];
            y = new double[iNumbers];
            PlotModels = new ObservableCollection<PlotModel>();
            PointsFromMyDatamodel = new ObservableCollection<Point>();
            PD1 = new ObservableCollection<Point>();
            DataPoints = new ObservableCollection<DataPoint>();
            PDatas = new ClassData[iLines];
            for (int i = 0; i < iLines; i++)
            {
                PDatas[i] = new ClassData();
                PlotModel pm = new PlotModel{ Title = "折线图"+(i+1).ToString() };
                pm.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X 轴" });//IsPanEnabled = false//IsZoomEnabled = false
                //pm.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X 轴",IsPanEnabled = false, IsZoomEnabled = false });//
                pm.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y 轴", Key = "YAxis" });
                pm.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Title = "Y1 轴", Minimum = 20, Maximum = 30, Key = "Y1Axis" });
                
                PlotModels.Add(pm);
            }
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = 0.1 * i;
                //y[i] = Math.Pow(Math.Abs(x[i] - iNumbers / 20), 0.66);
                y[i] =Math.Sin(x[i]);
                PointsFromMyDatamodel.Add(new Point(i, Math.Sin(x[i])));
                PD1.Add(new Point(i, 1 + Math.Sin(x[i])));
                //PD2.Add(new Point(i, 2.8 + Math.Sin(x[i])));
                DataPoints.Add(new DataPoint(x[i], y[i]));
                for (int j = 0; j < iLines; j++)
                {
                    //PDatas[j].PointsData.Add(new DataPoint(x[i] + j * 10, y[i]));
                    if (j == 3)
                    {
                        PDatas[j].PointsData.Add(new DataPoint(x[i], y[i] + 26.5));
                    }
                    else
                    {
                        PDatas[j].PointsData.Add(new DataPoint(x[i] + j * 2, y[i] + j * 2));
                    }
                }
                //DTimer_Tick(null, null);
            }
            MyPlotModel = new PlotModel { Title = "简单的折线图" };

            // 3. 配置坐标轴
            MyPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X 轴" });//IsPanEnabled = false//IsZoomEnabled = false
            //MyPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X 轴",IsPanEnabled = false, IsZoomEnabled = false });//
            MyPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y 轴", Key = "YAxis", PositionTier = 0});
            MyPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Title = "Y1 轴",Minimum= 20,Maximum = 30,Key="Y1Axis", PositionTier = 1 });
            MyPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "温度 (°C)",
                PositionTier = 2,
                StartPosition = 0,
                Key = "Temp",
                TextColor = OxyColors.Red
            });
            MyPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = " Temp2(°C)",
                PositionTier = 3,
                
                Key = "Temp2",
                TextColor = OxyColors.Red
            });
            MyPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = " Temp3(°C)",
                PositionTier = 4,

                Key = "Temp3",
                TextColor = OxyColors.Red
            });
            MyPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = " tt6(°C)",
                PositionTier = 6,

                Key = "Temp6",
                TextColor = OxyColors.Red
            });
            //DataPoints = new ObservableCollection<DataPoint>();
            UInt32 uColor = 0x0800000;
            OxyColor[] uColors = new OxyColor[] { OxyColors.Red, OxyColors.Green, OxyColors.Blue, OxyColors.DarkBlue, OxyColors.Black };
            for (int j = 0; j < iLines; j++)
            {
                var lineSeries = new LineSeries
                {
                    Title = "实时数据"+(j+1).ToString(),
                    // 关键步骤：将 LineSeries 的 ItemsSource 绑定到 ViewModel 的 DataPoints 属性
                    ItemsSource = PDatas[j].PointsData,

                    // 告诉 LineSeries 集合中的对象（DataPoint）哪个属性是 X 值，哪个是 Y 值
                    // 对于 DataPoint 对象，这通常可以省略，但对于自定义对象是必需的
                    YAxisKey=(j==3? "Y1Axis": "YAxis"),
                    DataFieldX = "X",
                    DataFieldY = "Y"
                };
                lineSeries.Color = OxyColors.Green;
                //lineSeries.Color = OxyColor.FromUInt32((UInt32)(0x7F00000 + j *uColor));
                if (j==2)
                {
                    lineSeries.TrackerFormatString = "";
                }
                //this.MyPlotModel.Series.Add(lineSeries);
                this.PlotModels[j].Series.Add(lineSeries);
            }
            var lineSeries1 = new LineSeries
            {
                Title = "实时数据",

                ItemsSource = DataPoints,

                DataFieldX = "X",
                DataFieldY = "Y",
                //MarkerType = MarkerType.Circle,   // ★ 显示小点
                //MarkerSize = 3,                   // ★ 点大小
                //MarkerStroke = OxyColors.Red,     // ★ 点颜色
                //MarkerFill = OxyColors.Yellow,    // ★ 点填充颜色
                //MarkerStrokeThickness = 1
            };

            var features = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Red,
                MarkerStroke = OxyColors.Black,
                MarkerSize = 5
            };

            // 加几个特征点，例如 x=2,5,8
            features.Points.Add(new ScatterPoint(2, Math.Sin(2)));
            features.Points.Add(new ScatterPoint(5, Math.Sin(5)));
            features.Points.Add(new ScatterPoint(8, Math.Sin(8)));
            MyPlotModel.Series.Add(features);

            var ls = new LineSeries
            {
                Title = "测试曲线",
                StrokeThickness = 2,
                Color = OxyColors.Blue,

                MarkerType = MarkerType.Circle,   // ★ 显示小点
                MarkerSize = 3,                   // ★ 点大小
                MarkerStroke = OxyColors.Red,     // ★ 点颜色
                MarkerFill = OxyColors.Yellow,    // ★ 点填充颜色
                MarkerStrokeThickness = 1
            };
            ls.Points.Add(new DataPoint(0, 1));
            ls.Points.Add(new DataPoint(10, -1));
            ls.Points.Add(new DataPoint(20, 1.5));
            ls.Points.Add(new DataPoint(40, 0));

            MyPlotModel.Series.Add(lineSeries1);
            MyPlotModel.Series.Add(ls);
            // 5. 刷新模型以确保图表更新
            MyPlotModel.InvalidatePlot(true);
        }
        private void MainViewModle_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
            string s = e.ToString();
            Filtrator();
        }

        private void MainViewModle_EventHideEdge(double dStart, double dEnd)
        {
            try
            {
                //dStartXDisplay = dStart;
                //dEndXDisplay = dEnd;
                //dEdge = (dEnd - dStart) * 0.1;
                //Filtrator();
                //EventHideEdge(dStartXDisplay, dEndXDisplay);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Filtrator()
        {
            //if (PointsData[0] == null) return;
            //PointsData[0].Clear();
            //for (int i = 0; i < PointsDataDisplay[0].Count(); i++)
            //{
            //    Point p = (Point)PointsDataDisplay[0][i];
            //    if ((p.X > dStartXDisplay + dEdge) && (p.X < dEndXDisplay - dEdge))
            //    {
            //        PointsData[0].Add(p);
            //    }
            //}
        }
        private void InitTimer()
        {
            dt.Interval = TimeSpan.FromMilliseconds(1000);
            dt.Tick += DTimer_Tick;
            dt.IsEnabled = true;

        }
        private void DTimer_Tick(object sender, EventArgs e)
        {

            try
            {
                double x, y;
                int i;
                //i = PointsDataDisplay[0].Count;
                i = PDatas[0].PointsDataDisplay.Count;
                x = i * 0.3;
                y = Math.Sin(x);
                for (int j = 0; j < iLines; j++)
                {
                    PDatas[j].PointsDataDisplay.Add(new Point(x + j * 10, y + j * 2));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void MouseNavigation_EventRangeChanged(double dMin, double dMax)
        {
            //throw new NotImplementedException();
            try
            {
                //Grid g = (Grid)cmg.Parent;
                //double dHeight = g.ActualHeight;
                //double dWidth = g.ActualWidth;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw;
            }
        }

        int ik;
        double[] x;
        double[] y;
        int iXMax = 5;
        int xMin = 5;
        int xMax = 65;
        int yMin = 5;
        int yMax = 15;

        //public LineGraph[] lgA;
        //public LineGraph lg;
        //public CircleMarkerGraph cmg;
       
        private void InitialMark()
        {
            //CircleMarkerGraph cmg = new CircleMarkerGraph();
            //cmg = new CircleMarkerGraph();
            //const int N = 2;
            //double[] x = new double[N] { 300, 400 };
            //double[] y = new double[N] { 5, 5 };
            //double[] c = new double[N] { 0.8, 0.8 };
            //double[] d = new double[N] { 18, 18 };
            //cmg.PlotColorSize(x, y, c, d);
            //lines.Children.Add(cmg);
        }
       // public Chart[] chart;
        const int iChartNumber = 20;
        Random rd = new Random();
        int k = 0;
        
        
    
    private void MainViewModle_MouseMove(object sender, MouseEventArgs e)
        {
            //Chart cha = (Chart)sender;
            //Point mousePosition = e.GetPosition(cha);
            ////Point dataPoint = plotter.Viewport.Transform.ScreenToData(mousePosition);
            ////dCurrentX = mousePosition.X;
            ////dCurrentY = mousePosition.Y;
            ////Point dataPoint = cha.Transform.ScreenToData(mousePosition);
            //dCurrentX = mousePosition.X;
            //dCurrentY = mousePosition.Y;
            //throw new NotImplementedException();
        }

        private void InitialDisplay()
        {

        }
        public void ShowMessage(object obj)       //消息 方法
        {
            //MessageBox.Show(obj.ToString());
        }
        public string MutiFun(string s1, string s2)
        {
            string sResult = string.Empty;
            int i1, i2, iRes;
            i1 = Convert.ToInt16(s1);
            i2 = Convert.ToInt16(s2);
            iRes = i1 * i2;
            sResult = iRes.ToString();
            return sResult;
        }
        private void mutiEvent(object o)
        {

            //LineSeries ls = (LineSeries)MyPlotModel.Series[0];
            //ls.Points.Add(new DataPoint(iXMax++, rd.Next(2,6)));
            //MyPlotModel.InvalidatePlot(true);
            
            DataPoints.Add(new DataPoint(iXMax++, rd.Next(2, 6)));
            LineSeries ls = (LineSeries)MyPlotModel.Series[0];
            ls.IsVisible = !ls.IsVisible;
            MyPlotModel.InvalidatePlot(true);
        }
        private void ChangeDisplayFun(object o)
        {
            
            var xAxis = this.MyPlotModel.Axes.FirstOrDefault(a => a.Position == AxisPosition.Bottom);
            if (xAxis != null)
            {
                // 2. 设置起始坐标 (Minimum) 和终止坐标 (Maximum)
                xAxis.Minimum = xMin;
                xAxis.Maximum = xMax;

                // 可选：如果希望在数据变化时轴的范围不自动调整，可以设置：
                // xAxis.IsZoomEnabled = false; 
                // xAxis.IsPanEnabled = false;
                xAxis.IsPanEnabled = true;
            }
            var yAxis = this.MyPlotModel.Axes.FirstOrDefault(a => a.Position == AxisPosition.Left);
            if (yAxis != null)
            {
                yAxis.Minimum = yMin;
                yAxis.Maximum = yMax;
            }
            MyPlotModel.InvalidatePlot(true);
            xMin--; xMax++; yMin--; yMax++;
        }

        private void ChangeDisplayFun1(object o)
        {
            //ineffective
            //iOriginX = 20f;
            //iOriginY = 21f;
            iHeight++;
            iWidth = 8f;
        }
        private void ExportFun(object o)
        {
            for (int i = 0; i < PlotModels.Count; i++)
            {


                PlotModels[i].Background = OxyColors.White;
            var pngExporter = new PngExporter
            {
                Width = 800,
                Height = 600,
            };

                pngExporter.ExportToFile(PlotModels[i], "plot"+(i+1).ToString()+".png");
            }
            MyPlotModel.Background = OxyColors.White;
            var pngExporter1 = new PngExporter
            {
                Width = 800,
                Height = 600,
            };

            pngExporter1.ExportToFile(MyPlotModel, "plot.png");
            //ExportToExcelFun();
            //ExportFun2();
        }
        private void ExportFun1(object o)
        {
            MyPlotModel.Background = OxyColors.White;
            var pngExporter = new PngExporter
            {
                Width = 800,
                Height = 600,
            };

            pngExporter.ExportToFile(MyPlotModel, "plot.png");
            
        }
       
      
       
        
       
        public static byte[] getJPGFromImage(BitmapSource imageC)
        {
            MemoryStream memStream = new MemoryStream();
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
    }
}
