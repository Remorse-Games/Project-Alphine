using UnityEngine;

[CreateAssetMenu(menuName = "Database/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;

    [TextArea]
    public string skillDescription;

    //skillType
    public int skillMPCost;
    public int skillTPCost;

    //skillScope skillOccasion

    public int skillSpeed;
    public int skillSuccessLevel;
    public int skillRepeat;
    public int skillTPGain;

    //skillHitType skillAnimation

    //TODO: Message (User Name "casts *!)

    //skillWeaponType1 and 2
    //skillDamageType skillDamageElement

    public string skillFormula;
    public int skillVariance;

    //TODO: Effects

    [TextArea]
    public string notes;

    public void OnEnable()
    {
        Sprite sp = Resources.Load<Sprite>("Image");

        skillName = "skill";
        skillIcon = sp;
        skillDescription = " ";
        skillMPCost = 0;
        skillTPCost = 0;
        skillSpeed = 0;
        skillSuccessLevel = 100;
        skillRepeat = 1;
        skillTPGain = 10;
        skillFormula = "a.atk * 4 - b.def * 2";
        skillVariance = 20;
        notes = "Skill #1 will be used when you selected the Attack Command";
    }
}
