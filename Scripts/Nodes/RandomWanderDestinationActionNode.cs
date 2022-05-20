using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MultiplayerARPG.KiwiCoderBT
{
    public class RandomWanderDestinationActionNode : ActionNode
    {
        public float randomWanderDistance = 2f;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            // Random position around summoner or around spawn point
            if (blackboard.activityComp.Entity.Summoner != null)
            {
                // Random position around summoner
                blackboard.moveToPosition = blackboard.activityComp.CurrentGameInstance.GameplayRule.GetSummonPosition(blackboard.activityComp.Entity.Summoner);
            }
            else
            {
                // Random position around spawn point
                Vector2 randomCircle = Random.insideUnitCircle * randomWanderDistance;
                if (blackboard.activityComp.CurrentGameInstance.DimensionType == DimensionType.Dimension2D)
                    blackboard.moveToPosition = blackboard.activityComp.Entity.SpawnPosition + new Vector3(randomCircle.x, randomCircle.y);
                else
                    blackboard.moveToPosition = blackboard.activityComp.Entity.SpawnPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);
            }
            return State.Success;
        }
    }
}
