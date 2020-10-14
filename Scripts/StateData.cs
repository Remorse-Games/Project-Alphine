using Boo.Lang;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/StateData")]
public class StateData : ScriptableObject
{
    public string stateName;
    public Sprite icon;
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
    }
}
