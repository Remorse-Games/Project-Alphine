using UnityEngine;

[CreateAssetMenu(menuName = "Database/TermData")]
public class TermData : ScriptableObject
{
    public string termLevel;
    public string termHP;
    public string termMP;
    public string termTP;
    public string termEXP;

    public string termLevelabbr;
    public string termHPabbr;
    public string termMPabbr;
    public string termTPabbr;
    public string termEXPabbr;

    public string termMaxHP;
    public string termAttack;
    public string termMAttack;
    public string termAgility;
    public string termHitRate;

    public string termMaxMP;
    public string termDefense;
    public string termMDefense;
    public string termLuck;
    public string termEvasionRate;

    public void OnEnable()
    {
        if (termLevel == null)
        {
            Init();
        }
    }

    public void Init()
    {
        termLevel = "Level";
        termHP = "HP";
        termMP = "MP";
        termTP = "TP";
        termEXP = "EXP";

        termLevelabbr = "Lv";
        termHPabbr = "HP";
        termMPabbr = "MP";
        termTPabbr = "TP";
        termEXPabbr = "EXP";

        termMaxHP = "Max HP";
        termMaxMP = "Max MP";
        termLuck = "Luck";
        termAgility = "Agility";
        termEvasionRate = "Evasion Rate";
        termHitRate = "Hit Rate";
        termAttack = "Attack";
        termDefense = "Defense";
        termMAttack = "M.Attack";
        termMDefense = "M.Defense";
    }
}