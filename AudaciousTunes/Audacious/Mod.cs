using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudaciousTunes
{
    /// <summary>
    /// Encapsulates the modification of existing sample data.
    /// </summary>
    public static class Mod
    {
        /// <summary>
        /// Mixes two sets of sample data.
        /// </summary>
        /// <param name="samples1">Existing sample data.</param>
        /// <param name="samples2">Existing sample data.</param>
        /// <param name="bias">
        /// A value from 0 to 1 describing the percent mix. 0 = the first
        /// sample set, 1 = the second, and everything between mixes the data
        /// proportionately.
        /// </param>
        public static List<float> Mix(List<float> samples1, List<float> samples2,
            float bias)
        {
            //Creates objects to avoid modifying the originals.
            List<float> newSamples1 = new List<float>(samples1);
            List<float> newSamples2 = new List<float>(samples2);
            
            //Expands the size of each sample list until they are equal.
            while (newSamples1.Count < newSamples2.Count)
            {
                newSamples1.Add(0);
            }
            while (newSamples2.Count < newSamples1.Count)
            {
                newSamples2.Add(0);
            }

            //Applies the bias as a volume change before mixing.
            if (bias <= 0.5)
            {
                newSamples2 = Volume(newSamples2, bias * 2);
            }
            else if (bias <= 1)
            {
                newSamples1 = Volume(newSamples1, 1 - ((bias - 0.5f) * 2));
            }

            //Iterates through all sampleNum, mixing if possible.
            for (int i = 0; i < newSamples1.Count; i++)
            {
                //Adds two sampleNum if possible and clamps if needed.
                if (i < newSamples2.Count)
                {
                    newSamples1[i] = Utils.Clamp
                        ((double)newSamples1[i] + (double)newSamples2[i]);
                }
            }

            //Returns the modified sample list.
            return newSamples1;
        }

        /// <summary>
        /// Multiplies sampleNum by the given volume.
        /// </summary>
        /// <param name="sampleNum">Existing sample data.</param>
        /// <param name="vol">The volume to multiply by.</param>
        public static List<float> Volume(List<float> samples, float vol)
        {
            List<float> result = new List<float>(samples);

            for (int i = 0; i < result.Count; i++)
            {
                result[i] *= vol;
            }

            return result;
        }
    }
}
