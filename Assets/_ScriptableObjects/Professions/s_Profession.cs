using UnityEngine;
using System.Collections.Generic;

public abstract class s_Profession : ScriptableObject
{    
    public int bonusStrength;
    public int bonusConstitution;
    public int bonusAgility;
    public int bonusIntelligence;
    public int bonusCharisma;

    public List<AttributeModifier> modifiers;
    public abstract void DoJob(Pawn pawn);
}
