[System.Serializable]
public class AttributeModifier
{
    public AttributeModifier(string label, AttributeType type, short value)
    {
        this.label = label;
        this.type = type;
        this.value = value;
    }

    public string label;
    [Sirenix.OdinInspector.HideLabel]
    [UnityEngine.HideInInspector]
    public AttributeType type;
    [Sirenix.OdinInspector.HideLabel]
    [UnityEngine.HideInInspector]
    public short value;
}