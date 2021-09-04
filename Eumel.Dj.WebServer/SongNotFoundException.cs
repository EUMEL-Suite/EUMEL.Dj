using System;

namespace Eumel.Dj.WebServer
{
    public class SongNotFoundException : Exception
    {
        public SongNotFoundException(string message) : base(message) { }
    }
}