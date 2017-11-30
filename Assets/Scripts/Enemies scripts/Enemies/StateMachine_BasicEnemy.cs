using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FP;
using System;

[RequireComponent(typeof(EnemyBasic))]
public class StateMachine_BasicEnemy : MonoBehaviour {

    public enum EnemyBasicAction { Null, Pursuit, Attack, Evade }
    EventFSM<EnemyBasicAction> _fsm;
    private EnemyBasic _entity;

    bool evadeOver;

    void Start () {
        _entity = GetComponent<EnemyBasic>();
        ManagerUpdate.instance.update += Execute;

        var Chasing = new State<EnemyBasicAction>("Chasing");
        var Evading = new State<EnemyBasicAction>("Evading");
        var Attacking = new State<EnemyBasicAction>("Attacking");


        Chasing
            .SetTransition(EnemyBasicAction.Attack, Attacking)
            .SetTransition(EnemyBasicAction.Evade, Evading)
            ;
        Evading
            .SetTransition(EnemyBasicAction.Pursuit, Chasing)
            ;
        Attacking
            .SetTransition(EnemyBasicAction.Pursuit, Chasing)
            .SetTransition(EnemyBasicAction.Evade, Evading)
            ;

        //Pursuit fsm.
        Chasing.OnEnter += () =>{
            _entity.SetTarget(_entity.boss.transform);
            _entity._ChangeMoving("MoveSeek");
        };

        Chasing.OnUpdate += () => {
            _entity._Moving(_entity.boss.transform);
        };

        //Evade fsm.
        Evading.OnEnter += () => {
            StartCoroutine(evadeTime());
            _entity.SetTarget(_entity.hero.transform);
            _entity._ChangeMoving("MoveFlee");
        };

        Evading.OnUpdate += () =>{
            _entity._Moving(_entity.target);
        };

        //Attack fsm.
        Attacking.OnEnter += () =>{
            _entity.SetTarget(_entity.hero.transform);
        };

        Attacking.OnUpdate += () => {
            _entity._Attaking();
        };

        _fsm = new EventFSM<EnemyBasicAction>(Chasing);
    }

    //First time evade.
    private IEnumerator evadeTime()
    {
        while (_entity.boss == null)
        {
            evadeOver = false;
            yield return new WaitForSeconds(_entity.researchTime);
            _entity._StartSearching();
        }
        evadeOver = true;
    }

    private void Execute()
    {
        if (_entity.boss != null)
        {
            if (_entity.boss.isAttacking)
                _fsm.Feed(EnemyBasicAction.Attack);
            else
                _fsm.Feed(EnemyBasicAction.Pursuit);
        }
        else
            _fsm.Feed(EnemyBasicAction.Evade);

        _fsm.Update();


        if (Vector3.Distance(transform.position, _entity.target.position) > 100)
            Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        ManagerUpdate.instance.update -= Execute;
        Destroy(_entity);
    }
}
