using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "s_AttributesProfile", menuName = "Attributes/s_AttributesProfile", order = 0)]
public class s_AttributesProfile : ScriptableObject
{
    public List<s_Attribute> attributes;
}