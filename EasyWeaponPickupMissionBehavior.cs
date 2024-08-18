using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI.Mission;



namespace EasyWeaponPickup
{
    public class EasyWeaponPickupMissionBehavior : MissionLogic
    {
        // public override MissionBehaviorType BehaviorType => MissionBehaviorType.Logic;

        public override void OnRenderingStarted()
        {
            base.OnRenderingStarted();
            try
            {
                MissionGauntletMainAgentEquipmentControllerView agentEquipmentControllerView = Mission.GetMissionBehavior<MissionGauntletMainAgentEquipmentControllerView>();
                if (agentEquipmentControllerView == null)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Unable to find MissionGauntletMainAgentEquipmentControllerView! easy pickup will temporary disabled for this scene!", Colors.Red));
                }
                else
                {
                    _input = agentEquipmentControllerView.Input;
                }
                
            }
            catch (Exception e)
            {
                _input = null;
                if (EasyWeaponPickupConfig.Instance.DebugEnabled)
                {
                    InformationManager.DisplayMessage(new InformationMessage(e.Message, Colors.Red));
                }
            }
            
        }

        public override void OnFocusGained(Agent agent, IFocusable focusableObject, bool isInteractable)
        {
            base.OnFocusGained(agent, focusableObject, isInteractable);
            if (_input == null) return;
            
            try
            {
                if (!isInteractable) return;
                
                if (focusableObject is Agent focusedAgent && !focusedAgent.IsMount ) return;
                    
                _canPickup = false;
                if (EasyWeaponPickupConfig.Instance.DebugEnabled)
                {
                    InformationManager.DisplayMessage(new InformationMessage("pickup disabled"));
                }

            }
            catch (Exception e)
            {
                if (EasyWeaponPickupConfig.Instance.DebugEnabled)
                {
                    InformationManager.DisplayMessage(new InformationMessage(e.Message, Colors.Red));
                }
            }

            
        }

