using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudaciousTunes
{
    /// <summary>
    /// Encompasses the generation of new sample data.
    /// </summary>
    public static class Gen
    {
        public enum Signal { Cosine, Sawtooth, Sine, Square, Triangle };

        /// <summary>
        /// Generates samples of white noise.
        /// </summary>
        /// <param name="ms">The length of the noise in milliseconds.</param>
        public static List<float> Noise(long sampleNum)
        {
            Random rng = new Random();            
            List<float> result = new List<float>();
            
            //Iterates through the samples to add data.
            for (int i = 0; i < sampleNum; i++)
            {
                //Converts the result to the float range.
                result.Add
                    (((float)(rng.NextDouble() - 0.5) * float.MaxValue));
            }

            return result;
        }

        /// <summary>
        /// Generates samples of silence copied to both channels.
        /// </summary>
        /// <param name="sampleNum">The number of samples of silence.</param>
        public static List<float> Silence(int sampleNum)
        {
            //Creates a variable to hold the result.
            List<float> result = new List<float>();

            //Iterates through the samples to add data.
            for (int i = 0; i < sampleNum; i++)
            {
                result.Add(0);
            }

            //Returns the new samples.
            return result;
        }

        /// <summary>
        /// Generates a pure tone with a given waveform and frequency in hertz
        /// for a duration specified in milliseconds and returns the samples.
        /// </summary>
        /// <param name="ms">The time of the signal in milliseconds.</param>
        /// <param name="hz">The positive frequency in hertz.</param>
        public static List<float> Tone(int ms, double hz)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
