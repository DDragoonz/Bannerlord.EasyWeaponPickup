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


        [SettingPropertyFloatingInteger("Max Pickup Distance", 0.1f, 10f, "0.0m", Order = 1, RequireRestart = false, HintText = "Max weapon pickup distance")]
        [SettingPropertyGroup("Settings", GroupOrder = 1)]
        public float MaxPickupDistance{ get; set; } = DefaultMaxPickupDistance;
        
        [SettingPropertyFloatingInteger("Max Pickup Height", 1f, 10f, "0.0m", Order = 1, RequireRestart = false, HintText = "Max weapon pickup height")]
        [SettingPropertyGroup("Settings", GroupOrder = 1)]
        public float MaxPickupHeight{ get; set; } = DefaultMaxPickupHeight;
        
        [SettingPropertyFloatingInteger("Pickup Height Bonus Mounted", 0f, 10f, "0.0m", Order = 1, RequireRestart = false, HintText = "Pickup height bonus while mounted")]
        [SettingPropertyGroup("Settings", GroupOrder = 1)]
        public float HorseHeightBonus{ get; set; } = DefaultHorseHeightBonus;
        
        [SettingPropertyBool("Mounted Require Perk", Order = 2, RequireRestart = false, HintText = "Shall picking up while mounted require perk \"Long Reach\"?")]
        [SettingPropertyGroup("Settings", GroupOrder = 1)]
        public bool RequireHorsePerk{ get; set; } = false;

        [SettingPropertyFloatingInteger("Minimum Pickup Height Mounted", 0f, 10f, "0.0m", Order = 3, RequireRestart = false, HintText = "Minimum pick up height while mounted. only if perk requirement is true")]
        [SettingPropertyGroup("Settings", GroupOrder = 1)]
        public float MinPickupHeightOnHorse { get; set; } = DefaultMinPickupHeightOnHorse;

        [SettingPropertyBool("Enable Debug", Order = 0, RequireRestart = false, HintText = "Show debug Message")]
        [SettingPropertyGroup("Debug", GroupOrder = 2)]
        public bool DebugEnabled{ get; set; } = false;
    }
}