        public override void OnFocusLost(Agent agent, IFocusable focusableObject)
        {
            base.OnFocusLost(agent, focusableObject);
            if (_input == null) return;
            
            _canPickup = true;
            if (EasyWeaponPickupConfig.Instance.DebugEnabled)
            {
                InformationManager.DisplayMessage(new InformationMessage("pickup enabled"));
            }
        }

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            try
            {
                if (Agent.Main == null || _input == null) return;
                
                if (_canPickup && !_isPressingKey && _input.IsGameKeyPressed(13) /*Input.IsKeyPressed(InputKey.F)*/)
                {
                    
                    _isPressingKey = true;
                    
                    GetPickableItemAndAmmoClass(out HashSet<ItemObject.ItemTypeEnum> pickableItem, out HashSet<WeaponClass> allowedAmmoClass);

                    if (allowedAmmoClass.IsEmpty() && pickableItem.IsEmpty()) return;
                    
                    UsableMissionObject itemToUse = GetNearestDroppedItem(pickableItem,allowedAmmoClass);
                    
                    // if no dropped items and allow pick up ammo
                    if (itemToUse == null && allowedAmmoClass.Count > 0)
                    {
                        itemToUse = GetNearestAmmoRefill();
                    }

                    if (itemToUse != null)
                    {
                        Agent.Main.UseGameObject(itemToUse);
                        if (EasyWeaponPickupConfig.Instance.DebugEnabled)
                        {
                            InformationManager.DisplayMessage(new InformationMessage("equip / use : "+ itemToUse + " distance : " + (itemToUse.GameEntity.GlobalPosition.AsVec2.DistanceSquared(Agent.Main.Position.AsVec2) + " height : "+Math.Abs(itemToUse.GameEntity.GlobalPosition.Z - Agent.Main.Position.Z))));    
                        }
                        
                    }
                    
                }

                if (_input.IsGameKeyReleased(13) /*Input.IsKeyReleased(InputKey.F)*/)
                {
                    _isPressingKey = false;
                }
            }
            catch (Exception e)
            {
                if (EasyWeaponPickupConfig.Instance.DebugEnabled)
                {
                    InformationManager.DisplayMessage(new InformationMessage(e.Message, Colors.Red));
                }
            }
            
        }

        private float GetMinimumPickupHeight()
        {
            if (Agent.Main == null) return 0;
            if (Agent.Main.MountAgent == null) return 0; 
            if (!EasyWeaponPickupConfig.Instance.RequireHorsePerk) return 0;
            if (Hero.MainHero == null) return 0;
            
            return Hero.MainHero.GetPerkValue(DefaultPerks.Throwing.LongReach) ? 0 : EasyWeaponPickupConfig.Instance.MinPickupHeightOnHorse;
        }

        private UsableMissionObject GetNearestDroppedItem(HashSet<ItemObject.ItemTypeEnum> pickableItem, HashSet<WeaponClass> allowedAmmoClass)
        {
            List<GameEntity> gameEntities = Mission.GetActiveEntitiesWithScriptComponentOfType<SpawnedItemEntity>().ToList();

            float minPickupHeight = GetMinimumPickupHeight();

            List<GameEntity> reachableDroppedItem = (from x in gameEntities
                where x.GlobalPosition.AsVec2.DistanceSquared(Agent.Main.Position.AsVec2) <= EasyWeaponPickupConfig.Instance.MaxPickupDistance
                      && Math.Abs(x.GlobalPosition.Z - Agent.Main.Position.Z) >= minPickupHeight
                      && Math.Abs(x.GlobalPosition.Z - Agent.Main.Position.Z) <= EasyWeaponPickupConfig.Instance.MaxPickupHeight + MaxHeightBonus
                select x).ToList();
            
            if (EasyWeaponPickupConfig.Instance.DebugEnabled)
            {
                InformationManager.DisplayMessage(new InformationMessage("usable mission object count : "+gameEntities.Count()));
                InformationManager.DisplayMessage(new InformationMessage("nearest usable mission object : "+reachableDroppedItem.Count()));    
            }
            
            foreach (GameEntity gameEntity in reachableDroppedItem)
            {
                foreach (SpawnedItemEntity droppedItem in gameEntity.GetScriptComponents<SpawnedItemEntity>())
                {
                    if (droppedItem.WeaponCopy.IsEmpty || droppedItem.IsDisabledForPlayers || !Agent.Main.CanUseObject(droppedItem))
                    {
                        continue;
                    }

                    if (droppedItem.WeaponCopy.IsAnyConsumable())
                    {
                        if (droppedItem.WeaponCopy.Amount == 0)
                        {
                            continue;
                        }
                        if (allowedAmmoClass.Contains(droppedItem.WeaponCopy.CurrentUsageItem.WeaponClass))
                        {
                            return droppedItem;
                        }
                    }

                    if (!pickableItem.Contains(droppedItem.WeaponCopy.Item.Type))
                    {
                        continue;
                    }

                    if (!Agent.Main.CanQuickPickUp(droppedItem))
                    {
                        continue;
                    }

                    return droppedItem;
                }


            }

            return null;
        }

        private void GetPickableItemAndAmmoClass(out HashSet<ItemObject.ItemTypeEnum> pickableItem, out HashSet<WeaponClass> allowedAmmoClass)
        {
            MissionEquipment mainEquipment = Agent.Main.Equipment;

            bool hasEmptySlot = false;

            pickableItem = new HashSet<ItemObject.ItemTypeEnum>
            {
                ItemObject.ItemTypeEnum.OneHandedWeapon,
                ItemObject.ItemTypeEnum.TwoHandedWeapon,
                ItemObject.ItemTypeEnum.Polearm,
                ItemObject.ItemTypeEnum.Shield,
                ItemObject.ItemTypeEnum.Bow,
                ItemObject.ItemTypeEnum.Crossbow,
            };

            allowedAmmoClass = new HashSet<WeaponClass>();

            for (EquipmentIndex i = EquipmentIndex.WeaponItemBeginSlot; i < EquipmentIndex.NumPrimaryWeaponSlots; i++)
            {
                MissionWeapon equipment = mainEquipment[i];

                if (equipment.IsEmpty)
                {
                    hasEmptySlot = true;
                }
                else
                {
                    
                    if (equipment.Item.Type == ItemObject.ItemTypeEnum.Bow)
                    {
                        pickableItem.Add(ItemObject.ItemTypeEnum.Arrows);
                    }
                    else if (equipment.Item.Type == ItemObject.ItemTypeEnum.Crossbow)
                    {
                        pickableItem.Add(ItemObject.ItemTypeEnum.Bolts);
                    }

                    if (equipment.IsAnyConsumable())
                    {
                        if (equipment.Amount < equipment.MaxAmmo)
                        {
                            allowedAmmoClass.Add(equipment.CurrentUsageItem.WeaponClass);
                        }

                        if (equipment.Amount == 0)
                        {
                            hasEmptySlot = true;
                        }

                    }
                    else
                    {
                        pickableItem.Remove(equipment.Item.Type); // prevent picking item from same category
                    }
                }
            }

            if (hasEmptySlot)
            {
                pickableItem.Add(ItemObject.ItemTypeEnum.Thrown); // thrown item always can be picked up.    
            }
            else
            {
                pickableItem.Clear();
            }
        }

        private UsableMissionObject GetNearestAmmoRefill()
        {
            // find nearest ammo refill point
            List<GameEntity> ammoRefillEntity = Mission.GetActiveEntitiesWithScriptComponentOfType<StandingPointWithWeaponRequirement>().ToList();
                        
            List<GameEntity> reachableAmmoRefill = (from x in ammoRefillEntity
                where x.GlobalPosition.AsVec2.DistanceSquared(Agent.Main.Position.AsVec2) <= EasyWeaponPickupConfig.Instance.MaxPickupDistance
                      && Math.Abs(x.GlobalPosition.Z - Agent.Main.Position.Z) <= EasyWeaponPickupConfig.Instance.MaxPickupDistance + MaxHeightBonus
                select x).ToList();

            foreach (GameEntity gameEntity in reachableAmmoRefill)
            {
                foreach (StandingPointWithWeaponRequirement ammoRefill in gameEntity.GetScriptComponents<StandingPointWithWeaponRequirement>())
                {
                    if (!ammoRefill.IsDisabledForPlayers && Agent.Main.CanUseObject(ammoRefill))
                    {
                        return ammoRefill;
                    }
                }
            }

            return null;
        }

        private float MaxHeightBonus
        {
            get
            {
                if (Agent.Main == null) return 0;
                return Agent.Main.MountAgent == null ? 0 : EasyWeaponPickupConfig.Instance.HorseHeightBonus;
            }
        }

        // cache
        private bool _canPickup = true;
        private bool _isPressingKey = false;
        private IInputContext _input = null;
        
        // settings
        
    }
}