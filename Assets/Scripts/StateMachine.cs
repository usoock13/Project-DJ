using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {
    public State currentState { get; private set; }
    
    public StateMachine(State _initialState) {
        currentState = _initialState;
        currentState.ActiveStateHandler();
    }
    public void ChangeState(State _nextState) {
        if(_nextState.GetStateName() != currentState.GetStateName()) {
            currentState.InactiveStateHandler();
            currentState = _nextState;
            currentState.ActiveStateHandler();
        }
    }
}
