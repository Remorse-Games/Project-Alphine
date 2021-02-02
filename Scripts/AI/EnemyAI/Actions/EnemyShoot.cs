using UnityEngine;

namespace Remorse.AI
{
    [CreateAssetMenu(menuName = "AI/AI Combat")]
    public class EnemyShoot : StateActions
    {
        public override void Execute(AIBehaviour states)
        {
            states.Combat();
        }
    }
}