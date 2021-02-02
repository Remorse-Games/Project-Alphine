using UnityEngine;
namespace Remorse.AI
{
    [CreateAssetMenu(menuName = "AI/Condition/In Combat Area")]
    public class EnemyInCombatArea : Condition
    {
        public override bool CheckCondition(AIBehaviour state)
        {
            if (state.IsInRange(state.distanceForCombat))
            {
                return true;
            }
            else
                return false;
        }
    }
}