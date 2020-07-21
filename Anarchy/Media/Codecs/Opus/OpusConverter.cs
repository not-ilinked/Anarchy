using System;

namespace Discord.Voice
{
    internal class OpusConverter
    {
        protected IntPtr _ptr;

        public const int SamplingRate = 48000;
        public const int Channels = 2;
        public const int TimeBetweenFrames = 20;

        public const int SampleBytes = sizeof(short) * Channels;

        public const int FrameSamplesPerChannel = SamplingRate / 1000 * TimeBetweenFrames;
        public const int FrameSamples = FrameSamplesPerChannel * Channels;
        public const int FrameBytes = FrameSamplesPerChannel * SampleBytes;

        protected void CheckError(OpusError error)
        {
            if (error != OpusError.OK)
                throw new OpusException(error);
        }
    }
}
