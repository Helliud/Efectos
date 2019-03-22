using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Reproductor
{
    class Delay : ISampleProvider
    {

       public float ganancia = 0;

        public bool Activo
        {
            get; set;
        }

        private ISampleProvider fuente;
        private int offsetMilisegundos;
        public int OffsetMilisegundos {
            get { return offsetMilisegundos; }
            set { offsetMilisegundos = value;
                cantidadMuestrasOffset = (int)(((float)OffsetMilisegundos / 1000.0f) * (float)fuente.WaveFormat.SampleRate); 
            }
        }
        private int cantidadMuestrasOffset;

        private List<float> bufferDelay = new List<float>();
        private int tamanoBuffer;
        private int duracionBufferSegundos;
        private int cantidadMuestraTranscurridas = 0;
        private int cantidadMuestrasBorradas = 0;
        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }

        public Delay(ISampleProvider fuente)
        {
            Activo = false;
            this.fuente = fuente;
            OffsetMilisegundos = 500;
            cantidadMuestrasOffset = (int)(((float)OffsetMilisegundos / 1000.0f) * (float)fuente.WaveFormat.SampleRate); ;
            duracionBufferSegundos = 10;
            tamanoBuffer = fuente.WaveFormat.SampleRate * duracionBufferSegundos;
        }

        public int Read(float[] buffer, int offset, int count)
        {

            //Leemos las muestras de la senal fuente
            var read = fuente.Read(buffer, offset, count);
            //Calcular tiempo trancurrido
            float tiempoTranscurridoSegundos = (float)cantidadMuestraTranscurridas / (float)fuente.WaveFormat.SampleRate;
            float milisegundosTranscurridos = tiempoTranscurridoSegundos * 1000.0f;
            //Llenando el buffer
            for (int i = 0; i < read; i++)
            {
                bufferDelay.Add(buffer[i + offset]); 
            }

            //Eliminar excedentes del buffer
            if(bufferDelay.Count > tamanoBuffer)
            {
                int diferencia = bufferDelay.Count - tamanoBuffer;
                bufferDelay.RemoveRange(0, diferencia);
                cantidadMuestrasBorradas += diferencia;
            }

            //Aplicar el efecto
            if (Activo)
            {

                if (milisegundosTranscurridos > OffsetMilisegundos)
                {
                    for (int i = 0; i < read; i++)
                    {
                        buffer[offset + i] += bufferDelay[cantidadMuestraTranscurridas - cantidadMuestrasBorradas + i - cantidadMuestrasOffset] * ganancia;
                    }
                }

            }

            cantidadMuestraTranscurridas += read;
            return read;
        }
    }
}
