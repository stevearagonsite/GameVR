#define FP_VERBOSE

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine_BossBasicEnemy))]
public class EnemyBossBasic : BaseEnemy {

    public Transform target { get { return _target; } }
    public float life;
    private float _speedRotation = 50f;
    private Transform _bulletSpawner;

    void Update()
    {
        _currentLife = life;
    }

    /// <summary> The return value is of 0 to 1. </summary>
    public float GetPercentageCurrentLife { get { return _currentLife / _maxLife; } }

    internal void _Attaking(){
        _currentAttack = (GetPercentageCurrentLife < 0.3f) ? _attackBehaviours["AttackBerserk"] : _attackBehaviours["AttackSimple"];
        _currentAttack.Attack(_bulletSpawner.transform);
    }

    internal void _Moving() {
        _currentMove = (GetPercentageCurrentLife < 0.3f) ? _moveBehaviours["MoveSin"] : _moveBehaviours["MoveForward"];
        transform.position -= _currentMove.Move(transform, _target, _speed);
        transform.LookAt(_target.transform);
    }

    protected override void Strategies(){

        //Move behaviors
        IMoveEnemy move1 = new MoveForwardEnemy();
        IMoveEnemy move2 = new MoveSinEnemy();

        _moveBehaviours = new Dictionary<string, IMoveEnemy>();
        _moveBehaviours.Add("MoveForward", move1);
        _moveBehaviours.Add("MoveSin", move2);

        _currentMove = _moveBehaviours["MoveSin"];

        //Attack behaviors
        IAttackEnemy attack1 = new AttackSimple();
        IAttackEnemy attack2 = new AttackBerserk();

        _attackBehaviours = new Dictionary<string, IAttackEnemy>();
        _attackBehaviours.Add("AttackSimple", attack1);
        _attackBehaviours.Add("AttackBerserk", attack2);

        _currentAttack = _attackBehaviours["AttackSimple"];
    }

    protected override void Damage(float value){
        base.Damage(value);
    }

    protected override void Die(){
        print("Die: Basic");
        Destroy(this.gameObject);
    }

    private void Start(){
        Strategies();
        _target = FindObjectOfType<Hero>().gameObject.transform;
        _bulletSpawner = GetComponentInChildren<Reference>().gameObject.transform;
    }
}
