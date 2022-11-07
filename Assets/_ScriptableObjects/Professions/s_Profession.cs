using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public abstract class s_Profession : ScriptableObject
{
    [TableList]
    public List<AttributeModifier> modifiers;
    public abstract void DoJob(Pawn pawn);
}
