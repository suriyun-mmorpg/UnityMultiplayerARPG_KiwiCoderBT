using UnityEngine;

namespace MultiplayerARPG.KiwiCoderBT
{
    public class MonsterDoAction : MonsterActionNode
    {
        [Tooltip("Turn to enemy speed")]
        public float turnToEnemySpeed = 800f;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            IDamageableEntity tempTargetEnemy;
            if (!Entity.TryGetTargetEntity(out tempTargetEnemy) || Entity.Characteristic == MonsterCharacteristic.NoHarm)
            {
                // No target, stop attacking
                ClearActionState();
                return State.Failure;
            }

            if (tempTargetEnemy.Entity == Entity.Entity || tempTargetEnemy.IsHideOrDead() || !tempTargetEnemy.CanReceiveDamageFrom(Entity.GetInfo()))
            {
                // If target is dead or in safe area stop attacking
                Entity.SetTargetEntity(null);
                ClearActionState();
                return State.Failure;
            }

            Vector3 currentPosition = Entity.CacheTransform.position;
            Vector3 targetPosition = tempTargetEnemy.GetTransform().position;
            Vector3 lookAtDirection = (targetPosition - currentPosition).normalized;
            if (lookAtDirection.sqrMagnitude > 0)
            {
                if (CurrentGameInstance.DimensionType == DimensionType.Dimension3D)
                {
                    Quaternion currentLookAtRotation = Entity.GetLookRotation();
                    Vector3 lookRotationEuler = Quaternion.LookRotation(lookAtDirection).eulerAngles;
                    lookRotationEuler.x = 0;
                    lookRotationEuler.z = 0;
                    currentLookAtRotation = Quaternion.RotateTowards(currentLookAtRotation, Quaternion.Euler(lookRotationEuler), turnToEnemySpeed * Time.deltaTime);
                    Entity.SetLookRotation(currentLookAtRotation);
                }
                else
                {
                    // Update 2D direction
                    Entity.SetLookRotation(Quaternion.LookRotation(lookAtDirection));
                }
            }

            Entity.AimPosition = Entity.GetAttackAimPosition(ref blackboard.isLeftHandAttacking);
            if (Entity.IsPlayingActionAnimation())
                return State.Running;

            if (blackboard.queueSkill != null && Entity.IndexOfSkillUsage(blackboard.queueSkill.DataId, SkillUsageType.Skill) < 0)
            {
                // Use skill when there is queue skill or randomed skill that can be used
                Entity.UseSkill(blackboard.queueSkill.DataId, false, 0, new AimPosition()
                {
                    type = AimPositionType.Position,
                    position = tempTargetEnemy.OpponentAimTransform.position,
                });
            }
            else
            {
                // Attack when no queue skill
                Entity.Attack(false);
            }

            ClearActionState();
            return State.Success;
        }
    }
}
