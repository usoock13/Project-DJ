using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : State {
    public StateJump(string _name) : base(_name) {
        base.stateName = _name;
    }
}
