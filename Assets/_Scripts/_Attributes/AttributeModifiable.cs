using System.Collections.Generic;

using UnityEngine;

public class AttributeModifiable : Attribute, IAttributeModifiable
{
    private List<s_AttributeModifier> mods;
    public int attributeModifierValue { get; set; }

    public override int value { get { return (short)(base.value + attributeModifierValue); } }

    public AttributeModifiable()
    {
        attributeModifierValue = 0;
        mods = new List<s_AttributeModifier>();
    }

    public void AddModifier(s_AttributeModifier mod)
    {
        mods.Add(mod);
        UpdateModifiers();
    }

    public void RemoveModifier(s_AttributeModifier mod)
    {
        mods.Remove(mod);
        UpdateModifiers();
    }

    public void ClearModifiers()
    {
        mods.Clear();
        UpdateModifiers();
    }

    public void UpdateModifiers()
    {
        attributeModifierValue = 0;
        float statModBaseValueAdd = 0;
        float statModBaseValuePercent = 0;
        float statModTotalValueAdd = 0;
        float statModTotalValuePercent = 0;

        foreach (s_AttributeModifier mod in mods)
        {
            switch (mod.type)
            {
                case s_AttributeModifier.Type.BaseValueAdd:
                    statModBaseValueAdd += mod.value;
                    break;
                case s_AttributeModifier.Type.BaseValuePercent:
                    statModBaseValuePercent += mod.value;
                    break;
                case s_AttributeModifier.Type.TotalValueAdd:
                    statModTotalValueAdd += mod.value;
                    break;
                case s_AttributeModifier.Type.TotalValuePercent:
                    statModTotalValuePercent += mod.value;
                    break;
                default:
                    Debug.LogError($"{mod.type} isn't implemented");
                    break;
            }
        }

        attributeModifierValue = (int)((baseValue * statModBaseValuePercent) + statModBaseValueAdd);
        attributeModifierValue += (int)((value * statModTotalValuePercent) + statModBaseValueAdd);
    }
}