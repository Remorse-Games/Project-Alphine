using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Database/TroopData")]
public class TroopData : ScriptableObject
{
    // General
    public string troopName;
    [TextArea]
    public string notes;

    // List of Names
    public List<string> troopAddedList = new List<string>();

    // Index
    public int indexAddedListTemp = -1;
    public int indexAvailableListTemp = -1;
    public int indexAddedList = 0;
    public int indexAvailableList = 0;


    // Sprite Image
    public Sprite background;

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

        troopName = "New Troop";
        background = sp;
        notes = "";
    }
}
