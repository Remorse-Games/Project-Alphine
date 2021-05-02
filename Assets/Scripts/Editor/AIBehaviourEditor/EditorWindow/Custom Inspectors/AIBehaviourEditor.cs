using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using Remorse.Utility;

namespace Remorse.AI
{
    [CustomEditor(typeof(AIBehaviour))]
    public class AIBehaviourEditor : Editor
    {
        /* Note from Imandana */
        /* This one must me dynamically check, because not every Chararter has All this AI types */
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
        AIBehaviour obj ;
        
        AICharacter myChar;
        AIMovement myMovement;
        AICombat myCombat;
        AIPatrol myPatrol;
        /* Edited by Imandana */ 
        
        private void OnEnable()
        {
            obj = target as AIBehaviour;
            
            myChar = obj.GetComponent<AICharacter>();
            myMovement =   obj.GetComponent<AIMovement>();
            myCombat =  obj.GetComponent<AICombat>();
            myPatrol =  obj.GetComponent<AIPatrol>();
        }

        public override void OnInspectorGUI()
        {
            /* Note from Imandana */
            /* NOTE : AICharacter, AIMovement, AICombat, AIPatrol script must first Attach to the GameObject FIRST
            /* before attaching THIS class due to statically data */
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

                                        int length = myPatrol.patrolArea.Count;
                            
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
                                            myPatrol.patrolArea.RemoveAt(removeIndex);
                                            removeIndex = -1;
                                        }

                                    GUILayout.EndHorizontal();
                
                                    if (GUILayout.Button("Add"))
                                    {   
                                        myPatrol.patrolArea.Add(new GridVector());
                                        pointIndex = myPatrol.patrolArea.Count - 1;
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
                        myPatrol.patrolArea[pointIndex] = new GridVector(
                            point.x,
                            point.y,
                            point.z
                        );
                    }

                    // On left mouse down
                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && clickTimer <= 0)
                    {
                        int lastPointIndex = -1;

                        Vector3 point = new Vector3(
                            Mathf.Round(hit.point.x),
                            Mathf.Round(hit.point.y),
                            Mathf.Round(hit.point.z)
                        );

                        for (int i = 0; i < length; i++)
                        {
                            if (point == myPatrol.patrolArea[i])
                            {
                                lastPointIndex = i;
                                break;
                            }
                        }

                        pointIndex = lastPointIndex == pointIndex ? -1 : lastPointIndex;

                        clickTimer = 0.1f;
                    }
                }

                /* Draw label on each point */

                style.normal.textColor = Color.yellow;
                style.alignment = TextAnchor.MiddleCenter;

                for (int i = 0; i < length; i++)
                {
                    Handles.Label(
                        myPatrol.patrolArea[i] + Vector3.up, 
                        string.Format("Point {0}", i + 1), 
                        style
                    );
                }

                if (clickTimer > 0)
                {
                    clickTimer -= Time.deltaTime;

                    if (clickTimer < 0)
                    {
                        clickTimer = 0;
                    }
                }
            }
            else
            {
                ActiveEditorTracker.sharedTracker.isLocked = false;

                clickTimer = 0;
            }
        }
    }
}