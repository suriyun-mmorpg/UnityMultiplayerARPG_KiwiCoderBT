namespace MultiplayerARPG.KiwiCoderBT
{
    public class MonsterRandomAction : MonsterActionNode
    {
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

            // If it has target then go to target
            if (!Entity.IsPlayingActionAnimation())
            {
                // Random action state to do next time
                if (CharacterDatabase.RandomSkill(Entity, out blackboard.queueSkill, out blackboard.queueSkillLevel) && blackboard.queueSkill != null)
                {
                    // Cooling down
                    if (Entity.IndexOfSkillUsage(blackboard.queueSkill.DataId, SkillUsageType.Skill) >= 0)
                    {
                        blackboard.queueSkill = null;
                        blackboard.queueSkillLevel = 0;
                    }
                }
                blackboard.isLeftHandAttacking = !blackboard.isLeftHandAttacking;
                return State.Success;
            }
            return State.Running;
        }
    }
}
