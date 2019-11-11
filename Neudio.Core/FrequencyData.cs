using System;
using System.Collections.Generic;
using System.Text;
using Accord.Math;
using System.Threading.Tasks;

namespace Neudio.Core
{
    public class FrequencyData
    {
        public const int BlockSize = 2048;

        public double[] Spectrum { get; } = new double[BlockSize];
        public double[] PowerSpectrum { get; } = new double[BlockSize];
        private double[] Samples { get; } = new double[BlockSize];
        private bool HasConvertApplied { get; set; } = true;

        public FrequencyData(double[] samples, int blockIndex)
        {
            var offset = blockIndex * BlockSize;
            Parallel.For(0, BlockSize, i =>
            {
                Spectrum[i] = samples[offset + i];
            });

            CosineTransform.DCT(Spectrum);
            CalculatePowerSpectrum();
        }

        public FrequencyData(short[] samples, int blockIndex)
        {
            var offset = blockIndex * BlockSize;
            Parallel.For(0, BlockSize, i =>
            {
                Spectrum[i] = samples[offset + i];
            });

            CosineTransform.DCT(Spectrum);
            CalculatePowerSpectrum();
        }

        public FrequencyData(WaveData waveData, int blockIndex)
        {
            var offset = blockIndex * BlockSize;
            Parallel.For(0, BlockSize, i =>
            {
                Spectrum[i] = waveData.Samples[offset + i];
            });

            CosineTransform.DCT(Spectrum);
            CalculatePowerSpectrum();
        }

        private void CalculatePowerSpectrum()
        {
            for(var i = 0;i < BlockSize;i++)
            {
                PowerSpectrum[i] = Math.Log10(Math.Abs(Spectrum[i]));
            }
        }

        public double this[int i]
        {
            get 
            {
                return Spectrum[i];
            }
            set 
            {
                Spectrum[i] = value;
                HasConvertApplied = false;
            }
        }

        public void ApplyConvert()
        {
            Spectrum.CopyTo(Samples);
            CosineTransform.IDCT(Samples);
            HasConvertApplied = true;
        }

        public void WriteSamples(double[] samples, int blockIndex)
        {
            if (!HasConvertApplied)
                ApplyConvert();

            var offset = blockIndex * BlockSize;
            Parallel.For(0, BlockSize, i =>
            {
                samples[offset + i] = Samples[i];
            });
        }

        public void WriteSamples(WaveData waveData, int blockIndex)
        {
            if (!HasConvertApplied)
                ApplyConvert();

            var offset = blockIndex * BlockSize;
            Parallel.For(0, BlockSize, i =>
            {
                waveData.Samples[offset + i] = (short)Samples[i];
            });
            Buffer.BlockCopy(waveData.Samples, 0, waveData.ByteArray, 0, waveData.ByteArray.Length);
        }

        public static FrequencyData[] CreateFrequencyDatas(WaveData waveData)
        {
            var count = waveData.SampleLength / BlockSize;
            var frequencyDatas = new FrequencyData[count];
            Parallel.For(0, count, i =>
            {
                frequencyDatas[i] = new FrequencyData(waveData, i);
            });

            return frequencyDatas;
        }

        public static void WriteFrequencyDatas(FrequencyData[] frequencyDatas, WaveData waveData)
        {
            var count = frequencyDatas.Length;
            Parallel.For(0, count, i =>
            {
                frequencyDatas[i].WriteSamples(waveData, i);
            });
        }
    }
}
