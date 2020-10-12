﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyData : ScriptableObject
{
    public string[] enemyItem =
       {
        "Potion",
        "Add other manually.",
    };

    public string[] enemyArmor =
    {
        "Shield",
        "Hat",
        "Cloth",
        "Ring",
        "Add other manually.",
    };

    public string[] enemyWeapon =
    {
        "Sword",
        "Add other manually.",
    };
    public string enemyName;
    public Sprite Image;

    public int[] selectedToggle = new int[3];
    public int[] selectedIndex = new int[3];

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

    [TextArea]
    public string notes;

    public void OnEnable()
    {
        Sprite sp = Resources.Load<Sprite>("Image");
        enemyName = "enemy";
        enemyEXP = 0;
        enemyAttack = 30;
        enemyDefense = 30;
        enemyMaxMP = 0;
        enemyMAttack = 30;
        enemyMDefense = 30;
        enemyAgility = 30;
        enemyLuck = 30;
        enemyMaxHP = 500;
        enemyGold = 0;
        Image = sp;
        notes = "write your notes here, it won't affect the game though.";
    }
}
