using UnityEngine;
using UnityEditor;
using LastBoss.Player;
using LastBoss.Craft;
using LastBoss.Combat;

namespace LastBossEditor.Creator
{
    public class PlayerCreatorWindow : EditorWindow
    {

        public Object playerBody;
        public Object playerFeet;
        public Object playerRightHand;
        public Object playerLeftHand;
        public Object playerAntena;

        public ParentPart parentPart;
        public Vector3 bulletSpawnPosition;

        [MenuItem("Alife/Player Creator")]
        public static void ShowWindow()
        {
            GetWindow<PlayerCreatorWindow>("Player Creator Window");
        }

        private void OnGUI()
        {
            GUILayout.Label("This is the Player Creator Window.\n" +
                "Player will be created and need 5 components.\n" +
                "assign those body parts and click create to have the player in the scene.\n");

            EditorGUILayout.BeginHorizontal();
            playerBody = EditorGUILayout.ObjectField("Body", playerBody, typeof(object), false);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            playerFeet = EditorGUILayout.ObjectField("Feet", playerFeet, typeof(object), false);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            playerRightHand = EditorGUILayout.ObjectField("Right Hand", playerRightHand, typeof(object), false);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            playerLeftHand = EditorGUILayout.ObjectField("Left Hand", playerLeftHand, typeof(object), false);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            playerAntena = EditorGUILayout.ObjectField("Antena", playerAntena, typeof(object), false);
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("Bullet Spawn");
            parentPart = (ParentPart)EditorGUILayout.EnumPopup("Parent of Bullet Spawn", parentPart);
            EditorGUILayout.BeginHorizontal();
            bulletSpawnPosition = EditorGUILayout.Vector3Field("Position", bulletSpawnPosition);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Create"))
            {
                PlayerCreator();
            }
        }

        void PlayerCreator()
        {
            GameObject player = new GameObject("Player");
            player.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
            player.tag = "MainPlayer";
            player.layer = LayerMask.NameToLayer("Player");

            //Main Setup
            player.AddComponent<Rigidbody>();
            player.GetComponent<Rigidbody>().useGravity = true;
            player.AddComponent<BoxCollider>();
            player.AddComponent<PlayerController>();
            player.AddComponent<PlayerDevelopment>();
            player.AddComponent<PlayerCombat>();
            player.AddComponent<CraftSystem>();
            player.AddComponent<FadeToMe>();


            GameObject pBody = (GameObject)Instantiate(playerBody, player.transform);
            GameObject pFeet = (GameObject)Instantiate(playerFeet, player.transform);
            GameObject pRightHand = (GameObject)Instantiate(playerRightHand, pBody.transform);
            GameObject pLeftHand = (GameObject)Instantiate(playerLeftHand, pBody.transform);
            GameObject pAntena = (GameObject)Instantiate(playerAntena, pBody.transform);

            GameObject bulletSpawn = new GameObject("Bullet Spawn");
            switch (parentPart)
            {
                case ParentPart.Feet:
                    bulletSpawn.transform.SetParent(player.transform);
                    break;
                case ParentPart.LeftHand:
                    bulletSpawn.transform.SetParent(player.transform);
                    break;
                case ParentPart.RightHand:
                    bulletSpawn.transform.SetParent(player.transform);
                    break;
                case ParentPart.Head:
                    bulletSpawn.transform.SetParent(pFeet.transform);
                    break;
                case ParentPart.Body:
                    bulletSpawn.transform.SetParent(pBody.transform);
                    break;
                default:
                    bulletSpawn.transform.SetParent(player.transform);
                    break;
            }

            bulletSpawn.transform.position = bulletSpawnPosition;

            //Player Setup
            player.GetComponent<PlayerController>().playerRigidbody = pFeet.GetComponent<Rigidbody>();
            player.GetComponent<PlayerCombat>().spawnBullet = bulletSpawn.transform;
            //Head Setup
            pBody.gameObject.tag = "Head";

            //Body Setup
            pBody.AddComponent<Rigidbody>();
            pBody.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

            //Right Hand Setup


            //Left Hand Setup


            //Antena Setup




        }
    }

    public enum ParentPart
    {
        Head, Body, LeftHand, RightHand, Feet
    }
}