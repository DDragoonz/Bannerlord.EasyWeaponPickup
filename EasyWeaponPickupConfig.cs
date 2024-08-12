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


        [SettingPropertyFloatingInteger("{=d62cd6Cfbf}Max Pickup Distance", 0.1f, 10f, "0.0", Order = 1, RequireRestart = false, HintText = "{=f3009bd2a5}Max weapon pickup distance")]
        [SettingPropertyGroup("{=aB4e0AFC78}Settings", GroupOrder = 1)]
        public float MaxPickupDistance { get; set; } = 2;

        [SettingPropertyFloatingInteger("{=8A792195Fe}Max Pickup Height", 1f, 10f, "0.0", Order = 1, RequireRestart = false, HintText = "{=442565bF83}Max weapon pickup height")]
        [SettingPropertyGroup("{=aB4e0AFC78}Settings", GroupOrder = 1)]
        public float MaxPickupHeight { get; set; } = 3;

        [SettingPropertyFloatingInteger("{=4a9a53132e}Pickup Height Bonus Mounted", 0f, 10f, "0.0", Order = 1, RequireRestart = false, HintText = "{=7f461F5ce5}Pickup height bonus while mounted")]
        [SettingPropertyGroup("{=aB4e0AFC78}Settings", GroupOrder = 1)]
        public float HorseHeightBonus { get; set; } = 1;

        [SettingPropertyBool("{=A691579aA0}Mounted Require Perk", Order = 2, RequireRestart = false, HintText = "{=2A1F2540ac}Shall picking up while mounted require perk \"Long Reach\"?")]
        [SettingPropertyGroup("{=aB4e0AFC78}Settings", GroupOrder = 1)]
        public bool RequireHorsePerk { get; set; } = true;

        [SettingPropertyFloatingInteger("{=A7F6605961}Minimum Pickup Height Mounted", 0f, 10f, "0.0", Order = 3, RequireRestart = false, HintText = "{=9949d78018}Minimum pick up height while mounted. only if perk requirement is true")]
        [SettingPropertyGroup("{=aB4e0AFC78}Settings", GroupOrder = 1)]
        public float MinPickupHeightOnHorse { get; set; } = 1;

        [SettingPropertyBool("{=Ae900c8c50}Enable Debug", Order = 0, RequireRestart = false, HintText = "{=B6E7024FFf}Show debug Message")]
        [SettingPropertyGroup("{=527a3d7b36}Debug", GroupOrder = 2)]
        public bool DebugEnabled { get; set; } = false;
    }
}