using UnityEngine;

public class UnitTriggerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Unit _unit;
    [SerializeField] private Combat _combat;

    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Resuraction = Animator.StringToHash("Resuraction");
    private static readonly int Slash = Animator.StringToHash("Attack");
    private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");

    private void OnEnable()
    {
        _unit.EventOnDamage += Damage;
        _unit.EventOnDie += Die;
        _unit.EventOnRevive += Revive;
        _combat.EventOnAttack += Attack;

    }

    private void Damage()
    {
        _animator.SetTrigger(TakeDamage);
    }

    private void Die()
    {
        _animator.SetTrigger(Death);
    }

    private void Revive()
    {
        _animator.ResetTrigger(TakeDamage);
        _animator.ResetTrigger(Slash);
        _animator.SetTrigger(Resuraction);
    }
    private void Attack()
    {
        _animator.SetTrigger(Slash);
    }

    private void OnDisable()
    {
        _unit.EventOnDamage -= Damage;
        _unit.EventOnDie -= Die;
        _unit.EventOnRevive -= Revive;
        _combat.EventOnAttack -= Attack;

    }



}

