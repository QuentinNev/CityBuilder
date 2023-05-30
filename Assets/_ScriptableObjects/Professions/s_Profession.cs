using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public abstract class s_Profession : ScriptableObject
{
    public abstract void DoJob(Pawn pawn);
}
