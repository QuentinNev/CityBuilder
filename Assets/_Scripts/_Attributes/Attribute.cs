public class Attribute
{
    public string name;
    short _baseValue;

    public short value { get { return _baseValue; } }
    public short baseValue { get { return _baseValue; } set { _baseValue = value; } }

    public Attribute()
    {
        name = string.Empty;
        _baseValue = 0;
    }

    public Attribute(string name, short value)
    {
        this.name = name;
        this._baseValue = value;
    }
}