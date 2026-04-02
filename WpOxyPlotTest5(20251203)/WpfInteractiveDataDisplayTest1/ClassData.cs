
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfInteractiveDataDisplayTest1
{
    public class ClassData : NotificationObject
    {
        public ObservableCollection<DataPoint> PointsData { get; set; }
        private ObservableCollection<Point> pointsDataDisplay;
        public ObservableCollection<Point> PointsDataDisplay
        {
            get { return pointsDataDisplay; }
            set
            {
                pointsDataDisplay = value;
                RaisePropertyChanged("PointsDataDisplay");
                //EventHideEdge(dStartXDisplay, dEndXDisplay);
                Filtrator();
            }
        }
        private double dStartXDisplay, dEndXDisplay;
        private double dEdge;
        public ClassData()
        {
            PointsData = new ObservableCollection<DataPoint>();
            //PointsDataDisplay = new ObservableCollection<Point>();
            //PlotBase.EventHideEdge += MainViewModle_EventHideEdge;
            //PointsDataDisplay.CollectionChanged += MainViewModle_CollectionChanged;
        }

        private void MainViewModle_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Filtrator();
        }

        private void MainViewModle_EventHideEdge(double dStart, double dEnd)
        {
            try
            {
                dStartXDisplay = dStart;
                dEndXDisplay = dEnd;
                dEdge = (dEnd - dStart) * 0.1;
                Filtrator();
                //EventHideEdge(dStartXDisplay, dEndXDisplay);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Filtrator()
        {
            //if (PointsData == null) return;
            //PointsData.Clear();
            //for (int i = 0; i < PointsDataDisplay.Count(); i++)
            //{
            //    Point p = (Point)PointsDataDisplay[i];
            //    if ((p.X > dStartXDisplay + dEdge) && (p.X < dEndXDisplay - dEdge))
            //    {
            //        PointsData.Add(p);
            //    }
            //}
        }
    }
}
