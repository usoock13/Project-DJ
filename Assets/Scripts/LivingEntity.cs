using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : IDamagable {
    bool isDead = false;
    public void OnDamage() {
        
    }
    public void OnDestroy() {

    }
    public void Die() {
        OnDestroy();
    }
}