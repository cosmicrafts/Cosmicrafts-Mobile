using Unity.Entities;

namespace Game.Components
{
    public struct PlayerComponent : IComponentData
    {
        public int ID;
        public Team MyTeam;
        public int CurrentEnergy;
        public int MaxEnergy;
        public float SpeedEnergy;
    }
}
