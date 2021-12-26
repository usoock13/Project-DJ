using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateActionDelay : State {
    public StateActionDelay(string _name) : base(_name) {
        base.stateName = _name;
    }
}
