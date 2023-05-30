using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnManager : MonoBehaviour
{
    public static PawnManager singleton;
    public s_Profession defaultProfession;
    public s_AttributesProfile defaultAttributes;

    private void Awake()
    {
        singleton = this;
    }

    public Pawn pawnPrefab;
    public List<Pawn> pawns;
    public List<s_Profession> professions;

    private void Update()
    {
        foreach (Pawn pawn in pawns)
        {
            try
            {
                pawn.UpdateBehavior();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public s_AttributesProfile GetRandomAttributes()
    {
        s_AttributesProfile profile = Instantiate(defaultAttributes);

        foreach (s_Attribute attr in profile.attributes)
        {
            attr.defaultValue = (short)Random.Range(0, 5);
        }

        return profile;
    }
}
