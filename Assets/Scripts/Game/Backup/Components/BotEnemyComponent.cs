using Unity.Entities;
using Unity.Collections;

namespace Game.Components
{
    public struct BotEnemyComponent : IComponentData
    {
        public int Level;
        public int Avatar;
        public float CurrentEnergy;
        public float MaxEnergy;
        public float SpeedEnergy;
        public Team MyTeam;
    }

    public struct BotEnemyNameComponent : IComponentData
    {
        public FixedString64Bytes Name;
    }
}
