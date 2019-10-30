using System;
using System.Collections.Generic;
using System.Text;

namespace Neudio.Core
{
    public class WaveData
    {
        public double[] DataArray { get; }
        public long Length { get; }
        public long Position { get; private set; } = 0;

        public WaveData(long length)
        {
            Length = length;
            DataArray = new double[Length];
        }

        public void Append(double[] datas)
        {
            var appendSize = Math.Min(DataArray.Length - Position - 1, datas.Length);
            for(long i = 0;i < appendSize;i++)
            {
                DataArray[Position + i] = datas[i];
            }

            Position += appendSize;
        }
    }
}
