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
    }
}