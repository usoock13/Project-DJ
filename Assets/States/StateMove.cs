using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMove : State {
    public StateMove(string _name) : base(_name) {
        base.stateName = _name;
    }
}
