using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/BattleEventData")]
public class BattleEventData : ScriptableObject
{
    public string condition = "Don't Run";
    public int span = 0;

    public List<string> eventCommand = new List<string>();
}
