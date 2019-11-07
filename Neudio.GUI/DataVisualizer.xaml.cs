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
    public partial class DataVisualizer : UserControl
    {
        private readonly OxyPlot.Series.LineSeries Series = new OxyPlot.Series.LineSeries()
        {
            StrokeThickness = 0.5
        };

        public DataVisualizer()
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

            GraphView.Model = model;
        }

        public void SetDataRange(int length, double min, double max)
        {
            Series.Points.Clear();
            for(var i = 0;i < length;i++)
            {
                var point = new OxyPlot.DataPoint((max - min) * i / length + min, 0);
                Series.Points.Add(point);
            }
        }

        public void SetData(double[] datas)
        {
            var count = Series.Points.Count;
            for(var i = 0;i < count;i++)
            {
                Series.Points[i] = new OxyPlot.DataPoint(Series.Points[i].X, datas[i]);
            }

            GraphView.InvalidatePlot();
        }

        public void SetData(short[] datas)
        {
            var count = Series.Points.Count;
            for (var i = 0; i < count; i++)
            {
                Series.Points[i] = new OxyPlot.DataPoint(Series.Points[i].X, datas[i]);
            }
        }
    }

}
