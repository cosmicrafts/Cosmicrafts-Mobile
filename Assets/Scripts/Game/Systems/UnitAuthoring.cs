using UnityEngine;

namespace Game.Components
{
    public class UnitAuthoring : MonoBehaviour
    {
        public int ID;
        public int PlayerId;
        public Team MyTeam;
        public bool IsFake;
        public int HitPoints;
        public int MaxHitPoints;
        public int Shield;
        public int MaxShield;
        public bool IsDead;
    }
}
