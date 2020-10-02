using System;

namespace Discord.Media
{
    internal class SodiumException : Exception
    {
        // libsodium might have return codes that go more in depth than 0 or -1, but i haven't been able to find that anywhere 
    }
}
