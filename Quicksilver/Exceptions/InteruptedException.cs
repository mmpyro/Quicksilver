using System;

namespace Quicksilver.Exceptions
{
    public class InteruptedException : Exception
    {
        public InteruptedException()
        {

        }

        public InteruptedException(string message) : base(message)
        {

        }
    }
}
