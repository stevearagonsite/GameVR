﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FP{
    public class EventFSM<T>{
        public readonly State<T> any;
        State<T> _current;
        bool _feeding = false;

        public EventFSM(State<T> initial, State<T> any = null){
            _current = initial;
            _current._Enter();
            this.any = (any != null) ? any : new State<T>("<any>");
            this.any.OnEnter += () => { throw new Exception("Can't make transition to fsm's <any> state"); };
        }

        public void Update(){
            _current._Update();
        }

        public bool Feed(T input){
            if (_feeding)
                throw new Exception("Error: Feeding from OnEnter or OnExit, will cause repeated or missing calls");

            State<T>.Transition transition;
            if(
                _current._tryGetTransition(input, out transition) 
                || any._tryGetTransition(input, out transition)
                ){
                _feeding = true;            //Not multi-thread safe...

                _current._Exit();
                #if FP_VERBOSE
				Debug.Log("FSM state: " + _current.name + "---" + input + "---> " + transition.targetState.name);
                #endif
                transition._Transition();
                _current = transition.targetState;
                _current._Enter();

                _feeding = false;

                return true;
            }
            return false;
        }
    }
}
