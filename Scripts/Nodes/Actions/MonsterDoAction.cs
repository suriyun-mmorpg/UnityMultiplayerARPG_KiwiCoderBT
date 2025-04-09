using UnityEngine;

namespace MultiplayerARPG.KiwiCoderBT
{
    public class MonsterDoAction : MonsterActionNode
    {
        private bool didAction;

        protected override void OnStart()
        {
            didAction = false;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (didAction && Entity.IsPlayingActionAnimation())
            {
                ClearActionState();
                return State.Success;
            }

            IDamageableEntity tempTargetEnemy;
            if (!Entity.TryGetTargetEntity(out tempTargetEnemy) || Entity.Characteristic == MonsterCharacteristic.NoHarm)
            {
                // No target, stop attacking
                ClearActionState();
                return State.Failure;
            }

            if (tempTargetEnemy.GetObjectId() == Entity.ObjectId || tempTargetEnemy.IsDeadOrHideFrom(Entity) || !tempTargetEnemy.CanReceiveDamageFrom(Entity.GetInfo()))
            {
                // If target is dead or in safe area stop attacking
                Entity.SetTargetEntity(null);
                ClearActionState();
                return State.Failure;
            }

            Entity.AimPosition = Entity.GetAttackAimPosition(ref blackboard.isLeftHandAttacking);
            if (Entity.IsPlayingActionAnimation())
                return State.Running;

            if (blackboard.queueSkill != null && Entity.IndexOfSkillUsage(SkillUsageType.Skill, blackboard.queueSkill.DataId) < 0)
            {
                // Use skill when there is queue skill or randomed skill that can be used
                Entity.UseSkill(blackboard.queueSkill.DataId, WeaponHandlingState.None, 0, new AimPosition()
                {
                    type = AimPositionType.Position,
                    position = blackboard.queueSkill.GetDefaultAttackAimPosition(Entity, blackboard.queueSkillLevel, blackboard.isLeftHandAttacking, tempTargetEnemy),
                });
            }
            else
            {
                // Attack when no queue skill
                WeaponHandlingState weaponHandlingState = WeaponHandlingState.None;
                Entity.Attack(ref weaponHandlingState);
            }

            didAction = true;
            return State.Running;
        }
    }
}
