using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudaciousTunes
{
    /// <summary>
    /// Encompasses importing, exporting, and creating wave files. Waves
    /// created are mono or stereo PCM with 8, 16, 24, or 32-bit depth and
    /// an arbitrary sample rate (default 44100).
    /// </summary>
    public class Wav
    {
        //Fields
        private List<float> _leftChannel; //Holds left channel data.
        private List<float> _rightChannel; //Holds right channel data.

        //Constructors
        /// <summary>
        /// Creates an empty wave.
        /// </summary>
        public Wav()
        {
            _leftChannel = new List<float>(10000);
            _rightChannel = new List<float>(10000);
        }

        /// <summary>
        /// Creates a wave object with samples copied to both channels.
        /// </summary>
        /// <param name="samples">Samples copied to both channels.</param>
        public Wav(List<float> samples)
        {
            _leftChannel = new List<float>(10000);
            _rightChannel = new List<float>(10000);
            Add(samples);
        }

        /// <summary>
        /// Creates a wav object with the given sample data (directly linked).
        /// </summary>
        /// <param name="leftChannel">Samples for the left channel.</param>
        /// <param name="rightChannel">Samples for the right channel.</param>
        public Wav(List<float> leftChannel, List<float> rightChannel) : this()
        {
            _leftChannel = leftChannel;
            _rightChannel = rightChannel;
        }

        /// <summary>
        /// Creates a wav object from the samples of a wav.
        /// </summary>
        /// <param name="wav">An existing wav object.</param>
        public Wav(Wav wav) : this()
        {
            _leftChannel.AddRange(wav._leftChannel);
            _rightChannel.AddRange(wav._rightChannel);
        }

        /// <summary>
        /// Creates the wav object from the samples of a loaded wav file.
        /// </summary>
        /// <param name="path">The path, filename, and file extension.</param>
        public Wav(string path) : this()
        {
            Load(path);
        }

        /// <summary>
        /// Creates the wave object from a wave stored in a memory stream.
        /// </summary>
        /// <param name="ms">The memory stream.</param>
        public Wav(MemoryStream ms) : this()
        {
            Load(ms);
        }

        //Methods
        /// <summary>
        /// Copies samples to both channels.
        /// </summary>
        /// <param name="sampleNum">Existing sample data.</param>
        public void Add(List<float> samples)
        {
            _leftChannel.AddRange(samples);
            _rightChannel.AddRange(samples);
        }

        /// <summary>
        /// Copies sample lists to each channel.
        /// </summary>
        /// <param name="leftChannel">Samples for the left channel.</param>
        /// <param name="rightChannel">Samples for the right channel.</param>
        public void Add(List<float> leftChannel, List<float> rightChannel)
        {
            _leftChannel.AddRange(leftChannel);
            _rightChannel.AddRange(rightChannel);
        }

        /// <summary>
        /// Copies samples from a wave object to both channels.
        /// </summary>
        /// <param name="wav">An existing wave object.</param>
        public void Add(Wav wav)
        {
            _leftChannel.AddRange(wav._leftChannel);
            _rightChannel.AddRange(wav._rightChannel);
        }

        /// <summary>
        /// Copies samples to the left channel.
        /// </summary>
        /// <param name="sampleNum">Existing sample data.</param>
        public void AddLeft(List<float> samples)
        {
            _leftChannel.AddRange(samples);
        }

        /// <summary>
        /// Copies samples to the right channel.
        /// </summary>
        /// <param name="sampleNum">Existing sample data.</param>
        public void AddRight(List<float> samples)
        {
            _rightChannel.AddRange(samples);
        }

        /// <summary>
        /// Clears the data so the channels are empty.
        /// </summary>
        public void Clear()
        {
            _leftChannel.Clear();
            _rightChannel.Clear();
        }

        /// <summary>
        /// Returns a new wave object with only a range of the data in it.
        /// </summary>
        /// <param name="index">The index where the samples begin.</param>
        /// <param name="count">The amount of samples to get.</param>
        public Wav GetRange(int index, int count)
        {
            Wav newWav = new Wav(
                _leftChannel.GetRange(index, count),
                _rightChannel.GetRange(index, count));

            return newWav;
        }

        /// <summary>
        /// Copies samples to both channels at the given index.
        /// </summary>
        /// <param name="sampleNum">Existing sample data.</param>
        /// <param name="index">The position to insert after.</param>
        public void Insert(int index, List<float> samples)
        {
            _leftChannel.InsertRange(index, samples);
            _rightChannel.InsertRange(index, samples);
        }

        /// <summary>
        /// Copies sample lists to each channel at the given index.
        /// </summary>
        /// <param name="leftChannel">Samples for the left channel.</param>
        /// <param name="rightChannel">Samples for the right channel.</param>
        /// <param name="index">The position to insert after.</param>
        public void Insert(
            int index,
            List<float> leftChannel,
            List<float> rightChannel)
        {
            //Inserts sample lists to each channel.
            this._leftChannel.InsertRange(index, leftChannel);
            this._rightChannel.InsertRange(index, rightChannel);
        }

        /// <summary>
        /// Copies samples from the wave object to the given index.
        /// </summary>
        /// <param name="wav">An existing wave object.</param>
        /// <param name="index">The position to insert after.</param>
        public void Insert(Wav wav, int index)
        {
            _leftChannel.InsertRange(index, wav._leftChannel);
            _rightChannel.InsertRange(index, wav._rightChannel);
        }

        /// <summary>
        /// Returns the length of the longest channel.
        /// </summary>
        public int Length()
        {
            if (_leftChannel.Count > _rightChannel.Count)
            {
                return _leftChannel.Count;
            }
            
            return _rightChannel.Count;
        }

        /// <summary>
        /// (Over)writes wave data and sampleNum to a file, saving with the
        /// given settings.
        /// </summary>
        /// <param name="path">
        /// The path and filename.
        /// </param>
        /// <param name="channels">May be 1 (mono) or 2 (stereo).</param>
        /// <param name="bitDepth">May be 8, 16, 24, or 32-bit.</param>
        /// <param name="sampRate">
        /// Samples per second; usually factors or multiples of 44100.
        /// </param>
        public void Serialize(string path,
            int channels,
            int bitDepth,
            int sampRate)
        {
            //Creates a new filestream instance.
            FileStream fs;

            if (Path.GetExtension(path) == ".wav")
            {
                fs = File.Create(path);
            }
            else
            {
                fs = File.Create(path + ".wav");
            }

            //Checks argument validity.
            if (channels < 1 || channels > 2)
            {
                throw new ArgumentException("Only mono and stereo channels " +
                    "are supported.");
            }
            if (bitDepth != 8 && bitDepth != 16 &&
                bitDepth != 24 && bitDepth != 32)
            {
                throw new ArgumentException("Only 8, 16, 24, and 32-bit " +
                    "audio is supported.");
            }
            if (sampRate < 0)
            {
                throw new ArgumentException("The sample rate must be " +
                    "positive.");
            }

            //Calculates variable info.
            int sampleLength = Length();
            uint dataChunkSize = (uint)sampleLength * ((uint)bitDepth / 8);
            uint headerSize = 36 + dataChunkSize;
            ushort blockAlign = (ushort)(channels * (bitDepth / 8));

            //Writes the RIFF header: chunk id, header size, and format.
            fs.Write(ASCIIEncoding.ASCII.GetBytes("RIFF"), 0, 4);
            fs.Write(BitConverter.GetBytes(headerSize), 0, 4);
            fs.Write(ASCIIEncoding.ASCII.GetBytes("WAVE"), 0, 4);

            /* Writes the format block with the following info, in order:
             chunk id, header size, audio format (PCM), sampleNum of channels,
             sample rate, byte rate, block alignment, and bits per sample. */
            fs.Write(ASCIIEncoding.ASCII.GetBytes("fmt "), 0, 4);
            fs.Write(BitConverter.GetBytes(16u), 0, 4);
            fs.Write(BitConverter.GetBytes((ushort)1), 0, 2);
            fs.Write(BitConverter.GetBytes((ushort)channels), 0, 2);
            fs.Write(BitConverter.GetBytes((uint)sampRate), 0, 4);
            fs.Write(BitConverter.GetBytes((uint)sampRate * blockAlign), 0, 4);
            fs.Write(BitConverter.GetBytes(blockAlign), 0, 2);
            fs.Write(BitConverter.GetBytes((ushort)bitDepth), 0, 2);

            //Writes the data block header and size information.
            fs.Write(ASCIIEncoding.ASCII.GetBytes("data"), 0, 4);
            fs.Write(BitConverter.GetBytes(dataChunkSize), 0, 4);

            //Pads each channel with silence until they have equal size.
            for (int i = 0; i < sampleLength; i++)
            {
                if (i >= _leftChannel.Count)
                {
                    _leftChannel.Add(0);
                }
                if (i >= _rightChannel.Count)
                {
                    _rightChannel.Add(0);
                }

                //todo: Change the range of data before it's casted to another
                //type so that you get the proportionate range (w/o errors).
                if (bitDepth == 8)
                {
                    fs.Write(new byte[] {(byte)_leftChannel[i]}, 0, 1);
                    if (channels == 2)
                    {
                        fs.Write(new byte[] {(byte)_rightChannel[i]}, 0, 1);
                    }
                }
                else if (bitDepth == 16)
                {
                    fs.Write(BitConverter.GetBytes((short)_leftChannel[i]), 0, 2);
                    if (channels == 2)
                    {
                        fs.Write(BitConverter.GetBytes((short)_rightChannel[i]), 0, 2);
                    }
                }
                else if (bitDepth == 24)
                {
                    //todo: Save floats as ints and remove the last bit.
                    throw new NotImplementedException();
                }
                else if (bitDepth == 32)
                {
                    fs.Write(BitConverter.GetBytes(_leftChannel[i]), 0, 4);
                    if (channels == 2)
                    {
                        fs.Write(BitConverter.GetBytes(_rightChannel[i]), 0, 4);
                    }
                }
            }

            //Ensures the data has been written and closes the FileStream.
            fs.Flush();
            fs.Dispose();
        }

        /// <summary>
        /// (Over)writes wave data and sampleNum to a memory stream, saving with
        /// the given settings.
        /// </summary>
        /// <param name="channels">May be 1 (mono) or 2 (stereo).</param>
        /// <param name="bitDepth">May be 8, 16, 24, or 32-bit.</param>
        /// <param name="sampRate">
        /// Samples per second; usually factors or multiples of 44100.
        /// </param>
        public MemoryStream Serialize(int channels,
            int bitDepth,
            int sampRate)
        {
            //Creates a new memory stream instance.            
            MemoryStream ms = new MemoryStream();

            //Checks argument validity.
            if (channels < 1 || channels > 2)
            {
                throw new ArgumentException("Only mono and stereo channels " +
                    "are supported.");
            }
            if (bitDepth != 8 && bitDepth != 16 &&
                bitDepth != 24 && bitDepth != 32)
            {
                throw new ArgumentException("Only 8, 16, 24, and 32-bit " +
                    "audio is supported.");
            }
            if (sampRate < 0)
            {
                throw new ArgumentException("The sample rate must be " +
                    "positive.");
            }

            //Calculates variable info.
            int sampleLength = Length();
            uint dataChunkSize = (uint)sampleLength * ((uint)bitDepth / 8);
            uint headerSize = 36 + dataChunkSize;
            ushort blockAlign = (ushort)(channels * (bitDepth / 8));

            //Writes the RIFF header: chunk id, header size, and format.
            ms.Write(ASCIIEncoding.ASCII.GetBytes("RIFF"), 0, 4);
            ms.Write(BitConverter.GetBytes(headerSize), 0, 4);
            ms.Write(ASCIIEncoding.ASCII.GetBytes("WAVE"), 0, 4);

            /* Writes the format block with the following info, in order:
             chunk id, header size, audio format (PCM), sampleNum of channels,
             sample rate, byte rate, block alignment, and bits per sample. */
            ms.Write(ASCIIEncoding.ASCII.GetBytes("fmt "), 0, 4);
            ms.Write(BitConverter.GetBytes(16u), 0, 4);
            ms.Write(BitConverter.GetBytes((ushort)1), 0, 2);
            ms.Write(BitConverter.GetBytes((ushort)channels), 0, 2);
            ms.Write(BitConverter.GetBytes((uint)sampRate), 0, 4);
            ms.Write(BitConverter.GetBytes((uint)sampRate * blockAlign), 0, 4);
            ms.Write(BitConverter.GetBytes(blockAlign), 0, 2);
            ms.Write(BitConverter.GetBytes((ushort)bitDepth), 0, 2);

            //Writes the data block header and size information.
            ms.Write(ASCIIEncoding.ASCII.GetBytes("data"), 0, 4);
            ms.Write(BitConverter.GetBytes(dataChunkSize), 0, 4);

            //Pads each channel with silence until they have equal size.
            for (int i = 0; i < sampleLength; i++)
            {
                //Appends silence if the index is out of bounds.
                if (i >= _leftChannel.Count)
                {
                    _leftChannel.Add(0);
                }
                if (i >= _rightChannel.Count)
                {
                    _rightChannel.Add(0);
                }

                //todo: Change the range of data before it's casted to another
                //type so that you get the proportionate range (w/o errors).
                if (bitDepth == 8)
                {
                    ms.Write(BitConverter.GetBytes((byte)_leftChannel[i]), 0, 2);
                    if (channels == 2)
                    {
                        ms.Write(
                            BitConverter.GetBytes((byte)_rightChannel[i]), 0, 2);
                    }
                }
                else if (bitDepth == 16)
                {
                    ms.Write(BitConverter.GetBytes((short)_leftChannel[i]), 0, 2);
                    if (channels == 2)
                    {
                        ms.Write(
                            BitConverter.GetBytes((short)_rightChannel[i]), 0, 2);
                    }
                }
                else if (bitDepth == 24)
                {
                    //todo: Save floats as ints and remove the last bit.
                    throw new NotImplementedException();
                }
                else if (bitDepth == 32)
                {
                    ms.Write(BitConverter.GetBytes(_leftChannel[i]), 0, 2);
                    if (channels == 2)
                    {
                        ms.Write(BitConverter.GetBytes(_rightChannel[i]), 0, 2);
                    }
                }
            }

            //Ensures the data has been written and returns the MemoryStream.
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        /// <summary>
        /// Overwrites the wave object from the sampleNum of a loaded wave file.
        /// Appends data to the original.
        /// </summary>
        /// <param name="path">The path and filename.</param>
        private void Load(string path)
        {
            //Reads all of the data into a byte array.
            byte[] fileData;

            if (Path.GetExtension(path) == ".wav")
            {
                fileData = File.ReadAllBytes(path);
            }
            else
            {
                fileData = File.ReadAllBytes(path + ".wav");
            }

            //Loads the data from a constructed memory stream.
            Load(new MemoryStream(fileData, false));
        }

        /// <summary>
        /// Overwrites the wave object from the sampleNum stored in a memory
        /// stream. Appends data to the original.
        /// </summary>
        /// <param name="ms">The existing memory stream with wave data.</param>
        private void Load(MemoryStream ms)
        {
            //Loads the data into a stream for a binary reader.
            BinaryReader br = new BinaryReader(ms, System.Text.Encoding.ASCII);

            //Attempts to read the RIFF header of the first chunk.
            string temp_chunkID = System.Text.Encoding.ASCII.GetString
                (br.ReadBytes(4));

            if (temp_chunkID != "RIFF")
            {
                throw new Exception("Wav load error: RIFF header expected. " +
                "Got " + temp_chunkID + " instead.");
            }

            //Attempts to read the size of the first chunk.
            uint temp_chunkSize = br.ReadUInt32();
            if (temp_chunkSize != br.BaseStream.Length - 8)
            {
                throw new Exception("Wav load error: RIFF size does not " +
                "match expected file size.");
            }

            //Attempts to read the format from the first chunk.
            string temp_format = System.Text.Encoding.ASCII.GetString
                (br.ReadBytes(4));
            if (temp_format != "WAVE")
            {
                throw new Exception("Wav load error: Block must be 'WAVE'." +
                " Got " + temp_format + " instead.");
            }
            //Attempts to begin organizing chunk locations.
            //Creates variables to store the names and sizes of all chunks.
            List<string> temp_chunksName = new List<string>();
            List<uint> temp_chunksSize = new List<uint>();
            List<long> temp_chunksPos = new List<long>();

            //Reads to the end of the data to find each block.            
            //Defines variables here to remain in scope.
            ushort temp_audioFormat = 0;
            ushort temp_numChannels = 0;
            ushort temp_bitsPerSample = 0;
            uint temp_sampRate = 0;

            while (br.BaseStream.Length - br.BaseStream.Position > 4)
            {
                //Attempts to read the chunk's name.
                temp_chunksName.Add(System.Text.Encoding.ASCII
                    .GetString(br.ReadBytes(4)));

                //Attempts to read the chunk's size.
                temp_chunksSize.Add(br.ReadUInt32());

                //Stores the chunk's position.
                temp_chunksPos.Add(br.BaseStream.Position);

                //Skips the rest of the known chunk.
                if (br.BaseStream.Position + temp_chunksSize.Last() > br.BaseStream.Length)
                {
                    throw new Exception("A chunk size doesn't match.");
                }

                br.BaseStream.Position += temp_chunksSize.Last();
            }

            //Operates on the 'fmt ' chunk.
            for (int i = 0; i < temp_chunksName.Count; i++)
            {
                //If the chunk exists.
                if (temp_chunksName[i] == "fmt ")
                {
                    //Seeks to the beginning of the chunk after the ID and
                    //Size segments.
                    br.BaseStream.Seek(temp_chunksPos[i], SeekOrigin.Begin);

                    //Tests for supported audio compression formats.
                    temp_audioFormat = br.ReadUInt16();
                    if (temp_audioFormat != 1)
                    {
                        throw new Exception("The wav file must be " +
                            "uncompressed (linear quantization). Got " +
                        "code " + temp_audioFormat + " instead.");
                    }

                    //Tests for supported channel formats.
                    temp_numChannels = br.ReadUInt16();
                    if (temp_numChannels != 1 && temp_numChannels != 2)
                    {
                        throw new Exception("The wav file must have " +
                            "mono or stereo channels only. Channels: " +
                        temp_numChannels + ".");
                    }

                    //Tests for possible sample rate.
                    temp_sampRate = br.ReadUInt32();
                    if (temp_sampRate < 1)
                    {
                        throw new Exception("Sample rate must be greater " +
                        "than zero. Got " + temp_sampRate + " instead.");
                    }

                    br.ReadUInt32(); //Reads past Byte Rate
                    br.ReadUInt16(); //Reads past Block Align
                    
                    //Tests for supported bitPerSample formats.
                    temp_bitsPerSample = br.ReadUInt16();
                    if (temp_bitsPerSample != 8 &&
                        temp_bitsPerSample != 16 &&
                        temp_bitsPerSample != 24 &&
                        temp_bitsPerSample != 32)
                    {
                        throw new Exception("The wav file must have a " +
                        "bit depth of 8, 16, 24, or 32. Bit depth: " +
                        temp_bitsPerSample + ".");
                    }
                }

                //If the chunk doesn't exist.
                else if (i == temp_chunksName.Count)
                {
                    string mssg = "'fmt ' chunk not found. Chunks: ";
                    foreach (string chunk in temp_chunksName)
                    {
                        mssg += chunk;
                    }

                    throw new Exception(mssg);
                }
            }

            //Operates on the 'data' chunk.
            for (int i = 0; i < temp_chunksName.Count; i++)
            {
                //If the chunk exists.
                if (temp_chunksName[i] == "data")
                {
                    br.BaseStream.Seek(temp_chunksPos[i], SeekOrigin.Begin);
                    if (br.BaseStream.Position + temp_chunksSize[i] >
                        br.BaseStream.Length)
                    {
                        throw new Exception("File size invalid: 'data'.");
                    }
                    //Reads all of the data into a byte list.
                    List<byte> rawData = new List<byte>();
                    rawData.AddRange(br.ReadBytes((int)temp_chunksSize[i]));

                    //Constructs a wave from the given data.
                    Add(Construct(rawData,
                        temp_numChannels,
                        temp_bitsPerSample,
                        (int)temp_sampRate));
                }

                //If the chunk doesn't exist.
                else if (i == temp_chunksName.Count - 1)
                {
                    string mssg = "'data' chunk not found. Chunks: ";
                    foreach (string chunk in temp_chunksName)
                    {
                        mssg += chunk;
                    }

                    throw new Exception(mssg);
                }
            }

            //Ensures the binary reader is closed.
            br.Dispose();
        }

        /// <summary>
        /// Returns a new wave with the sample data from the second appended.
        /// </summary>
        /// <param name="wav1">An existing wave object.</param>
        /// <param name="wav2">An existing wave object.</param>
        public static Wav operator +(Wav wav1, Wav wav2)
        {
            Wav result = new Wav(wav1);
            result.Add(wav2);
            return result;
        }

        /// <summary>
        /// Returns a new wave with the given sample data appended.
        /// </summary>
        /// <param name="wav">An existing wave object.</param>
        /// <param name="samples">Existing sample data.</param>
        public static Wav operator +(Wav wav, List<float> samples)
        {
            Wav result = new Wav(wav);
            result.Add(samples);
            return result;
        }

        /// <summary>
        /// Converts samples into a wave object.
        /// </summary>
        /// <param name="samples">Existing sample data.</param>
        public static explicit operator Wav(List<float> samples)
        {
            return new Wav(samples);
        }

        /// <summary>
        /// Constructs a wave object from the byte data.
        /// </summary>
        /// <param name="samples">The raw byte data.</param>
        /// <param name="channels">May be 1 (mono) or 2 (stereo).</param>
        /// <param name="bitDepth">May be 8, 16, 24, or 32-bit.</param>
        /// <param name="sampRate">
        /// Samples per second; usually factors or multiples of 44100.
        /// </param>
        private static Wav Construct(List<byte> samples,
            int channels,
            int bitDepth,
            int sampRate)
        {
            Wav wav = new Wav();

            //Checks argument validity.
            if (channels < 1 || channels > 2)
            {
                throw new ArgumentException("Only mono and stereo channels " +
                    "are supported.");
            }
            if (bitDepth != 8 && bitDepth != 16 &&
                bitDepth != 24 && bitDepth != 32)
            {
                throw new ArgumentException("Only 8, 16, 24, and 32-bit " +
                    "audio is supported.");
            }
            if (sampRate < 0)
            {
                throw new ArgumentException("The sample rate must be " +
                    "positive.");
            }

            //Holds sampleNum throughout construction.
            List<float> sampleData = new List<float>();

            //If the sample alignment is off, this fixes it.
            while (samples.Count % (bitDepth / 8) != 0)
            {
                samples.Add(0);
            }

            //Converts the raw data into sampleNum in groups based on bit depth.
            if (bitDepth == 8)
            {
                for (int i = 0; i < samples.Count; i++)
                {
                    sampleData.Add((float)samples[i]);
                }
            }
            else if (bitDepth == 16)
            {
                for (int i = 0; i < samples.Count; i += 2)
                {
                    sampleData.Add((float)BitConverter.ToInt16
                        (samples.GetRange(i, 2).ToArray(), 0));
                }
            }
            else if (bitDepth == 24)
            {
                for (int i = 0; i < samples.Count; i += 3)
                {
                    List<byte> byteArray = samples.GetRange(i, 3);
                    byteArray.Add(0); //Promotes to 4 bytes for ease.
                    sampleData.Add((float)BitConverter.ToSingle
                        (samples.GetRange(i, 2).ToArray(), 0));
                }
            }
            else //bit depth of 32.
            {
                for (int i = 0; i < samples.Count; i += 4)
                {
                    sampleData.Add(BitConverter.ToSingle
                        (samples.GetRange(i, 4).ToArray(), 0));
                }
            }

            //De-interleaves the data into the wave.
            if (channels == 1)
            {
                for (int i = 0; i < sampleData.Count; i++)
                {
                    if (channels == 1)
                    {
                        wav._leftChannel.Add(sampleData[i]);
                        wav._rightChannel.Add(sampleData[i]);
                    }
                    else
                    {
                        //If the sample sampleNum is even, adds it to the left.
                        //If the sample sampleNum is odd, adds it to the right.
                        if (i % 2 == 0)
                        {
                            wav._leftChannel.Add(sampleData.Last());
                        }
                        else
                        {
                            wav._rightChannel.Add(sampleData.Last());
                        }
                    }
                }
            }

            return wav;
        }
    }
}