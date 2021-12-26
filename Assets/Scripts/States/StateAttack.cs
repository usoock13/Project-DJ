using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : State {
    public StateAttack(string _name) : base(_name) {
        base.stateName = _name;
    }
}
