using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Sprite Image;

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
    }
}
