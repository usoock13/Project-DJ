using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDodge : State {
    public StateDodge(string _name) : base(_name) {
        base.stateName = _name;
    }
}
