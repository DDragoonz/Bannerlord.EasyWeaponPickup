using TaleWorlds.MountAndBlade;

namespace EasyWeaponPickup
{
    public class EasyWeaponPickupSubModule : MBSubModuleBase
    {
        public override void OnBeforeMissionBehaviorInitialize(Mission mission)
        {
            base.OnBeforeMissionBehaviorInitialize(mission);
            mission.AddMissionBehavior(new EasyWeaponPickupMissionBehavior());
        }
    }
}