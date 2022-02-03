using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Profession/Unemployed", order = 1)]
public class s_Unemployed : s_Profession
{
    public override void DoJob(Pawn pawn)
    {
        pawn.Status = "Doing clodo de l'espace"; // quote from a french streamer
    }
}
