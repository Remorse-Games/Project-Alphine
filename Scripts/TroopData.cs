using UnityEngine;

[CreateAssetMenu(menuName = "Database/TroopData")]
public class TroopData : ScriptableObject
{
    public string troopName;
    public Sprite background;


    [TextArea]
    public string notes;

    public void OnEnable()
    {
        if (troopName == null)
        {
            Init();
        }
    }

    public void Init()
    {
        Sprite sp = Resources.Load<Sprite>("Image");

        troopName = "troop";
        background = sp;
        notes = "";
    }
}
