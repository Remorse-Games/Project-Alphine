using UnityEngine;

public class ClassesData : ScriptableObject
{
    public string className;

    #region ExpVar    
    public AnimationCurve expCurve;
    public int baseValue;
    public int extraValue;
    public int accelA;
    public int accelB;
    #endregion

    #region maxHPVar
    public AnimationCurve maxHPCurve;
    public int minHP;
    public int maxHP;
    public int growthRateHP;
    #endregion
    public int getExp(int level)
    {
            return Mathf.RoundToInt(baseValue*(Mathf.Pow(level-1, 0.9f+accelA/250f))*level*
            (level+1)/(6+Mathf.Pow(level,2)/50/accelB)+(level-1)*extraValue);
    }

    ///<summary>
    /// returns a value based on the levels, min, max, and growth rate values. Used to generate Curve
    ///</summary>
    ///<param name="level"> The X value that wanted to be evaluated </param>
    ///<param name="minVal">The minimal Value of the curve, level 1 value </param>
    ///<param name="maxVal">The maximum value of the curve, level 100 value</param>
    ///<param name="growthRate">0 = normal, 1 = slow, -1 = fast determine how fast should the curve grow</param>
    public int GetCurveValue(int level, int minVal, int maxVal, int growthRate)
    {
        if(growthRate < 0)
        {
            return Mathf.RoundToInt(((minVal-maxVal)/Mathf.Pow(99,2)) * Mathf.Pow((level - 100),2) + maxVal);
        }
        else if (growthRate > 0)
        {
            return Mathf.RoundToInt(((maxVal - minVal)/Mathf.Pow(99,2)) * Mathf.Pow((level - 1),2) + minVal);
        }
        else
        {
            return Mathf.RoundToInt(((maxVal-minVal)/99) *level + minVal);
        }
    }

    public void OnEnable() {
        className = "New Class";
        expCurve = new AnimationCurve();
        maxHPCurve = new AnimationCurve();
        for(int i=0;i<100;i++)
        {
            expCurve.AddKey(i, 0);
            maxHPCurve.AddKey(i,0);
        }
        #region ExpBase
        baseValue = 30;
        extraValue = 20;
        accelA = 30;
        accelB = 30;
        #endregion
        #region maxValBase
        minHP = 450;    
        maxHP = 5350;
        #endregion
    }
}
