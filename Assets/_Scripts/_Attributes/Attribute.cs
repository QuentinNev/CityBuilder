using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum AttributeType { Strength, Constitution, Agility, Intelligence, Charisma}

[System.Serializable]
public class Attribute
{
    public Attribute(AttributeType type, short value)
    {
        _baseValue = value;
        this.type = type;
    }

    [ReadOnly]
    [HideLabel]
    public AttributeType type;
    /// <summary>
    /// Default value (related to Pawn default stats)
    /// </summary>
    short _baseValue;
    /// <summary>
    /// Effective attribute value including all modifiers
    /// </summary>
    [SerializeField]
    [ReadOnly]
    [HideLabel]
    short finalValue;
    public short value { get { return finalValue; } }

    List<AttributeModifier> modifiers = new List<AttributeModifier>();

    public void AddModifier(AttributeModifier modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(AttributeModifier modifier)
    {
        modifiers.Remove(modifier);
    }

    /// <summary>
    /// Called whenever the attribute value is modified to update the final value
    /// (can't use a getter for this because it can't be shown in inspector)
    /// </summary>
    void OnAttributeUpdated()
    {
        finalValue = _baseValue;

        foreach (AttributeModifier mod in modifiers)
            finalValue += mod.value;
    }
}