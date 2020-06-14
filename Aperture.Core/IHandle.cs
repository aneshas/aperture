using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface IHandle<in T>
    {
        Task HandleAsync(T @event);
    }
}