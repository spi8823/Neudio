using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Neudio.Core
{
    public class WaveData : IWaveProvider
    {
        public static readonly NAudio.Wave.WaveFormat Format = new NAudio.Wave.WaveFormat((int)Math.Pow(2, 15), 16, 1);
        public static readonly int BytesPerSample = Format.BitsPerSample / 8;

        public short[] Samples { get; }
        public byte[] ByteArray { get; }
        public int WriteBytePosition { get; private set; } = 0;
        public int ReadBytePosition { get; private set; } = 0;
        public int SampleLength { get; private set; }

        public WaveFormat WaveFormat => Format;

        public WaveData(double seconds)
        {
            var length = Format.ConvertLatencyToByteSize((int)(seconds * 1000));
            Samples = new short[length / BytesPerSample];
            ByteArray = new byte[length];
        }

        public void AppendBytes(byte[] datas, int size)
        {
            var appendSampleSize = (int)Math.Min(ByteArray.Length - WriteBytePosition - 1, size) / BytesPerSample;
            if (appendSampleSize <= 0)
                return;

            Parallel.For(0, appendSampleSize, i =>
            {
                Samples[WriteBytePosition / BytesPerSample + i] = BitConverter.ToInt16(datas, i * BytesPerSample);
                for (var j = 0; j < BytesPerSample; j++)
                {
                    var index = i * BytesPerSample + j;
                    ByteArray[WriteBytePosition + index] = datas[index];
                }
            });

            WriteBytePosition += appendSampleSize * BytesPerSample;
            SampleLength = WriteBytePosition / BytesPerSample;
        }

        public void AppendSamples(short[] samples, int size)
        {
            var appendSampleSize = Math.Min(ByteArray.Length - WriteBytePosition - 1, size * BytesPerSample) / BytesPerSample;
            if (appendSampleSize <= 0)
                return;

            Buffer.BlockCopy(samples, 0, ByteArray, WriteBytePosition, size * BytesPerSample);
            Buffer.BlockCopy(samples, 0, Samples, WriteBytePosition, size * BytesPerSample);
            WriteBytePosition += appendSampleSize * BytesPerSample;
            SampleLength = WriteBytePosition / BytesPerSample;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var readCount = Math.Min(count - offset, SampleLength * BytesPerSample - ReadBytePosition - 1);
            for (var i = 0; i < readCount; i++)
            {
                buffer[offset + i] = ByteArray[ReadBytePosition + i];
            }

            ReadBytePosition += readCount;
            return readCount;
        }
    }
}
