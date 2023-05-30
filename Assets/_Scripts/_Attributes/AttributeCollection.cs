using System.Collections.Generic;

public class AttributeCollection
{
    private Dictionary<AttributeType, Attribute> _attributes;

    public AttributeCollection(s_AttributesProfile profile)
    {
        _attributes = new Dictionary<AttributeType, Attribute>();
        Configure(profile);
    }

    public virtual void Configure(s_AttributesProfile profile)
    {
        foreach (s_Attribute attribute in profile.attributes)
        {
            Attribute attr = CreateOrGetStat(attribute.type);
            attr.name = attribute.attributeName;
            attr.baseValue = attribute.defaultValue;
        }
    }

    public bool Contains(AttributeType type)
    {
        return _attributes.ContainsKey(type);
    }


    public Attribute GetStat(AttributeType statType)
    {
        if (Contains(statType))
        {
            return _attributes[statType];
        }
        return null;
    }

    public T GetStat<T>(AttributeType type) where T : Attribute
    {
        return GetStat(type) as T;
    }

    protected Attribute CreateStat(AttributeType statType)
    {
        Attribute stat = new Attribute();
        _attributes.Add(statType, stat);
        return stat;
    }

    protected Attribute CreateOrGetStat(AttributeType statType)
    {
        Attribute stat = GetStat(statType);
        if (stat == null)
        {
            stat = CreateStat(statType);
        }
        return stat;
    }
}