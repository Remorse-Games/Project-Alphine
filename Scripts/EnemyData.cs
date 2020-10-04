using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    public string enemyName;

    public void OnEnable()
    {
        Sprite sp = Resources.Load<Sprite>("Image");

        enemyName = "enemy";
    }
}
