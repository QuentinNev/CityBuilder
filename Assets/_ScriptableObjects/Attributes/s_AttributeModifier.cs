using UnityEngine;

[CreateAssetMenu(fileName = "Modifier", menuName = "Attributes/Attribute modifier", order = 0)]
public class s_AttributeModifier : ScriptableObject
{
    public enum Type { BaseValueAdd, TotalValueAdd, BaseValuePercent, TotalValuePercent }

    public new string name;
    public Type type;
    public AttributeType attribute;
    public float value;
}