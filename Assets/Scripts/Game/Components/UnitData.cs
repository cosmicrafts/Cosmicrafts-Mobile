using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Components
{
    public struct UnitData
    {
        public int ID;
        public int PlayerId;
        public Team MyTeam;
        public int HitPoints;
        public int MaxHitPoints;
        public int Shield;
        public int MaxShield;
        public bool IsDead;
    }

    public static class UnitDataBuilder
    {
        public static BlobAssetReference<UnitData> CreateUnitData(int id, int playerId, Team team, int hitPoints, int maxHitPoints, int shield, int maxShield, bool isDead)
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var unitData = ref builder.ConstructRoot<UnitData>();
            unitData.ID = id;
            unitData.PlayerId = playerId;
            unitData.MyTeam = team;
            unitData.HitPoints = hitPoints;
            unitData.MaxHitPoints = maxHitPoints;
            unitData.Shield = shield;
            unitData.MaxShield = maxShield;
            unitData.IsDead = isDead;
            var blobReference = builder.CreateBlobAssetReference<UnitData>(Allocator.Persistent);
            builder.Dispose();
            return blobReference;
        }
    }
}
