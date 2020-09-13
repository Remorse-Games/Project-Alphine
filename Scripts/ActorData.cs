using UnityEngine;

public class ActorData : ScriptableObject
{
    public string actorName;
    public string actorNickname;
    //public ActorClass actorClass;
    public int initLevel;
    public int maxLevel;
    public string description;

    public Sprite face;
    public Sprite characterWorld;
    public Sprite battler;

    //TODO : Equipment

    //TODO : Traits

    public string notes;
}