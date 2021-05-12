using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyData : ScriptableObject
{
    // General
    public string enemyName;
    [TextArea]
    public string notes;

    // Index
    public int[] selectedToggle = new int[3];
    public int[] selectedIndex = new int[3];

    // Value
    public int[] enemyProbability = new int[3] {1, 1, 1};
    public int enemyAttack;
    public int enemyDefense;
    public int enemyMAttack;
    public int enemyMDefense;
    public int enemyAgility;
    public int enemyLuck;
    public int enemyMaxHP;
    public int enemyMaxMP;
    public int enemyEXP;
    public int enemyGold;

    // Sprite Image
    public Sprite Image;

    public void OnEnable()
    {
        if (enemyName == null)
        {
            Init();
        }
    }
    public void Init()
    {
        enemyName = "New Enemy";
    }
}
