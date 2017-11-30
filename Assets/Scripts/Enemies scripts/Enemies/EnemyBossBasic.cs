#define FP_VERBOSE

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine_BossBasicEnemy))]
public class EnemyBossBasic : BaseEnemy {

    public GameObject Arm;
    public Transform bulletSpawner;
    public Transform target { get { return _target; } }
    public bool isAttacking { get; set; }
    public float life = 100;
    private float _speedRotation = 50f;

    public const string KEY_MOVE_FORWARD = "MoveForward";
    public const string KEY_MOVE_SIN = "MoveSin";
    public const string KEY_MOVE_SIN_IDLE = "MoveSinIdle";
    public const string KEY_ATTACK_SIMPLE = "AttackSimple";
    public const string KEY_ATTACK_BERSERK = "AttackBerserk";


    private void Start(){
        ManagerUpdate.instance.updateFixed += Execute; 

        _speed = 5;
        Strategies();
        _target = FindObjectOfType<Hero>().gameObject.transform;
    }


    protected override void Strategies(){

        //Move behaviors
        _moveBehaviours = new Dictionary<string, IMoveEnemy>();
        _moveBehaviours.Add(KEY_MOVE_FORWARD, new MoveForwardEnemy());
        _moveBehaviours.Add(KEY_MOVE_SIN, new MoveSinEnemy());
        _moveBehaviours.Add(KEY_MOVE_SIN_IDLE, new MoveSinIdleEnemy());

        _currentMove = _moveBehaviours[KEY_MOVE_FORWARD];

        //Attack behaviors
        _attackBehaviours = new Dictionary<string, IAttackEnemy>();
        _attackBehaviours.Add(KEY_ATTACK_SIMPLE, new AttackSimple());
        _attackBehaviours.Add(KEY_ATTACK_BERSERK, new AttackBerserk());

        _currentAttack = _attackBehaviours[KEY_ATTACK_SIMPLE];
    }


    void Execute(){
        _currentLife = life;
    }

    /// <summary> The return value is of 0 to 1. </summary>
    public float GetPercentageCurrentLife { get { return _currentLife / _maxLife; } }

    internal void _Attaking() {
        _currentAttack.Attack(bulletSpawner.transform);
        Arm.transform.LookAt(_target);

    }

    internal void _Moving() {
        transform.position -= _currentMove.Move(transform, _target, _speed);
        Arm.transform.LookAt(_target);
    }

    internal void _ChangeMove(string value){
         _currentMove = _moveBehaviours[value];
    }

    internal void _ChangeAttack(string value)
    {
        _currentAttack = _attackBehaviours[value];
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    protected override void Damage(float value){
        base.Damage(value);
    }

    protected override void Die(){
        print("Die: Basic boss");
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        ManagerUpdate.instance.updateFixed -= Execute;
    }
}
