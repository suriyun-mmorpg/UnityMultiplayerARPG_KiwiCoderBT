using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MultiplayerARPG.KiwiCoderBT
{
    public class EntityMoveToPositionActionNode : ActionNode
    {
        public float tolerance = 1.0f;

        protected override void OnStart()
        {
            blackboard.activityComp.Entity.SetTargetEntity(null);
            blackboard.activityComp.Entity.SetExtraMovementState(ExtraMovementState.IsWalking);
            blackboard.activityComp.Entity.PointClickMovement(blackboard.moveToPosition);
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (Vector3.Distance(blackboard.activityComp.Entity.CacheTransform.position, blackboard.moveToPosition) < tolerance)
            {
                return State.Success;
            }
            return State.Running;
        }
    }
}
