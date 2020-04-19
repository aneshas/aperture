using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface IHandleEvent<in T>
    {
        Task Handle(T @event);
    }
}