using UnityEngine;

[CreateAssetMenu(menuName = "Database/StateData")]
public class StateData : ScriptableObject
{
    // General
    public string stateName;
    [TextArea]
    public string notes;

    // Index
    public int selectedRestriction;
    public int selectedSVMotion;
    public int selectedSVOverlay;
    public int selectedAutoRemoval;

    // General Settings
    public int statePriority;
    public int durationInTurnsA;
    public int durationInTurnsB;
    public int removeByDamageValue;
    public int removeByWalkingValue;

    // Messages
    public string firstMessageTarget;
    public string secondMessageTarget;
    public string thirdMessageTarget;
    public string fourthMessageTarget;

    // Selection Toogle
    public bool stateRemoveAt;
    public bool stateRemoveByRestriction;
    public bool stateRemoveByDamage;
    public bool stateRemoveByWalking;


    // Sprite Image
    public Sprite icon;

    public void OnEnable()
    {
        if (stateName == null)
        {
            Init();
        }
    }

    public void Init()
    {
        stateName = "New State";
    }
}
