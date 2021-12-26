using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : State {
    public StateIdle(string _name) : base(_name) {
        base.stateName = _name;
    }
}
