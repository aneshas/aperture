using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface IHandleEvent<in T>
    {
        Task HandleAsync(T @event);
    }
}