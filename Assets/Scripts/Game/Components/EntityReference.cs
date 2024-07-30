using Unity.Entities;

namespace Game.Components
{
    public struct EntityReference : IComponentData
    {
        public Entity entity;
    }
}
