using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudaciousTunes;
using System.Media;
using System.Threading;
using System.Diagnostics; //TODO: temporary.

namespace TestingProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Wav finalWav = new Wav();
            //finalWav += Gen.PureSine(1000, 80);
#if _debug
            Console.WriteLine("--Done---");
            Console.Read();
#endif
            //Creates a wav from the result.
            finalWav.Serialize("wave1", 2, 16, 44100);
#if !_debug
            //Creates a soundplayer to play the sound.
            SoundPlayer snd = new SoundPlayer(finalWav.Serialize(2, 16, 44100));
            snd.Play();
            
            //Automatically closes the console after the sound finishes.
            float ms = Utils.TimeBySamples(finalWav.Length(), 44100, 2);
            Thread.Sleep((int)ms + 50); //Time to register playing the sound.
#endif
        }
    }
}