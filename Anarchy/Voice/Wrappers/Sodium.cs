using System;
using System.Runtime.InteropServices;

namespace Discord.Voice
{
    public unsafe static class Sodium
    {
        [DllImport("libsodium", EntryPoint = "crypto_secretbox_easy", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SecretBoxEasy(byte* output, byte* input, long inputLength, byte[] nonce, byte[] secret);
        [DllImport("libsodium", EntryPoint = "crypto_secretbox_open_easy", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SecretBoxOpenEasy(byte* output, byte* input, long inputLength, byte[] nonce, byte[] secret);

        public static int Encrypt(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset, byte[] nonce, byte[] secret)
        {
            fixed (byte* inPtr = input)
            fixed (byte* outPtr = output)
            {
                int status = SecretBoxEasy(outPtr + outputOffset, inPtr + inputOffset, inputLength, nonce, secret);
                if (status != 0)
                    throw new Exception($"Sodium Error: {status}");
                return inputLength + 16;
            }
        }

        public static int Decrypt(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset, byte[] nonce, byte[] secret)
        {
            fixed (byte* inPtr = input)
            fixed (byte* outPtr = output)
            {
                int status = SecretBoxOpenEasy(outPtr + outputOffset, inPtr + inputOffset, inputLength, nonce, secret);
                if (status != 0)
                    throw new Exception($"Sodium Error: {status}");
                return inputLength - 16;
            }
        }
    }
}
