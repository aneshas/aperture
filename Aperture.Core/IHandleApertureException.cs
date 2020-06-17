using System;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface IHandleApertureException
    {
        Task HandleApertureException(Exception e);
    }
}