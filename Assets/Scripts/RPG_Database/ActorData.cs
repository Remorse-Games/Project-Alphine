using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Database/ActorData")]
public class ActorData : ScriptableObject
{
    // General
    public string actorName;
    public string actorNickname;
    [TextArea]
    public string description;
    [TextArea]
    public string notes;

    // Folder Path To Help The Ease In Creating PlayMode Animation In Unity
    public string sliceSpritePath;
    public string animationPath;
    public string slicedSpriteLocation;

    // Index
    public int selectedClassIndex;
    public int[] allArmorIndexes;

    // Levels
    public int initLevel;
    public int maxLevel;

    // Sprite Image
    public Sprite face;
    public Sprite characterWorld;
    public Sprite battler;



    public void OnEnable()
    {
        if (actorName == null &&
            actorNickname == null)
        {
            Init();
        }
    }

    public void Init()
    {
        actorName = "New Player";
    }
}