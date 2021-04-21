using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface IHandle<in T>
    {
        // TODO - Add context and cancellation token?
        Task HandleAsync(T @event);
    }
}