using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Core.Recorder Recorder { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Visualizer.SetDataRange(100, 0, 100);
            var datas = new double[100];
            var random = new Random();
            var timer = new System.Timers.Timer(10);
            timer.Elapsed += (a, b) =>
            {
                for (var i = 0; i < 100; i++)
                {
                    datas[i] = random.NextDouble() * 100;
                }
                Visualizer.SetData(datas);
            };
            timer.Start();
        }

        private void StartRecordingButton_Click(object sender, RoutedEventArgs e)
        {
            var config = new Core.RecorderConfig()
            {
                DeviceNumber = 0,
                RecordSeconds = 10,
            };
            Recorder = new Core.Recorder(config);
            Recorder.Start();
        }

        private void StopRecordingButton_Click(object sender, RoutedEventArgs e)
        {
            Recorder.Stop();

            var waveData = Recorder.WaveData;
            var frequencyDatas = Core.FrequencyData.CreateFrequencyDatas(waveData);
            Parallel.ForEach(frequencyDatas, frequencyData => 
            {
            });
            Core.FrequencyData.WriteFrequencyDatas(frequencyDatas, waveData);

            new Core.Player().Play(Recorder.WaveData);
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var waveData = new Core.WaveData(5);
            var f = 440;
            var samples = new short[waveData.Samples.Length];
            for(var i = 0;i < samples.Length;i++)
            {
                samples[i] = (short)(Math.Sin(2 * Math.PI * f * i / (double)Core.WaveData.Format.SampleRate) * 1000);
            }
            waveData.AppendSamples(samples, samples.Length);

            var frequencyDatas = Core.FrequencyData.CreateFrequencyDatas(waveData);
            foreach(var frequencyData in frequencyDatas)
            {
                for(var i = 11;i < Core.FrequencyData.BlockSize;i++)
                {
                    frequencyData[Core.FrequencyData.BlockSize - i + 10] = frequencyData[Core.FrequencyData.BlockSize - i];
                }
                for(var i = 0;i < 10;i++)
                {
                    frequencyData[i] = 0;
                }
            }
            Core.FrequencyData.WriteFrequencyDatas(frequencyDatas, waveData);
        }
    }
}
