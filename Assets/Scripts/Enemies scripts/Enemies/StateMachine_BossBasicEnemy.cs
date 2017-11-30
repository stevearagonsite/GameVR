using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FP;

[RequireComponent(typeof(EnemyBossBasic))]
public class StateMachine_BossBasicEnemy : MonoBehaviour {

    public enum EnemyBossBasicAction { Null, Pursuit, Attack, AttackBerserk}
    EventFSM<EnemyBossBasicAction> _fsm;
    private EnemyBossBasic _entity;
    private float _minDistanceAttack = 10;
    private float _berserkActivePercentage = 0.3f;


    void Start () {
        _entity = GetComponent<EnemyBossBasic>();
        ManagerUpdate.instance.update += Execute;

        var Chasing = new State<EnemyBossBasicAction>("Chasing");
        var Attacking = new State<EnemyBossBasicAction>("Attacking");
        var AttackingBerserk = new State<EnemyBossBasicAction>("AttackingBerserk");
        var Damaged = new State<EnemyBossBasicAction>("Damaged");

        Chasing
            .SetTransition(EnemyBossBasicAction.Attack, Attacking)
            .SetTransition(EnemyBossBasicAction.AttackBerserk, AttackingBerserk)
            ;
        Attacking
            .SetTransition(EnemyBossBasicAction.Pursuit, Chasing)
            .SetTransition(EnemyBossBasicAction.AttackBerserk, AttackingBerserk)
            ;
        AttackingBerserk
            .SetTransition(EnemyBossBasicAction.Pursuit, Chasing)
            .SetTransition(EnemyBossBasicAction.AttackBerserk, AttackingBerserk)
            ;

        //Pursuit fsm.
        Chasing.OnUpdate += () => {
            if (_entity.GetPercentageCurrentLife < _berserkActivePercentage)
                _entity._ChangeMove("MoveSin");

            _entity._Moving();
        };

        //Attack fsm.
        Attacking.OnEnter += () => {
            _minDistanceAttack = 20;
            _entity._ChangeAttack("AttackSimple");
            _entity.isAttacking = true;
        };

        Attacking.OnUpdate += () => {
            _entity._Attaking();
        };

        Attacking.OnExit += () => {
            _minDistanceAttack = 10;
            _entity.isAttacking = false;
        };

        //Attack berserk fsm.
        AttackingBerserk.OnEnter += () => {
            _minDistanceAttack = 30;
            _entity._ChangeAttack("AttackBerserk");
            _entity._ChangeMove("MoveSinIdle");

            _entity.isAttacking = true;
        };

        AttackingBerserk.OnUpdate += () => {
            _entity._Attaking();
            _entity._Moving();
        };

        AttackingBerserk.OnExit += () => {
            _minDistanceAttack = 10;
            _entity.isAttacking = false;
        };


        _fsm = new EventFSM<EnemyBossBasicAction>(Chasing);
    }

    private void Execute()
    {
        if (!_fsm.Equals(EnemyBossBasicAction.Attack) && !_fsm.Equals(EnemyBossBasicAction.AttackBerserk)
            && Vector3.Distance(_entity.transform.position, _entity.target.position) < _minDistanceAttack)
        {

            if (_entity.GetPercentageCurrentLife > _berserkActivePercentage)
                _fsm.Feed(EnemyBossBasicAction.Attack);
            else
                _fsm.Feed(EnemyBossBasicAction.AttackBerserk);
        }
        else if (!_fsm.Equals(EnemyBossBasicAction.Pursuit))
        {

            _fsm.Feed(EnemyBossBasicAction.Pursuit);
        }
        
        _fsm.Update();
    }


    private void OnDestroy()
    {
        ManagerUpdate.instance.update -= Execute;
    }
}
