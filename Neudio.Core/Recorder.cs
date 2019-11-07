using System;

namespace Neudio.Core
{
    public class Recorder
    {
        public RecorderConfig Config { get; }
        public bool IsRecording { get; private set; }

        public WaveData WaveData { get; set; }
        private NAudio.Wave.WaveInEvent WaveIn { get; set; }

        public Recorder(RecorderConfig config)
        {
            Config = config;
        }

        public static WaveData CreateWaveData(RecorderConfig config)
        {
            var format = new NAudio.Wave.WaveFormat(WaveData.Format.SampleRate, 1);
            var waveData = new WaveData(config.RecordSeconds);
            return waveData;
        }

        public static NAudio.Wave.WaveInEvent CreateWaveIn(RecorderConfig config, WaveData waveData)
        {
            var waveIn = new NAudio.Wave.WaveInEvent();
            waveIn.DeviceNumber = config.DeviceNumber;
            waveIn.WaveFormat = WaveData.Format;

            waveIn.DataAvailable += (_, e) =>
            {
                waveData.AppendBytes(e.Buffer, e.BytesRecorded);
            };

            return waveIn;
        }

        public void Start()
        {
            if (IsRecording)
                return;

            IsRecording = true;
            WaveData = CreateWaveData(Config);
            WaveIn = CreateWaveIn(Config, WaveData);

            WaveIn?.StartRecording();
        }

        public void Stop()
        {
            if (!IsRecording)
                return;

            IsRecording = false;
            WaveIn?.StopRecording();
            WaveIn?.Dispose();
        }

        public void Save(string filename)
        {
            var writer = new NAudio.Wave.WaveFileWriter(filename, WaveData.Format);
            writer.WriteSamples(WaveData.Samples, 0, WaveData.SampleLength);
            writer.Close();
        }
    }

    public class RecorderConfig
    {
        public double RecordSeconds { get; set; }
        public int DeviceNumber { get; set; } = 0;
    }
}
