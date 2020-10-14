using Boo.Lang;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/TroopData")]
public class TroopData : ScriptableObject
{
    public string troopName;
    public Sprite background;
    public List<string> troopAddedList = new List<string>();
    public int indexAddedListTemp = -1;
    public int indexAvailableListTemp = -1;
    public int indexAddedList = 0;
    public int indexAvailableList = 0;

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
