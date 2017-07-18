using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudaciousTunes
{
    /// <summary>
    /// A collection of utilities including conversion, analysis, clamping, and
    /// interpolation.
    /// </summary>
    public static class Utils
    {
        public enum ConvertType { Time, Samples, Frequency };

        /// <summary>
        /// Clamps a double to the value range of a float.
        /// </summary>
        public static float Clamp(double value)
        {
            if (value > float.MaxValue)
            {
                value = float.MaxValue;
            }
            else if (value < float.MinValue)
            {
                value = float.MinValue;
            }

            return (float)value;
        }

        /// <summary>
        /// Returns the ms equivalent to the given sampleNum of sampleNum.
        /// </summary>
        /// <param name="sampleNum">Existing sample data.</param>
        /// <param name="channels">The sampleNum of channels involved.</param>
        public static float TimeBySamples(int sampleNum,
            int sampRate,
            int channels)
        {
            //Returns sampleNum of sampleNum divided by the sample rate * channels.
            return sampleNum / ((sampRate / 1000f) * channels);
        }
    }
}
