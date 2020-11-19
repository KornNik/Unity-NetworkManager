using UnityEngine;

public class HealSkill : UpgradeableSkill
{
    [SerializeField] private int _baseHealAmount = 10;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private int _healAmountByLevel = 1;

    private int _healAmount;


    public override int Level
    {
        set
        {
            base.Level = value;
            _healAmount = _baseHealAmount + _healAmountByLevel * Level;
        }
    }

    protected override void OnCastComplete()
    {
        if (isServer)
        {
            _unit.Stats.AddHealth(_healAmount);
        }
        else _particle.Play();
        base.OnCastComplete();
    }
}
