using UnityEngine;

[CreateAssetMenu(menuName = "Database/TermData")]
public class TermData : ScriptableObject
{
    // Names In Tab

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

    public string commandFight;
    public string commandItem;
    public string commandFormation;
    public string commandEscape;
    public string commandSkill;
    public string commandOption;
    public string commandAttack;
    public string commandEquip;
    public string commandSave;
    public string commandGuard;
    public string commandStatus;
    public string commandGameEnd;

    public string commandWeapon;
    public string commandOptimize;
    public string commandNewGame;
    public string commandArmor;
    public string commandClear;
    public string commandContinue;
    public string commandKeyItem;
    public string commandBuy;
    public string commandToTitle;
    public string commandEquip2;
    public string commandSell;
    public string commandCancel;

    public void OnEnable()
    {
        Init();
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

        commandFight = "Fight";
        commandItem = "Item";
        commandFormation = "Formation";
        commandEscape = "Escape";
        commandSkill = "Skill";
        commandOption = "Option";
        commandAttack = "Attack";
        commandEquip = "Equip";
        commandSave = "Save";
        commandGuard = "Guard";
        commandStatus = "Status";
        commandGameEnd = "GameEnd";

        commandWeapon = "Weapon";
        commandOptimize = "Optimize";
        commandNewGame = "NewGame";
        commandArmor = "Armor";
        commandClear = "Clear";
        commandContinue = "Continue";
        commandKeyItem = "KeyItem";
        commandBuy = "Buy";
        commandToTitle = "ToTitle";
        commandEquip2 = "Equip";
        commandSell = "Sell";
        commandCancel = "Cancel";
    }   
}