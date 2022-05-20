using UnityEngine;

namespace MultiplayerARPG.KiwiCoderBT
{
    public class MonsterRandomWanderDestination : MonsterActionNode
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
            if (Entity.Summoner != null)
            {
                // Random position around summoner
                blackboard.moveToPosition = CurrentGameInstance.GameplayRule.GetSummonPosition(Entity.Summoner);
            }
            else
            {
                // Random position around spawn point
                Vector2 randomCircle = Random.insideUnitCircle * randomWanderDistance;
                if (CurrentGameInstance.DimensionType == DimensionType.Dimension2D)
                    blackboard.moveToPosition = Entity.SpawnPosition + new Vector3(randomCircle.x, randomCircle.y);
                else
                    blackboard.moveToPosition = Entity.SpawnPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);
            }
            return State.Success;
        }
    }
}
