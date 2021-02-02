using UnityEngine;

namespace Remorse.AI
{
    [CreateAssetMenu(menuName = "AI/AI Chase")]
    public class EnemyChase : StateActions
    {
        public override void Execute(AIBehaviour states)
        {
            states.Chase();
        }
    }
}