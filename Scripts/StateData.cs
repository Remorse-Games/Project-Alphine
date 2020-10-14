using UnityEngine;

[CreateAssetMenu(menuName = "Database/StateData")]
public class StateData : ScriptableObject
{
    public string stateName;
    public Sprite icon;

    public int statePriority;
    public int selectedRestriction;
    public int selectedSVMotion;
    public int selectedSVOverlay;
    public int selectedAutoRemoval;
    public int durationInTurnsA;
    public int durationInTurnsB;
    public int removeByDamageValue;
    public int removeByWalkingValue;

    public bool stateRemoveAt;
    public bool stateRemoveByRestriction;
    public bool stateRemoveByDamage;
    public bool stateRemoveByWalking;

    [TextArea]
    public string notes;

    public void OnEnable()
    {
        if (stateName == null)
        {
            Init();
        }
    }

    public void Init()
    {
        Sprite sp = Resources.Load<Sprite>("Image");

        stateName = "state";
        icon = sp;

        statePriority = 100;

        durationInTurnsA = 1;
        durationInTurnsB = 1;

            
    }
}
