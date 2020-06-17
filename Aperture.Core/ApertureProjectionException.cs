using System;

namespace Aperture.Core
{
    public class ApertureProjectionException : Exception
    {
        public ApertureProjectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}