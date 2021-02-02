using UnityEngine;
using UnityEditor;
using LastBoss.Inventory;
using LastBoss.System;

namespace LastBossEditor.Creator
{
    public class ObjectDropCreatorWindow : EditorWindow
    {
        public string objectName;
        public Object objectPrefab;

        [MenuItem("Alife/Object Drop Creator")]
        public static void ShowWindow()
        {
            GetWindow<ObjectDropCreatorWindow>("Object Drop Creator");
        }

        private void OnGUI()
        {
            objectName = EditorGUILayout.TextField("Object Name", "name");
            EditorGUILayout.BeginHorizontal();
            objectPrefab = EditorGUILayout.ObjectField("Object Prefab", objectPrefab, typeof(object), true);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Create"))
            {
                GameObject go = Instantiate((GameObject)objectPrefab);
                go.name = objectName;
                go.AddComponent<ItemDrop>();
                go.AddComponent<DragObject>();

                if (go.GetComponent<Collider>() == null)
                    go.AddComponent<BoxCollider>();
                else
                    go.GetComponent<Collider>().isTrigger = true;
            }
        }
    }
}
