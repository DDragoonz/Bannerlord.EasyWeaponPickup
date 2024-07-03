using System;
using System.Collections.Generic;
using System.Linq;
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
            Input = Mission.GetMissionBehavior<MissionGauntletMainAgentEquipmentControllerView>().Input;
        }

        public override void OnFocusGained(Agent agent, IFocusable focusableObject, bool isInteractable)
        {
            base.OnFocusGained(agent, focusableObject, isInteractable);
            try
            {
                if (isInteractable)
                {
                    Agent focusedAgent = focusableObject as Agent;
                    if (focusedAgent != null && !focusedAgent.IsMount )
                    {
                        return;
                    }
                    
                    canPickup = false;
                    if (debugEnabled)
                    {
                        InformationManager.DisplayMessage(new InformationMessage("pickup disabled"));
                    }

                }
                
                // if (Agent.Main.CanUseObject(focusableObject as UsableMissionObject))
                // {
                //     canPickup = false;
                //     InformationManager.DisplayMessage(new InformationMessage("pickup disabled"));
                // }
            }
            catch (Exception e)
            {
                if (debugEnabled)
                {
                    InformationManager.DisplayMessage(new InformationMessage(e.Message, Colors.Red));
                }
            }

            
        }

        public override void OnFocusLost(Agent agent, IFocusable focusableObject)
        {
            base.OnFocusLost(agent, focusableObject);
            canPickup = true;
            if (debugEnabled)
            {
                InformationManager.DisplayMessage(new InformationMessage("pickup enabled"));
            }
        }

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            if (Agent.Main == null) return;

            // if (debugEnabled)
            // {
            // if (Input.IsKeyReleased(InputKey.Down))
            // {
            //     maxHeight -= 0.1f;
            //     if (maxHeight < 0.1) maxHeight = 0.1f;
            //     InformationManager.DisplayMessage(new InformationMessage("maxheight : "+maxHeight));
            // }
            // if (Input.IsKeyReleased(InputKey.Up))
            // {
            //     maxHeight += 0.1f;
            //     if (maxHeight < 0.1) maxHeight = 0.1f;
            //     InformationManager.DisplayMessage(new InformationMessage("maxheight : "+maxHeight));
            // }
            // if (Input.IsKeyReleased(InputKey.Left))
            // {
            //     minDistance -= 0.1f;
            //     if (minDistance < 0.1) minDistance = 0.1f;
            //     InformationManager.DisplayMessage(new InformationMessage("minDistance : "+minDistance));
            // }
            // if (Input.IsKeyReleased(InputKey.Right))
            // {
            //     minDistance += 0.1f;
            //     if (minDistance < 0.1) minDistance = 0.1f;
            //     InformationManager.DisplayMessage(new InformationMessage("minDistance : "+minDistance));
            // }
            // }

            try
            {
                if (Input == null)
                {
                    return;
                }
                
                
                if (canPickup && !isPressingKey && Input.IsGameKeyPressed(13) /*Input.IsKeyPressed(InputKey.F)*/)
                {
                    isPressingKey = true;
                    MissionEquipment mainEquipment = Agent.Main.Equipment;

                    bool hasEmptySlot = false;

                    HashSet<ItemObject.ItemTypeEnum> pickableItem = new HashSet<ItemObject.ItemTypeEnum>
                    {
                        ItemObject.ItemTypeEnum.OneHandedWeapon,
                        ItemObject.ItemTypeEnum.TwoHandedWeapon,
                        ItemObject.ItemTypeEnum.Polearm,
                        ItemObject.ItemTypeEnum.Shield,
                        ItemObject.ItemTypeEnum.Bow,
                        ItemObject.ItemTypeEnum.Crossbow,
                    };

                    HashSet<WeaponClass> allowedAmmoClass = new HashSet<WeaponClass>();

                    for (EquipmentIndex i = EquipmentIndex.WeaponItemBeginSlot; i < EquipmentIndex.NumPrimaryWeaponSlots; i++)
                    {
                        MissionWeapon equipment = mainEquipment[i];
                        
                        if (equipment.IsEmpty)
                        {
                            hasEmptySlot = true;
                        }
                        else
                        {
                            pickableItem.Remove(equipment.Item.Type); // prevent picking item from same category

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

                    if (allowedAmmoClass.IsEmpty() && pickableItem.IsEmpty())
                    {
                        return;
                    }
                    

                    List<GameEntity> gameEntities = Mission.GetActiveEntitiesWithScriptComponentOfType<SpawnedItemEntity>().ToList();

                    float maxHeightBonus = Agent.Main.MountAgent == null ? 0 : horseHeightBonus;
                    
                    List<GameEntity> reachableDroppedItem = (from x in gameEntities
                        where x.GlobalPosition.AsVec2.DistanceSquared(Agent.Main.Position.AsVec2) <= minDistance
                        && Math.Abs(x.GlobalPosition.Z - Agent.Main.Position.Z) <= maxHeight + maxHeightBonus
                        // orderby x.GetScriptComponents<SpawnedItemEntity>().First().WeaponCopy.IsAnyConsumable() // was causing crash?
                        select x).ToList();
                    
                    // foreach (GameEntity gameEntity in entities)
                    // {
                    // if (debugEnabled)
                    // {
                        //     InformationManager.DisplayMessage(new InformationMessage(gameEntity.Name + " distance : " + (gameEntity.GlobalPosition.AsVec2.DistanceSquared(Agent.Main.Position.AsVec2) + " height : "+Math.Abs(gameEntity.GlobalPosition.Z - Agent.Main.Position.Z))));
                    // }
                    // }

                    SpawnedItemEntity droppedItemToUse = null;
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
                                if (allowedAmmoClass.Contains(droppedItem.WeaponCopy.CurrentUsageItem.WeaponClass))
                                {
                                    droppedItemToUse = droppedItem;
                                    break;
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
                            droppedItemToUse = droppedItem;
                                    
                            break;
                        }

                        if (droppedItemToUse != null)
                        {
                            break;
                        }
                        
                    }

                    if (droppedItemToUse != null)
                    {
                        Agent.Main.UseGameObject(droppedItemToUse);
                        if (debugEnabled)
                        {
                            InformationManager.DisplayMessage(new InformationMessage("equip / use : "+ droppedItemToUse.WeaponName + " distance : " + (droppedItemToUse.GameEntity.GlobalPosition.AsVec2.DistanceSquared(Agent.Main.Position.AsVec2) + " height : "+Math.Abs(droppedItemToUse.GameEntity.GlobalPosition.Z - Agent.Main.Position.Z))));    
                        }
                        
                    }

                    if (debugEnabled)
                    {
                        InformationManager.DisplayMessage(new InformationMessage("usable mission object count : "+gameEntities.Count()));
                        InformationManager.DisplayMessage(new InformationMessage("nearest usable mission object : "+reachableDroppedItem.Count()));    
                    }
                    
                }

                if (Input.IsGameKeyReleased(13) /*Input.IsKeyReleased(InputKey.F)*/)
                {
                    isPressingKey = false;
                }
            }
            catch (Exception e)
            {
                if (debugEnabled)
                {
                    InformationManager.DisplayMessage(new InformationMessage(e.Message, Colors.Red));
                }
            }
            
        }

        // cache
        private bool canPickup = true;
        private bool isPressingKey = false;
        private IInputContext Input = null;
        
        // setttings
        private float minDistance = 2;
        private float maxHeight = 3;
        private float horseHeightBonus = 1;
        
        private bool debugEnabled = false;

    }
}