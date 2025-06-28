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

#region GeneralSettings

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
        [SettingPropertyGroup("{=EWP_00100}Debug", GroupOrder = 3)]
        public bool DebugEnabled{ get; set; } = false;

#endregion

#region AllowedWeapon

        [SettingPropertyBool("{=EWP_00201}One Handed", Order = 0, RequireRestart = false, HintText = "{=EWP_00202}Allow Pick up One Handed weapon")]
        [SettingPropertyGroup("{=EWP_00200}Allowed Weapon", GroupOrder = 2)]
        public bool AllowOneHanded{ get; set; } = true;
        
        [SettingPropertyBool("{=EWP_00203}Two Handed", Order = 0, RequireRestart = false, HintText = "{=EWP_00204}Allow Pick up Two Handed weapon")]
        [SettingPropertyGroup("{=EWP_00200}Allowed Weapon", GroupOrder = 2)]
        public bool AllowTwoHanded{ get; set; } = true;
        
        [SettingPropertyBool("{=EWP_00205}Polearm", Order = 0, RequireRestart = false, HintText = "{=EWP_00206}Allow Pick up Polearm")]
        [SettingPropertyGroup("{=EWP_00200}Allowed Weapon", GroupOrder = 2)]
        public bool AllowPolearm{ get; set; } = true;
        
        [SettingPropertyBool("{=EWP_00207}Shield", Order = 0, RequireRestart = false, HintText = "{=EWP_00208}Allow Pick up Shield")]
        [SettingPropertyGroup("{=EWP_00200}Allowed Weapon", GroupOrder = 2)]
        public bool AllowShield{ get; set; } = true;
        
        [SettingPropertyBool("{=EWP_00209}Bow", Order = 0, RequireRestart = false, HintText = "{=EWP_00210}Allow Pick up Bow")]
        [SettingPropertyGroup("{=EWP_00200}Allowed Weapon", GroupOrder = 2)]
        public bool AllowBow{ get; set; } = true;
        
        [SettingPropertyBool("{=EWP_00211}Crossbow", Order = 0, RequireRestart = false, HintText = "{=EWP_00212}Allow Pick up Crossbow")]
        [SettingPropertyGroup("{=EWP_00200}Allowed Weapon", GroupOrder = 2)]
        public bool AllowCrossbow{ get; set; } = true;
        
        [SettingPropertyBool("{=EWP_00213}Thrown", Order = 0, RequireRestart = false, HintText = "{=EWP_00214}Allow Pick up Throwable weapon")]
        [SettingPropertyGroup("{=EWP_00200}Allowed Weapon", GroupOrder = 2)]
        public bool AllowThrown{ get; set; } = true;
        
        [SettingPropertyBool("{=EWP_00215}Pistol", Order = 0, RequireRestart = false, HintText = "{=EWP_00216}Allow Pick up Pistol")]
        [SettingPropertyGroup("{=EWP_00200}Allowed Weapon", GroupOrder = 2)]
        public bool AllowPistol{ get; set; } = true;
        
        [SettingPropertyBool("{=EWP_00217}Musket", Order = 0, RequireRestart = false, HintText = "{=EWP_00218}Allow Pick up Musket")]
        [SettingPropertyGroup("{=EWP_00200}Allowed Weapon", GroupOrder = 2)]
        public bool AllowMusket{ get; set; } = true;

#endregion
        
        
    }
}
