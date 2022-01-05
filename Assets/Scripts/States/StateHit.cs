using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHit : State {
    public StateHit(string _name) : base(_name) {
        base.stateName = _name;
    }
}
