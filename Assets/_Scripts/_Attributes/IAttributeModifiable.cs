public interface IAttributeModifiable
{
    int attributeModifierValue { get; }

    void AddModifier(s_AttributeModifier mod);
    void RemoveModifier(s_AttributeModifier mod);
    void ClearModifiers();
    void UpdateModifiers();
}