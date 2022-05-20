using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

namespace MultiplayerARPG.KiwiCoderBT
{
    [DefaultExecutionOrder(int.MaxValue)]
    [RequireComponent(typeof(BehaviourTreeRunner))]
    public class MonsterActivityComponentBT : BaseMonsterActivityComponent
    {
        private BehaviourTreeRunner runner;

        private void Start()
        {
            runner = GetComponent<BehaviourTreeRunner>();
            runner.tree.blackboard.activityComp = this;
            if (!Entity.IsServer)
                runner.enabled = false;
        }

        private void Update()
        {
            if (!Entity.IsServer || Entity.Identity.CountSubscribers() == 0 || CharacterDatabase == null)
            {
                runner.enabled = false;
                return;
            }
            runner.enabled = true;
        }
    }
}
