
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfInteractiveDataDisplayTest1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModle mv;
        public MainWindow()
        {
            InitializeComponent();
            //Initial();
            //InitialMark();
            //mv = new MainViewModle();
            mv=(MainViewModle)this.DataContext;


            //LayoutRoot.Children.Add(mv.lg);
            //Te.Children.Add(mv.lg);
            //for (int i = 0; i < mv.chart.Length; i++)
            //{
            //    if (i < mv.chart.Length - 4)
            //    {
            //        wp.Children.Add(mv.chart[i]);
            //    }
            //    else
            //    {
            //        gr.Children.Add(mv.chart[i]);
            //        Grid.SetRow(mv.chart[i], 2 + ((i  + 4- mv.chart.Length) / 2)*2);
            //        Grid.SetColumn(mv.chart[i], 2 + (i  + 4- mv.chart.Length) % 2);
            //    }
            //}
            //Te.Children.Add(mv.cmg);
            for (int i = 0; i < mv.PlotModels.Count; i++)
            {
                PlotView pv = new PlotView
                {
                    Width = 300,
                    Height = 300,
                    BorderThickness = new Thickness(5),
                    Model = mv.PlotModels[i],
                };
                wp.Children.Add(pv);
            }
            //AxisTe.Range = new Range(1, 20);
            //AxisTe.Range = 3;
        }
        private void Initial()
        {
            double[] x = new double[2000];
            for (int i = 0; i < x.Length; i++)
                //x[i] = 3.1415 * i / (x.Length - 1);
                x[i] = 0.1 * i;

            //for (int i = 0; i < 3; i++)
            //{
            //    var lg = new LineGraph();
            //    //lines.Children.Add(lg);
            //    lg.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, (byte)(i * 10), 0));
            //    lg.Description = String.Format("Data series {0}", i + 1);
            //    lg.StrokeThickness = 2;
            //    lg.Plot(x, x.Select(v => 3 * i + Math.Sin(v)).ToArray());
            //    //var cmg = new CircleMarkerGraph();
            //    //cmg.PlotColorSize(100, 5, 0.8, 18);
            //    //lines.Children.Add(cmg);

            //}
        }
        private void InitialMark()
        {
            //CircleMarkerGraph cmg = new CircleMarkerGraph();
            //const int N = 2;
            //double[] x = new double[N] { 30, 40 };
            //double[] y = new double[N] { 5, 5 };
            //double[] c = new double[N] { 0.8, 0.8 };
            //double[] d = new double[N] { 18,18 };
            //cmg.PlotColorSize(x, y, c, d);
            //lines.Children.Add(cmg);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string s = "";
            //s += "Original X:" + plo.PlotOriginX.ToString();
            //s += "   Original Y:" + plo.PlotOriginY.ToString();
            //s += "   Width:" + plo.PlotWidth.ToString();
            //s += "   Height:" + plo.PlotHeight.ToString();
            //tbDisplay.Text = s;
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            
            //plo.PlotOriginX=20;
            //plo.PlotOriginY = 30;
            //plo.PlotWidth = 130;
            //plo.PlotHeight = 80;
            
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
