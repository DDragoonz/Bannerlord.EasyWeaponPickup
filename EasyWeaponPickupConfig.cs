using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace EasyWeaponPickup
{
    public class EasyWeaponPickupConfig : AttributeGlobalSettings<EasyWeaponPickupConfig>
    {
        public override string Id => "EasyWeaponPickup";
        public override string DisplayName => "Easy Weapon Pickup";
        public override string FolderName => "EasyWeaponPickup";
        public override string FormatType => "json2";

        public const float DefaultMaxPickupDistance = 2;
        public const float DefaultMaxPickupHeight = 3;
        public const float DefaultHorseHeightBonus = 1;
        public const float DefaultMinPickupHeightOnHorse = 1;


        [SettingPropertyFloatingInteger("{=EWP_00001}Max Pickup Distance", 0.1f, 10f, "0.0m", Order = 1, RequireRestart = false, HintText = "{=EWP_00002}Max weapon pickup distance")]
        [SettingPropertyGroup("{=EWP_00000}Settings", GroupOrder = 1)]
        public float MaxPickupDistance{ get; set; } = DefaultMaxPickupDistance;
        
        [SettingPropertyFloatingInteger("{=EWP_00003}Max Pickup Height", 1f, 10f, "0.0m", Order = 1, RequireRestart = false, HintText = "{=EWP_00004}Max weapon pickup height")]
        [SettingPropertyGroup("{=EWP_00000}Settings", GroupOrder = 1)]
        public float MaxPickupHeight{ get; set; } = DefaultMaxPickupHeight;
        
        [SettingPropertyFloatingInteger("{=EWP_00005}Pickup Height Bonus Mounted", 0f, 10f, "0.0m", Order = 1, RequireRestart = false, HintText = "{=EWP_00006}Pickup height bonus to allow you pick higher objects while mounted")]
        [SettingPropertyGroup("{=EWP_00000}Settings", GroupOrder = 1)]
        public float HorseHeightBonus{ get; set; } = DefaultHorseHeightBonus;
        
        [SettingPropertyBool("{=EWP_00007}Mounted Require Perk", Order = 2, RequireRestart = false, HintText = "{=EWP_00008}Shall picking up while mounted require perk \"Long Reach\"?")]
        [SettingPropertyGroup("{=EWP_00000}Settings", GroupOrder = 1)]
        public bool RequireHorsePerk{ get; set; } = false;

        [SettingPropertyFloatingInteger("{=EWP_00009}Minimum Pickup Height Mounted", 0f, 10f, "0.0m", Order = 3, RequireRestart = false, HintText = "{=EWP_00010}Minimum pick up height while mounted, to allow pick up arrow stuck on tree or walls even without Long Reach perk")]
        [SettingPropertyGroup("{=EWP_00000}Settings", GroupOrder = 1)]
        public float MinPickupHeightOnHorse { get; set; } = DefaultMinPickupHeightOnHorse;

        [SettingPropertyBool("{=EWP_00101}Enable Debug", Order = 0, RequireRestart = false, HintText = "{=EWP_00102}Show debug Message")]
        [SettingPropertyGroup("{=EWP_00100}Debug", GroupOrder = 2)]
        public bool DebugEnabled{ get; set; } = false;
    }
}