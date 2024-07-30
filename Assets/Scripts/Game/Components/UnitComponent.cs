using Unity.Entities;

namespace Game.Components
{
    public struct UnitComponent : IComponentData
    {
        public int ID;
        public int PlayerId;
        public Team MyTeam;
        public bool IsFake;
    }
}
