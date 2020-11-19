using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTriggerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Unit _unit;
    [SerializeField] private Combat _combat;

    private void Start()
    {
        _unit.EventOnDamage += Damage;
        _unit.EventOnDie += Die;
        _unit.EventOnRevive += Revive;
        _combat.EventOnAttack += Attack;
    }
    private void Damage()
    {
        _animator.SetTrigger("TakeDamage");
    }
    private void Die()
    {
        _animator.SetTrigger("Death");
    }
    private void Revive()
    {
        _animator.ResetTrigger("TakeDamage");
        _animator.ResetTrigger("Attack");
        _animator.SetTrigger("Resuraction");
    }
    private void Attack()
    {
        _animator.SetTrigger("Attack");
    }
}
