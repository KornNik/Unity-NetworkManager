using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Stat
{
    [SerializeField] private int _baseValue;

    public delegate void StatChanged(int value);
    public event StatChanged OnStatChanged;

    private List<int> _modifiers = new List<int>();

    public int baseValue
    {
        get { return _baseValue; }
        set
        {
            _baseValue = value;
            if (OnStatChanged != null) OnStatChanged(GetValue());
        }
    }

    public int GetValue()
    {
        int finalValue = _baseValue;
        _modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(int modifier)
    {
        if (modifier != 0)
        {
            _modifiers.Add(modifier);
            if (OnStatChanged != null) { OnStatChanged(GetValue()); }
        }
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
        {
            _modifiers.Remove(modifier);
            if (OnStatChanged != null) { OnStatChanged(GetValue()); }
        }
    }
}