using UnityEngine;

public interface IDamagable {
    void OnDamage(Damage damagePoint);
    void OnDestroy();
}