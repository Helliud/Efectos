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
        private ISampleProvider fuente;

        public EfectoVolumen(ISampleProvider fuente)
        {
            this.fuente = fuente;
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
                buffer[offset + i] *= 1.0f;
            }

            return read;
        }
    }
}
