using System.Collections;
using System.Collections.Generic;

public abstract class State {
    protected string stateName;
    public delegate void VoidDelegate();
    public VoidDelegate activeDelegate;
    public VoidDelegate inactiveDelegate;
    public State(string _name) {
        stateName = _name;
    }
    public virtual void ActiveStateHandler() {
        if(activeDelegate != null) activeDelegate();
    }
    public virtual void InactiveStateHandler() {
        if(inactiveDelegate != null) inactiveDelegate();
    }
    public string GetStateName() {
        return stateName;
    }
}