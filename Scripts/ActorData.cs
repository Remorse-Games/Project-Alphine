using UnityEngine;

[CreateAssetMenu(menuName = "Database/ActorData")]
public class ActorData : BaseData
{
    public string actorNickname;
    //public ActorClass actorClass;
    public int initLevel;
    public int maxLevel;

    [TextArea]
    public string description;

    public Sprite face;
    public Sprite characterWorld;
    public Sprite battler;

    //TODO : Equipment

    //TODO : Traits

    [TextArea]
    public string notes;

    public void OnEnable()
    {
        Sprite sp = Resources.Load<Sprite>("Image");

        dataName = "player";
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