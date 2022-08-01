using UnityEngine;

namespace MultiplayerARPG.KiwiCoderBT
{
    public class MonsterMoveToPosition : MonsterActionNode
    {
        public float tolerance = 1.0f;
        public ExtraMovementState extraMovementState = ExtraMovementState.None;

        protected override void OnStart()
        {
            Entity.SetTargetEntity(null);
            Entity.SetExtraMovementState(extraMovementState);
            Entity.PointClickMovement(blackboard.moveToPosition);
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (Vector3.Distance(Entity.EntityTransform.position, blackboard.moveToPosition) < tolerance)
            {
                return State.Success;
            }
            return State.Running;
        }
    }
}
