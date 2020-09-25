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
    public int growthRate;
    #endregion
    public int getExp(int level)
    {
            return Mathf.RoundToInt(baseValue*(Mathf.Pow(level-1, 0.9f+accelA/250f))*level*
            (level+1)/(6+Mathf.Pow(level,2)/50/accelB)+(level-1)*extraValue);
    }

    public int getMaxHP(int level)
    {
        if(growthRate < 0)
        {
            return Mathf.RoundToInt(((minHP-maxHP)/Mathf.Pow(99,2)) * Mathf.Pow((level - 100),2) + maxHP);
        }
        else if (growthRate > 0)
        {
            return Mathf.RoundToInt(((maxHP - minHP)/Mathf.Pow(99,2)) * Mathf.Pow((level - 1),2) + minHP);
        }
        else
        {
            return Mathf.RoundToInt(((maxHP-minHP)/99) *level + minHP);
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
        #region MaxHPBase
        minHP = 450;    
        maxHP = 5350;
        #endregion
    }
}
