using Engine.Core.Models.Interfaces;

using System.Threading.Tasks;

namespace Engine.Core.Factories.Interfaces
{
    public interface IEntityFactory
    {
        Task<T> Create<T>() where T : IEntity, new();
        Task<TChild> Create<TParent, TChild>() where TChild : TParent, new();
    }
}
