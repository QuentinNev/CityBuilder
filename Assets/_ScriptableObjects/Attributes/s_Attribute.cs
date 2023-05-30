using UnityEngine;

[CreateAssetMenu(fileName = "s_Attribute", menuName = "Attributes/s_Attribute", order = 0)]
public class s_Attribute : ScriptableObject
{
    public string attributeName;
    public short defaultValue;
    public AttributeType type;
}