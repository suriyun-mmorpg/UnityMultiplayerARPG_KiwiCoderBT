using MultiplayerARPG;
using MultiplayerARPG.KiwiCoderBT;
using System.Collections.Generic;

namespace TheKiwiCoder
{
    public partial class Blackboard
    {
        public MonsterActivityComponentKiwiCoderBT activityComp;
        public BaseSkill queueSkill;
        public short queueSkillLevel;
        public bool isLeftHandAttacking;
        public List<BaseCharacterEntity> enemies = new List<BaseCharacterEntity>();
    }
}
