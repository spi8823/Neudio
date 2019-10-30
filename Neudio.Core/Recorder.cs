using System;

namespace Neudio.Core
{
    public class Recorder
    {
        public RecorderConfig Config { get; }
        public bool IsRecording { get; private set; }

        private NAudio.Wave.WaveInEvent WaveIn { get; set; }
        private NAudio.Wave.WaveFileWriter WaveWriter { get; set; }

        public Recorder(RecorderConfig config)
        {
            Config = config;
        }

        public static NAudio.Wave.WaveInEvent CreateWaveIn(RecorderConfig config, NAudio.Wave.WaveFileWriter writer)
        {
            var waveIn = new NAudio.Wave.WaveInEvent();
            waveIn.DeviceNumber = config.DeviceNumber;
            waveIn.WaveFormat = writer.WaveFormat;

            waveIn.DataAvailable += (_, e) =>
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
                writer.Flush();
            };
            waveIn.RecordingStopped += (_, __) =>
            {
                writer.Flush();
            };

            return waveIn;
        }

        public static NAudio.Wave.WaveFileWriter CreateWaveFileWriter(RecorderConfig config)
        {
            var format = new NAudio.Wave.WaveFormat(config.SamplingRate, config.ChannelCount);
            var writer = new NAudio.Wave.WaveFileWriter(config.FileName, format);
            return writer;
        }

        public void Start()
        {
            if (IsRecording)
                return;

            IsRecording = true;
            WaveWriter = CreateWaveFileWriter(Config);
            WaveIn = CreateWaveIn(Config, WaveWriter);

            WaveIn?.StartRecording();
        }

        public void Stop()
        {
            if (!IsRecording)
                return;

            IsRecording = false;
            WaveIn?.StopRecording();
            WaveIn?.Dispose();
            WaveWriter?.Dispose();
        }
    }

    public class RecorderConfig
    {
        public string FileName { get; set; } = "";
        public int DeviceNumber { get; set; } = 0;
        public int SamplingRate { get; set; } = 44100;
        public int ChannelCount { get; set; } = 1;
    }
}
