public class Attribute
{
    public string name;
    int _baseValue;

    public virtual int value { get { return _baseValue; } }
    public virtual int baseValue { get { return _baseValue; } set { _baseValue = value; } }

    public Attribute()
    {
        name = string.Empty;
        _baseValue = 0;
    }

    public Attribute(string name, int value)
    {
        this.name = name;
        this._baseValue = value;
    }
}