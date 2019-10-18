using System;

namespace GracenoteConnector.Library
{
    public class GracenoteException : Exception
    {
        public GracenoteException(string message)
            : base(message)
        {
        }
    }
}
