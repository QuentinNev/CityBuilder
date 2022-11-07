using System.Collections.Generic;

[System.Serializable]
public class Attribute
{
    public Attribute(short _baseValue)
    {

    }

    short _baseValue;
    public List<AttributeModifier> modifiers;
}