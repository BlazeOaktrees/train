using InteractiveDataDisplay.WPF;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
        public const int iLines = 2;
        public const int iNumbers = 100;
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
        public MainViewModle()               //构造函数
        {
            CMutify = new DelegateCommand(mutiEvent);
            ChangeDisplay = new DelegateCommand(ChangeDisplayFun);
            CommandExport = new DelegateCommand(ExportFun);
            ChangeDisplay1 = new DelegateCommand(ChangeDisplayFun1);
            //InteractiveDataDisplay.WPF.MouseNavigation.EventRangeChanged += MouseNavigation_EventRangeChanged;

            InitialDisplay();
            Initial();
            InitTimer();
            //Chart.EventHideEdge += MainViewModle_EventHideEdge;
            //Plot.EventHideEdge += MainViewModle_EventHideEdge;
            //LineGraph.EventHideEdge += MainViewModle_EventHideEdge;
            //PlotBase.EventHideEdge += MainViewModle_EventHideEdge;
            //PointsDataDisplay[0].CollectionChanged += MainViewModle_CollectionChanged;

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
                Grid g = (Grid)cmg.Parent;
                double dHeight = g.ActualHeight;
                double dWidth = g.ActualWidth;
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
        public LineGraph[] lgA;
        public LineGraph lg;
        public CircleMarkerGraph cmg;
        private void Initial1()
        {

            iOriginX = 10;
            iOriginY = 11;
            iHeight = 8;
            iWidth = 4;
            x = new double[iNumbers];
            y = new double[iNumbers];
            PointsFromMyDatamodel = new ObservableCollection<Point>();
            PD1 = new ObservableCollection<Point>();
            PD2 = new ObservableCollection<Point>();
            //PointsData = new ObservableCollection<Point>[iLines];
            //PointsDataDisplay = new ObservableCollection<Point>[iLines];
            PDatas = new ClassData[iLines];
            for (int i = 0; i < iLines; i++)
            {
                PDatas[i] = new ClassData();
                //PointsData[i] = new ObservableCollection<Point>();
                //PointsDataDisplay[i] = new ObservableCollection<Point>();
            }
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = 0.1 * i;
                y[i] = Math.Pow(Math.Abs(x[i] - iNumbers / 20), 0.66);
                //PointsFromMyDatamodel[i] = new Point(i, Math.Sin(x[i]));
                PointsFromMyDatamodel.Add(new Point(i, Math.Sin(x[i])));
                PD1.Add(new Point(i, 1 + Math.Sin(x[i])));
                PD2.Add(new Point(i, 2.8 + Math.Sin(x[i])));
                //for (int j = 0; j < iLines; j++)
                //{
                //    PointsData[j].Add(new Point(x[i]+j*10, y[i]));
                //}
                DTimer_Tick(null, null);
            }
            lgA = new LineGraph[iLines];
            lg = new LineGraph();
            lg.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, (byte)(255), 0));
            lg.Description = String.Format("Data series {0}", 99);
            lg.StrokeThickness = 2;
            Binding b = new Binding();
            b.Path = new PropertyPath("PointsFromMyDatamodel");
            b.Mode = BindingMode.TwoWay;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            lg.SetBinding(LineGraph.ObservablePointsProperty, b);
            for (int i = 0; i < iLines; i++)
            {
                //var lg = new LineGraph();
                lgA[i] = new LineGraph();
                //lgA[i].Name = "L" + i.ToString();
                //lines.Children.Add(lgA[i]);
                lgA[i].Stroke = new SolidColorBrush(Color.FromArgb(255, 0, (byte)(i * 10), 0));
                lgA[i].Description = String.Format("Data series {0}", i + 1);
                lgA[i].StrokeThickness = 2;
                //lgA[i].OCPointsAll = PointsData[i];
                //lgA[i].Plot(x, x.Select(v => 3 * i + Math.Sin(v)).ToArray());
                Binding b1 = new Binding();
                b1.Path = new PropertyPath(String.Format("PDatas[{0}].PointsData", i));
                b1.Mode = BindingMode.TwoWay;
                b1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                lgA[i].SetBinding(LineGraph.ObservablePointsProperty, b1);

            }
            InitialMark();
        }
        private void InitialMark()
        {
            //CircleMarkerGraph cmg = new CircleMarkerGraph();
            cmg = new CircleMarkerGraph();
            const int N = 2;
            double[] x = new double[N] { 300, 400 };
            double[] y = new double[N] { 5, 5 };
            double[] c = new double[N] { 0.8, 0.8 };
            double[] d = new double[N] { 18, 18 };
            cmg.PlotColorSize(x, y, c, d);
            //lines.Children.Add(cmg);
        }
        public Chart[] chart;
        const int iChartNumber = 20;
        int k = 0;
        private void Initial()
        {
            x = new double[iNumbers];
            y = new double[iNumbers];
            PDatas = new ClassData[iChartNumber * iLines];
            for (int i = 0; i < iChartNumber * iLines; i++)
            {
                PDatas[i] = new ClassData();
            }
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = 0.1 * i;
                y[i] = Math.Pow(Math.Abs(x[i] - iNumbers / 20), 0.66);
                for (int k = 0; k < iChartNumber; k++)
                {
                    PDatas[2 * k].PointsDataDisplay.Add(new Point(i, 1 + Math.Sin(x[i])));
                    PDatas[2 * k + 1].PointsDataDisplay.Add(new Point(i, 2.8 + Math.Sin(x[i])));
                }

                DTimer_Tick(null, null);
            }
            lgA = new LineGraph[iChartNumber * iLines];
            for (int i = 0; i < iChartNumber * iLines; i++)
            {
                lgA[i] = new LineGraph();
                lgA[i].Stroke = new SolidColorBrush(Color.FromArgb(255, 0, (byte)(i * 10), 0));
                lgA[i].Description = String.Format("Data series {0}", i + 1);
                lgA[i].StrokeThickness = 2;
                Binding b1 = new Binding();
                b1.Path = new PropertyPath(String.Format("PDatas[{0}].PointsDataDisplay", i));
                b1.Mode = BindingMode.TwoWay;
                b1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                lgA[i].SetBinding(LineGraph.ObservablePointsProperty, b1);

            }

            chart = new Chart[iChartNumber];
            for (int i = 0; i < iChartNumber; i++)
            {
                chart[i] = new Chart();
                chart[i].PlotWidth = 120;
                chart[i].MouseMove += Chart_MouseMove;
                //chart[i].MouseMove += MainViewModle_MouseMove;
                //chart[i].PlotHeight = 5;
                Binding b1 = new Binding();
                b1.Path = new PropertyPath(String.Format("iHeight"));
                chart[i].SetBinding(Chart.PlotHeightProperty, b1);
                chart[i].IsVerticalNavigationEnabled = false;
                chart[i].IsHorizontalNavigationEnabled = false;
                chart[i].Margin = new Thickness(10);
                chart[i].Width = 400;
                chart[i].Height = 300;
                chart[i].LegendVisibility = Visibility.Collapsed;
                Grid g = new Grid();

                g.Children.Add(lgA[2 * i]);
                g.Children.Add(lgA[2 * i + 1]);
                //g.Margin = new Thickness(5);
                chart[i].Content = g;
            }


        }
        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            Chart chart = (Chart)sender;

            // Get mouse position relative to chart
            Point mousePosition = e.GetPosition(chart);

            // Convert screen coordinates to data coordinates using LiveCharts
            //ChartValues<double> xValues = chart.Series[0].Values as ChartValues<double>;
            //ChartValues<double> yValues = chart.Series[1].Values as ChartValues<double>;
            //double xScale = chart.AxisX[0].ActualWidth / (chart.AxisX[0].MaxValue - chart.AxisX[0].MinValue);
            //double yScale = chart.AxisY[0].ActualHeight / (chart.AxisY[0].MaxValue - chart.AxisY[0].MinValue);
            //double dataX = (mousePosition.X / xScale) + chart.AxisX[0].MinValue;
            //double dataY = (chart.ActualHeight - mousePosition.Y) / yScale + chart.AxisY[0].MinValue;

            // Now you have the data coordinates
            //double dCurrentX = dataX;
            //double dCurrentY = dataY;

            // Do something with dCurrentX and dCurrentY...
        }
    
    private void MainViewModle_MouseMove(object sender, MouseEventArgs e)
        {
            Chart cha = (Chart)sender;
            Point mousePosition = e.GetPosition(cha);
            //Point dataPoint = plotter.Viewport.Transform.ScreenToData(mousePosition);
            //dCurrentX = mousePosition.X;
            //dCurrentY = mousePosition.Y;
            //Point dataPoint = cha.Transform.ScreenToData(mousePosition);
            dCurrentX = mousePosition.X;
            dCurrentY = mousePosition.Y;
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

        }
        private void ChangeDisplayFun(object o)
        {
            //PointsFromMyDatamodel.Add(new Point(3000,1));
            //const int N = 2;
            //double[] x = new double[N] { 800, 900 };
            //double[] y = new double[N] { 15, 15 };
            //double[] c = new double[N] { 0.8, 0.8 };
            //double[] d = new double[N] { 8, 8 };
            //cmg.PlotColorSize(x, y, c, d);
            for (int i = 0; i < iChartNumber * iLines; i++)
            {
                
                lgA[i].Description = String.Format("Data series {0} Height {1}", i + 1, iHeight);
            }
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

            ExportToExcelFun();
            //ExportFun2();
        }
        private void ExportToExcelFun()
        {
            BitmapSource[] bmp = new BitmapSource[1];
            //for (int i = 0; i < bmp.Length; i++)
            //{
            //    bmp[i] = CreateBitmap();
            //}
            //Grid g = (Grid)chart[18].Parent;
            //GroupBox gb = (GroupBox)g.Parent;
            bmp[0] = CreateBitmap(chart[18]);
            SaveImageToExcel(bmp, @"E:\tzf\fileTest\20210106\WpfInteractiveDataDisplayTest32(20210116SomeChartsToImageExcel)\WpfInteractiveDataDisplayTest1\bin\Debug\Template.xlsx");
        }
        private void ExportFun1()
        {
            for (int i = 0; i < iChartNumber; i++)
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap(400, 300, 96, 96, PixelFormats.Default);
                rtb.Render(chart[i]);
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));
                string file = "d:\\t\\TestImage\\" + DateTime.Now.ToString("yyyyMMddHHmmss" + i.ToString()) + ".png";
                using (Stream stm = File.Create(file))
                {
                    encoder.Save(stm);
                }
            }
        }
        private void ExportFun2()
        {
            for (int i = 0; i < iChartNumber; i++)
            {
                BitmapSource bmp = CreateBitmap(chart[i]);
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                string file = "d:\\t\\TestImage\\" + DateTime.Now.ToString("yyyyMMddHHmmss" + i.ToString()) + ".png";
                using (Stream stm = File.Create(file))
                {
                    encoder.Save(stm);
                }
            }
        }
        private BitmapSource CreateBitmap(Chart chart)
        {
            Grid g = (Grid)chart.Parent;
            GroupBox gb = (GroupBox)g.Parent;
            var wantRanderSize = new Size(gb.ActualWidth + 20, gb.ActualHeight + 20);
            gb.Measure(wantRanderSize);

            gb.Arrange(new Rect(new Point(0 - gb.Margin.Left, 0 - gb.Margin.Top), wantRanderSize));
            //chart.Arrange(new Rect(new Point(-10,-10), wantRanderSize));
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)gb.ActualWidth, (int)gb.ActualHeight, 96, 96, PixelFormats.Default);
            bmp.Render(gb);
            return bmp;
        }
        //private BitmapSource CreateNotRanderElementScreenshot(UIElement element)
        //{
        //    var wantRanderSize = new Size(300, 300);
        //    element.Measure(wantRanderSize);

        //    element.Arrange(new Rect(new Point(0, 0), wantRanderSize));

        //    return CreateElementScreenshot(element);
        //}
        //private BitmapSource CreateElementScreenshot(Visual visual)
        //{

        //    RenderTargetBitmap bmp = new RenderTargetBitmap((int)RenderSize.Width, (int)RenderSize.Height, 96, 96, PixelFormats.Default);
        //    bmp.Render(visual);

        //    return bmp;
        //}
        private void TestToImage()
        {
            /*
            //图片
            //SetCellValue(pos11[8], sheet0, test.IntervalTime);
            string picPath = System.Environment.CurrentDirectory + "\\temp\\data.png";
            FileStream picFs = File.OpenRead(picPath); //OpenRead
            int filelength = 0;
            filelength = (int)picFs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 

            picFs.Read(image, 0, filelength); //按字节流读
            int pictureIdx = xssfWorkbookDown.AddPicture(image, PictureType.PNG);
            IDrawing patriarch = sheet0.CreateDrawingPatriarch();
            // 插图片的位置  XSSFClientAnchor（dx1,dy1,dx2,dy2,col1,row1,col2,row2) 
            //参数表示图片的位置.文章尾部有说明
            IClientAnchor anchor = new XSSFClientAnchor(0, 0, 0, 0, 0, 6, 11, 7);
            //把图片插到相应的位置
            patriarch.CreatePicture(anchor, pictureIdx);
            picFs.Close();
            */
        }
        internal static void SaveImageToExcel(BitmapSource[] bmp, string templatefile)
        {

            IWorkbook xssfWorkbookDown;
            //读取模板
            using (FileStream file = new FileStream(templatefile, FileMode.Open, FileAccess.Read))
            {
                xssfWorkbookDown = new XSSFWorkbook(file);
                file.Close();
                file.Dispose();
            }
            //获取模板格式
            ISheet sheetFormat = xssfWorkbookDown.GetSheetAt(1);
            ISheet sheet0 = xssfWorkbookDown.GetSheetAt(0);

            {

                for (int i = 0; i < bmp.Length; i++)
                {
                    int pictureIdx = xssfWorkbookDown.AddPicture(getJPGFromImage(bmp[i]), PictureType.PNG);
                    IDrawing patriarch = sheet0.CreateDrawingPatriarch();
                    // 插图片的位置  XSSFClientAnchor（dx1,dy1,dx2,dy2,col1,row1,col2,row2)
                    IClientAnchor anchor = new XSSFClientAnchor(0, 0, 5, 2, 3, 2, 5, 13);
                    patriarch.CreatePicture(anchor, pictureIdx);
                    string filename = "D:\\t\\TestExcel\\" + DateTime.Now.ToString("yyyyMMddHHmmss" + i.ToString()) + ".xlsx";
                    try
                    {

                        FileStream file = new FileStream(filename, FileMode.Create);
                        xssfWorkbookDown.Write(file);
                        file.Close();
                        file.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

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
