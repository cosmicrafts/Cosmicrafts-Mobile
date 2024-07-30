using Unity.Entities;

namespace Game.Components
{
    public struct SpellComponent : IComponentData
    {
        public int ID;
        public int PlayerId;
        public Team MyTeam;
        public bool IsFake;
    }
}
