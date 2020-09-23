using UnityEngine;

public class ClassesData : ScriptableObject
{
    public string className;
    public AnimationCurve expCurve;
    
    public int baseValue;
    public int extraValue;
    public int accelA;
    public int accelB;

    public int getExp(int level)
    {
            return Mathf.RoundToInt(baseValue*(Mathf.Pow(level-1, 0.9f+accelA/250f))*level*
            (level+1)/(6+Mathf.Pow(level,2)/50/accelB)+(level-1)*extraValue);
    }

    public void OnEnable() {
        className = "New Class";
        expCurve = new AnimationCurve();
        for(int i=0;i<100;i++)
        {
            expCurve.AddKey(i,0);
        }
        baseValue = 30;
        extraValue = 20;
        accelA = 30;
        accelB = 30;
    }
}
