using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Database/ActorData")]
public class ActorData : ScriptableObject
{
    public string actorName;
    public string actorNickname;

    // Folder Path
    public string sliceSpritePath;
    public string animationPath;
    public string slicedSpriteLocation;

    public int initLevel;
    public int selectedClassIndex;
    public int maxLevel;

    [TextArea]
    public string description;

    public Sprite face;
    public Sprite characterWorld;
    public Sprite battler;

    //TODO : Equipment
    public int[] allArmorIndexes;
    //TODO : Traits

    [TextArea]
    public string notes;

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
        Sprite sp = Resources.Load<Sprite>("Image");

        actorName = "New Player";
        actorNickname = "actorNickname";
        initLevel = 1;
        maxLevel = 99;
        description = "insert your description here";
        face = sp;
        characterWorld = sp;
        battler = sp;
        notes = "write your notes here, it won't affect the game though.";
    }
}