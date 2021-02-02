using UnityEngine;

namespace LastBoss.AI
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