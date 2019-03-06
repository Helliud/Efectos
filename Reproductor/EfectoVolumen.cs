using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Reproductor
{
    class EfectoVolumen : ISampleProvider
    {
        private float volume;
        public float Volume
        {
            get
            {
                return volume;
            }
            set
            {
                if(value < 0)
                {
                    volume = 0;
                }
                else if (value > 1)
                {
                    volume = 1;
                }
                else
                {
                    volume = value;
                }
                volume = value;
            }
        }

        private ISampleProvider fuente;

        public EfectoVolumen(ISampleProvider fuente)
        {
            this.fuente = fuente;
            volume = 1;
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var read = fuente.Read(buffer, offset, count);
            for(int i = 0; i < read; i++)
            {
                buffer[offset + i] *= volume;
            }

            return read;
        }
    }
}
