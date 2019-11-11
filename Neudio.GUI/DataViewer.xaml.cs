using System;
using System.Collections.Generic;
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

namespace Neudio.GUI
{
    /// <summary>
    /// DataVisualizer.xaml の相互作用ロジック
    /// </summary>
    public partial class DataViewer : UserControl
    {
        private readonly OxyPlot.Series.LineSeries Series = new OxyPlot.Series.LineSeries()
        {
            StrokeThickness = 0.5
        };

        public DataViewer()
        {
            InitializeComponent();
            InitializeView();
        }

        private void InitializeView()
        {
            var model = new OxyPlot.PlotModel()
            {
                Title = "Test",
            };

            model.Axes.Add(new OxyPlot.Axes.LinearAxis() { Position = OxyPlot.Axes.AxisPosition.Bottom });
            model.Axes.Add(new OxyPlot.Axes.LinearAxis() { Position = OxyPlot.Axes.AxisPosition.Left });

            model.Series.Add(Series);

            Graph.Model = model;
        }

        public void SetDataRange(int length, double minX, double maxX, double minY, double maxY)
        {
            Series.Points.Clear();
            for(var i = 0;i < length;i++)
            {
                var point = new OxyPlot.DataPoint((maxX - minX) * i / length + minX, 0);
                Series.Points.Add(point);
            }

            Series.YAxis.Minimum = minY;
            Series.YAxis.Maximum = maxY;
        }

        public void SetDataRange(int length, Func<int, double> indexToX, (double min, double max) YRange)
        {
            Series.Points.Clear();
            for(var i = 0;i < length;i++)
            {
                var point = new OxyPlot.DataPoint(indexToX(i), 0);
                Series.Points.Add(point);
            }

            Series.YAxis.Minimum = YRange.min;
            Series.YAxis.Maximum = YRange.max;
        }

        public void SetData(double[] datas, int offset, int count)
        {
            for(var i = 0;i < count;i++)
            {
                Series.Points[i] = new OxyPlot.DataPoint(Series.Points[i].X, datas[offset + i]);
            }

            Graph.InvalidatePlot();
        }

        public void SetData(short[] datas, int offset, int count)
        {
            for (var i = 0; i < count; i++)
            {
                Series.Points[i] = new OxyPlot.DataPoint(Series.Points[i].X, datas[offset + i]);
            }

            Graph.InvalidatePlot();
        }
    }

}
