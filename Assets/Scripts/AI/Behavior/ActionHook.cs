using UnityEngine;

namespace Remorse.AI
{
    public class ActionHook : MonoBehaviour
    {
		public Action[] fixedUpdateActions;
		public Action[] updateActions;

		void FixedUpdate()
		{
			if (fixedUpdateActions == null)
				return;

			for (int i = 0; i < fixedUpdateActions.Length; i++)
			{
				fixedUpdateActions[i].Execute();
			}
		}

		void Update()
        {
			if (updateActions == null)
				return;

            for (int i = 0; i < updateActions.Length; i++)
            {
                updateActions[i].Execute();
            }
        }
    }
}
