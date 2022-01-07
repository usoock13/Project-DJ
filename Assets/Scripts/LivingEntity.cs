using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamagable {
    public float healthPoint;
    bool isDead = false;
    bool isFixedEntity = false;
    protected void Start() {
        
    }
    public virtual void OnDamage(Damage damage) {
        healthPoint -= damage.point;
        if(healthPoint<0) {
            healthPoint = 0;
            Die();
        }
    }
    public virtual void Die() {
        isDead = true;
        OnDestroy();
    }
    public virtual void OnDestroy() {}
}