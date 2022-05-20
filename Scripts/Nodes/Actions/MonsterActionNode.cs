using TheKiwiCoder;

namespace MultiplayerARPG.KiwiCoderBT
{
    public abstract class MonsterActionNode : ActionNode
    {
        public BaseMonsterCharacterEntity Entity { get { return blackboard.activityComp.Entity; } }
        public MonsterCharacter CharacterDatabase { get { return blackboard.activityComp.CharacterDatabase; } }
        public GameInstance CurrentGameInstance { get { return GameInstance.Singleton; } }
    }
}
