using System.Collections.Generic;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using MCM.Common;

namespace EasyWeaponPickup
{
    public class EasyWeaponPickupConfig : AttributeGlobalSettings<EasyWeaponPickupConfig>
    {
        public override string Id => "EasyWeaponPickup";
        public override string DisplayName => "Easy Weapon Pickup";
        public override string FolderName => "EasyWeaponPickup";
        public override string FormatType => "json2";
        
        
        [SettingPropertyFloatingInteger("Max Pickup Distance", 0.1f, 10f, "0.0", Order = 1, RequireRestart = false, HintText = "Max weapon pickup distance")]
        [SettingPropertyGroup("Settings", GroupOrder = 1)]
        public float MaxPickupDistance{ get; set; } = 2;
        
        [SettingPropertyFloatingInteger("Max Pickup Height", 1f, 10f, "0.0", Order = 1, RequireRestart = false, HintText = "Max weapon pickup height")]
        [SettingPropertyGroup("Settings", GroupOrder = 1)]
        public float MaxPickupHeight{ get; set; } = 3;
        
        [SettingPropertyFloatingInteger("Pickup Height Bonus Mounted", 0f, 10f, "0.0", Order = 1, RequireRestart = false, HintText = "Pickup height bonus while mounted")]
        [SettingPropertyGroup("Settings", GroupOrder = 1)]
        public float HorseHeightBonus{ get; set; } = 1;
        
        [SettingPropertyBool("Mounted Require Perk", Order = 2, RequireRestart = false, HintText = "Shall picking up while mounted require perk \"Long Reach\"?")]
        [SettingPropertyGroup("Settings", GroupOrder = 1)]
        public bool RequireHorsePerk{ get; set; } = true;

        [SettingPropertyFloatingInteger("Minimum Pickup Height Mounted", 0f, 10f, "0.0", Order = 3, RequireRestart = false, HintText = "Minimum pick up height while mounted. only if perk requirement is true")]
        [SettingPropertyGroup("Settings", GroupOrder = 1)]
        public float MinPickupHeightOnHorse { get; set; } = 1;

        [SettingPropertyBool("Enable Debug", Order = 0, RequireRestart = false, HintText = "Show debug Message")]
        [SettingPropertyGroup("Debug", GroupOrder = 2)]
        public bool DebugEnabled{ get; set; } = false;
    }
}