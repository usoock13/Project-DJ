using UnityEngine;

public struct Damage {
    public float point;
    public Vector2 force;
    public GameObject origin;
    public Damage(float _point, Vector2 _force, GameObject _origin) {
        point = _point;
        force = _force;
        origin = _origin;
    }
}
public struct DamageOverTime {}
public struct CroudControl {}