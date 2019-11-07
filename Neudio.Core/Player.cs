using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neudio.Core
{
    public class Player
    {
        private NAudio.Wave.WaveOutEvent WaveOut { get; }

        public Player()
        {
            WaveOut = new WaveOutEvent();
        }

        public void Play(IWaveProvider provider)
        {
            WaveOut.Init(provider);
            WaveOut.Play();
        }

        public void Pause()
        {
            WaveOut.Pause();
        }

        public void Stop()
        {
            WaveOut.Stop();
        }
    }
}
