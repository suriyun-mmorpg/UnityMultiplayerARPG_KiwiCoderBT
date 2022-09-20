using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerARPG.KiwiCoderBT
{
    public class MonsterFindEnemy : MonsterActionNode
    {
        [Tooltip("If this is TRUE, monster will attacks buildings")]
        public bool isAttackBuilding = false;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (FindEnemyFunc())
                return State.Success;
            return State.Failure;
        }

        /// <summary>
        /// Return `TRUE` if found enemy
        /// </summary>
        /// <returns></returns>
        public bool FindEnemyFunc()
        {
            IDamageableEntity targetEntity;
            if (!Entity.TryGetTargetEntity(out targetEntity) || targetEntity.Entity == Entity.Entity ||
                 targetEntity.IsDead() || !targetEntity.CanReceiveDamageFrom(Entity.GetInfo()))
            {
                BaseCharacterEntity enemy;
                for (int i = blackboard.enemies.Count - 1; i >= 0; --i)
                {
                    enemy = blackboard.enemies[i];
                    blackboard.enemies.RemoveAt(i);
                    if (enemy != null && enemy.Entity != Entity.Entity && !enemy.IsDead() &&
                        enemy.CanReceiveDamageFrom(Entity.GetInfo()))
                    {
                        // Found target, attack it
                        Entity.SetAttackTarget(enemy);
                        return true;
                    }
                }

                // If no target enemy or target enemy is dead, Find nearby character by layer mask
                blackboard.enemies.Clear();
                if (Entity.IsSummonedAndSummonerExisted)
                {
                    // Find enemy around summoner
                    blackboard.enemies.AddRange(Entity.FindAliveCharacters<BaseCharacterEntity>(
                        Entity.Summoner.EntityTransform.position,
                        CharacterDatabase.SummonedVisualRange,
                        false, /* Don't find an allies */
                        true,  /* Always find an enemies */
                        true));
                }
                else
                {
                    blackboard.enemies.AddRange(Entity.FindAliveCharacters<BaseCharacterEntity>(
                        CharacterDatabase.VisualRange,
                        false, /* Don't find an allies */
                        true,  /* Always find an enemies */
                        false  /* Don't find an neutral */));
                }

                for (int i = blackboard.enemies.Count - 1; i >= 0; --i)
                {
                    enemy = blackboard.enemies[i];
                    blackboard.enemies.RemoveAt(i);
                    if (enemy != null && enemy.Entity != Entity.Entity && !enemy.IsDead() &&
                        enemy.CanReceiveDamageFrom(Entity.GetInfo()))
                    {
                        // Found target, attack it
                        Entity.SetAttackTarget(enemy);
                        return true;
                    }
                }

                if (!isAttackBuilding)
                    return false;
                // Find building to attack
                List<BuildingEntity> buildingEntities = Entity.FindAliveDamageableEntities<BuildingEntity>(CharacterDatabase.VisualRange, CurrentGameInstance.buildingLayer.Mask);
                foreach (BuildingEntity buildingEntity in buildingEntities)
                {
                    // Attack target settings
                    if (buildingEntity == null || buildingEntity.Entity == Entity.Entity ||
                        buildingEntity.IsDead() || !buildingEntity.CanReceiveDamageFrom(Entity.GetInfo()))
                    {
                        // If building is null or cannot receive damage from monster, skip it
                        continue;
                    }
                    if (Entity.Summoner != null)
                    {
                        if (Entity.Summoner.Id.Equals(buildingEntity.CreatorId))
                        {
                            // If building was built by summoner, skip it
                            continue;
                        }
                    }
                    // Found target, attack it
                    Entity.SetAttackTarget(buildingEntity);
                    return true;
                }
            }

            return true;
        }
    }
}
