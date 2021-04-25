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

        bool editPatrolArea;

        AIBehaviour myTarget;

        private void OnEnable()
        {
            myTarget = (AIBehaviour)target;
        }

        public override void OnInspectorGUI()
        {
            index = GUILayout.SelectionGrid(index, tabNames, tabNames.Length);

            switch (index)
            {
                case 0:
                    myTarget.health = EditorGUILayout.FloatField("Health", myTarget.health);
                    break;

                case 1:
                    myTarget.moveSpeed = EditorGUILayout.Slider("Move Speed", myTarget.moveSpeed, 0, 100);
                    myTarget.runSpeed = EditorGUILayout.Slider("Run Speed", myTarget.runSpeed, 0, 100);
                    myTarget.rotateSpeed = EditorGUILayout.Slider("Rotate Speed", myTarget.rotateSpeed, 0, 100);
                    break;

                case 2:
                    myTarget.distanceForCombat = EditorGUILayout.Slider("Distance For Combat", myTarget.distanceForCombat, 0, 50);
                    myTarget.combatCooldown = EditorGUILayout.Slider("Combat Cooldown", myTarget.combatCooldown, 0, 100);

                    GUILayout.Space(10);

                    myTarget.bullet = (GameObject)EditorGUILayout.ObjectField("Bullet", myTarget.bullet, typeof(GameObject), true);
                    myTarget.spawnBullet = (Transform)EditorGUILayout.ObjectField("Spawn Bullet Position", myTarget.spawnBullet, typeof(Transform), true);
                    break;

                case 3:
                    myTarget.currentState = (State)EditorGUILayout.ObjectField("Current State", myTarget.currentState, typeof(State), true);
                    myTarget.radius = EditorGUILayout.Slider("Vision Radius", myTarget.radius, 0, 50);
                    myTarget.fov = EditorGUILayout.Slider("Vision FOV", myTarget.fov, 0, 360);

                    GUILayout.Space(10);

                    editPatrolArea = GUILayout.Toggle(editPatrolArea, "Edit Patrol Area", "Button");
                    break;
            }
        }

        private void OnSceneGUI()
        {
            if (editPatrolArea)
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