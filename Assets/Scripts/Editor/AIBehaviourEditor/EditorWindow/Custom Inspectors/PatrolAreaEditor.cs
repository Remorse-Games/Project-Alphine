using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

namespace Remorse.AI
{
    [CustomEditor(typeof(AIBehaviour))]
    public class PatrolAreaEditor : Editor
    {
         public string[] tabNames =
        {
            "Character",
            "Movement",
            "Combat",
            "Patrol"
        };

        int index = 0;
        int pointIndex = -1;
        int removeIndex = -1;

        float clickTimer = 0;

        Vector2 scrollPos = Vector2.zero;

        /* Edited by Imandana */
        AICharacter myChar;
        AIMovement myMovement;
        AICombat myCombat;
        AIPatrol myPatrol;
        /* Edited by Imandana */
        
        private void OnEnable()
        {
            myChar = target as AICharacter;
            myMovement =  target as AIMovement;
            myCombat = target as AICombat;
            myPatrol = target as AIPatrol;
        }

        public override void OnInspectorGUI()
        {
            index = GUILayout.SelectionGrid(index, tabNames, tabNames.Length);

            switch (index)
            {
                case 0:
                    myChar.health = EditorGUILayout.FloatField("Health", myChar.health);
                    break;

                case 1:
                    myMovement.moveSpeed = EditorGUILayout.Slider("Move Speed", myMovement.moveSpeed, 0, 100);
                    myMovement.runSpeed = EditorGUILayout.Slider("Run Speed", myMovement.runSpeed, 0, 100);
                    myMovement.rotateSpeed = EditorGUILayout.Slider("Rotate Speed", myMovement.rotateSpeed, 0, 100);
                    break;

                case 2:
                    myCombat.distanceForCombat = EditorGUILayout.Slider("Distance For Combat", myCombat.distanceForCombat, 0, 50);
                    myCombat.combatCooldown = EditorGUILayout.Slider("Combat Cooldown", myCombat.combatCooldown, 0, 100);

                    GUILayout.Space(10);

                    myCombat.bullet = (GameObject)EditorGUILayout.ObjectField("Bullet", myCombat.bullet, typeof(GameObject), true);
                    myCombat.spawnBullet = (Transform)EditorGUILayout.ObjectField("Spawn Bullet Position", myCombat.spawnBullet, typeof(Transform), true);
                    break;

                case 3:
                    myPatrol.currentState = (State)EditorGUILayout.ObjectField("Current State", myPatrol.currentState, typeof(State), true);

                    GUILayout.Space(10);

                    EditorGUI.BeginDisabledGroup(myPatrol.editPatrolArea);

                        myPatrol.radius = EditorGUILayout.Slider("Vision Radius", myPatrol.radius, 0, 50);
                        myPatrol.fov = EditorGUILayout.Slider("Vision FOV", myPatrol.fov, 0, 360);

                    EditorGUI.EndDisabledGroup();

                    GUILayout.Space(10);

                    myPatrol.editPatrolArea = GUILayout.Toggle(myPatrol.editPatrolArea, "Edit Patrol Area", "Button");
                    break;
            }
        }

        private void OnSceneGUI()
        {
            if (myPatrol.editPatrolArea)
            {
                ActiveEditorTracker.sharedTracker.isLocked = true;

                Vector3 mousePosition = Event.current.mousePosition;
                Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {

                    }
                }
            }
            else
            {
                ActiveEditorTracker.sharedTracker.isLocked = false;
            }
        }
    }
}