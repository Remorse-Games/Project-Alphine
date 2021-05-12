using UnityEngine;

[CreateAssetMenu(menuName = "Database/SkillData")]
public class SkillData : ScriptableObject
{
    //Skill Name and Icon
    public string skillName;
    public Sprite Icon;

    [TextArea]
    public string skillDescription;
    public string skillUserNameMessage;
    public string skillMessage;

    //Index for selected Class.
    public int selectedSkillTypeIndex;
    public int selectedSkillScopeIndex;
    public int selectedSkillOccasionIndex;
    public int selectedSkillHitTypeIndex;
    public int selectedSkillAnimationIndex;

    public int selectedSkillWeaponOneIndex;
    public int selectedSkillWeaponTwoIndex;

    public int selectedTypeIndex;
    public int selectedElementIndex;
    public int selectedCriticalHits;

    //skillType
    public int skillMPCost;
    public int skillTPCost;

    //skillScope skillOccasion

    public int skillSpeed;
    public int skillSuccessLevel;
    public int skillCooldownTime;
    public int skillRepeat;
    public int skillTPGain;

    public string skillFormula;
    public int skillVariance;

    [TextArea]
    public string notes;

    public void OnEnable()
    {
        if (skillName == null)
        {
            Init();
        }
    }

    public void Init()
    {
        Sprite sp = Resources.Load<Sprite>("Image");

        skillName = "New Skill";
        Icon = sp;
        skillDescription = "Insert your description here";
        skillMPCost = 0;
        skillTPCost = 0;
        skillSpeed = 0;
        skillUserNameMessage = "! !";
        skillMessage = "";
        skillSuccessLevel = 100;
        skillRepeat = 1;
        skillTPGain = 10;
        skillFormula = "a.atk * 4 - b.def * 2";
        skillVariance = 20;
        notes = "Skill #1 will be used when you selected the Attack Command";
    }
}
