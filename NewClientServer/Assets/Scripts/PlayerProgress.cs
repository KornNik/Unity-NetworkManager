using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    private int _level = 1;
    private int _statPoints;
    private float _exp;
    private float _nextLevelExp = 100;
    private UserData _data;
    private StatsManager _manager;

    public void Load(UserData data)
    {
        _data = data;
        if (data.Level > 0) _level = data.Level;
        _statPoints = data.StatPoints;
        _exp = data.Exp;
        if (data.NextLevelExp > 0) _nextLevelExp = data.NextLevelExp;
    }

    public StatsManager Manager
    {
        set
        {
            _manager = value;
            _manager.Exp = _exp;
            _manager.NextLevelExp = _nextLevelExp;
            _manager.Level = _level;
            _manager.StatPoints = _statPoints;
        }
    }

    public void AddExp(float addExp)
    {
        _data.Exp = _exp += addExp;
        while (_exp >= _nextLevelExp)
        {
            _data.Exp = _exp -= _nextLevelExp;
            LevelUP();
        }

        if (_manager != null)
        {
            _manager.Exp = _exp;
            _manager.Level = _level;
            _manager.NextLevelExp = _nextLevelExp;
            _manager.StatPoints = _statPoints;
        }
    }
    public bool RemoveStatPoint()
    {
        if (_statPoints > 0)
        {
            _data.StatPoints = --_statPoints;
            if (_manager != null) { _manager.StatPoints = _statPoints; }
            return true;
        }
        return false;
    }

    private void LevelUP()
    {
        _data.Level = ++_level;
        _data.NextLevelExp = _nextLevelExp += 100f;
        _data.StatPoints = _statPoints += 3;
    }
}

