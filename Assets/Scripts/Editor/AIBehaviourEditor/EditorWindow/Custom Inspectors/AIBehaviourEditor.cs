﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

namespace Remorse.AI
{
    [CustomEditor(typeof(AIBehaviour))]
    public class AIBehaviourEditor : Editor
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

        int clickCount = 0;

        Vector2 scrollPos = Vector2.zero;

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

                    GUILayout.Space(10);

                    EditorGUI.BeginDisabledGroup(myTarget.editPatrolArea);

                        myTarget.radius = EditorGUILayout.Slider("Vision Radius", myTarget.radius, 0, 50);
                        myTarget.fov = EditorGUILayout.Slider("Vision FOV", myTarget.fov, 0, 360);

                    EditorGUI.EndDisabledGroup();

                    GUILayout.Space(10);

                    myTarget.editPatrolArea = GUILayout.Toggle(myTarget.editPatrolArea, "Edit Patrol Area", "Button");
                    break;
            }
        }

        private void OnSceneGUI()
        {
            if (myTarget.editPatrolArea)
            {
                ActiveEditorTracker.sharedTracker.isLocked = true;

                #region GUI

                Handles.BeginGUI();

                    float GUIWidth = 150;
                    float GUIHeight = 300;

                    GUILayout.BeginArea(new Rect(10, 10, GUIWidth, GUIHeight));

                        GUILayout.BeginVertical();
                
                            GUIStyle style = new GUIStyle(GUI.skin.box);
                            GUILayout.Box(" ", style, GUILayout.Width(GUIWidth), GUILayout.Height(GUIHeight));

                            GUILayout.BeginArea(new Rect(0, 0, GUIWidth, GUIHeight));

                                var centerAlign = new GUIStyle(GUI.skin.label);
                                centerAlign.alignment = TextAnchor.MiddleCenter;

                                GUILayout.Space(5);

                                GUILayout.Label("Patrol Point", centerAlign);
                
                                GUILayout.Space(5);

                                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

                                    GUILayout.BeginHorizontal();

                                        int length = myTarget.patrolArea.Count;
                            
                                        string[] pointNames = new string[length];
                                        string[] removeNames = new string[length];

                                        for (int i = 0; i < length; i++)
                                        {
                                            pointNames[i] = string.Format("Point {0}", i + 1);
                                            removeNames[i] = "-";
                                        }

                                        pointIndex = GUILayout.SelectionGrid(
                                            pointIndex,
                                            pointNames,
                                            1
                                        );

                                        removeIndex = GUILayout.SelectionGrid(
                                            removeIndex,
                                            removeNames,
                                            1,
                                            GUILayout.Width(20)
                                        );

                                        if (removeIndex != -1)
                                        {
                                            pointIndex = -1;
                                            myTarget.patrolArea.RemoveAt(removeIndex);
                                            removeIndex = -1;
                                        }

                                    GUILayout.EndHorizontal();
                
                                    if (GUILayout.Button("Add"))
                                    {   
                                        myTarget.patrolArea.Add(new Vector3());
                                        pointIndex = myTarget.patrolArea.Count - 1;
                                    }

                                GUILayout.EndScrollView();
                    
                            GUILayout.EndArea();

                        GUILayout.EndVertical();

                    GUILayout.EndArea();

                Handles.EndGUI();

                #endregion

                // Get Mouse Raycast
                Vector3 mousePosition = Event.current.mousePosition;
                Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    // Drag point when a point is selected
                    if (pointIndex != -1)
                    {
                        Vector3 point = hit.point;
                        myTarget.patrolArea[pointIndex] = new Vector3(
                            Mathf.Round(point.x),
                            point.y,
                            Mathf.Round(point.z)
                        );
                    }

                    // On left mouse down
                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        // click count are used to make this function just called once
                        clickCount++;

                        if (clickCount == 2)
                        {
                            /* Select the point that the mouse click or release the point that selected */

                            int lastPointIndex = -1;

                            Vector3 point = new Vector3(
                                Mathf.Round(hit.point.x),
                                hit.point.y,
                                Mathf.Round(hit.point.z)
                            );

                            for (int i = 0; i < length; i++)
                            {
                                if (point == myTarget.patrolArea[i])
                                {
                                    lastPointIndex = i;
                                    break;
                                }
                            }

                            pointIndex = lastPointIndex == pointIndex ? -1 : lastPointIndex;

                            clickCount = 0;
                        }
                    }
                }

                /* Draw label on each point */

                style.normal.textColor = Color.yellow;
                style.alignment = TextAnchor.MiddleCenter;

                for (int i = 0; i < length; i++)
                {
                    Handles.Label(
                        myTarget.patrolArea[i] + Vector3.up, 
                        string.Format("Point {0}", i + 1), 
                        style
                    );
                }
            }
            else
            {
                ActiveEditorTracker.sharedTracker.isLocked = false;
            }
        }
    }
}