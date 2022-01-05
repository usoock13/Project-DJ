using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageEntity : MonoBehaviour {
    bool areaDebug = false;
    protected virtual void DamageTarget(IDamagable target, Damage damage) {
        target.OnDamage(damage);
    }
}