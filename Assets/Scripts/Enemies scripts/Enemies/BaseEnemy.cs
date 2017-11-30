using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseEnemy : MonoBehaviour {

    protected float _maxLife = 100;
    protected float _currentLife;
    protected float _damage = 5;
    protected float _speed = 2;
    protected Transform _target;

    protected IMoveEnemy _currentMove;
    protected Dictionary<string, IMoveEnemy> _moveBehaviours;

    protected IAttackEnemy _currentAttack;
    protected Dictionary<string, IAttackEnemy> _attackBehaviours;

    protected abstract void Strategies();

    protected virtual void Damage(float value)
    {
        _currentLife -= value;
        if (_currentLife < 0) Die();
    }
    protected abstract void Die();
}
