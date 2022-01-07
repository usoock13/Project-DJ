using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {
    float weaponPower = 15f;
    float weaponForce = 1f;

    public Player player;

    public List<GameObject> basicAttackArea = new List<GameObject>();
    public List<GameObject> specialAttackArea = new List<GameObject>();
    public List<GameObject> jumpAttackArea = new List<GameObject>();
    public List<GameObject> jumpSpecialAttackArea = new List<GameObject>();

    void Awake() {
        basicAttackStands.Add(new Stand(20f, new Vector2(5f, 0), .15f, .25f, 1f, 1f));
        basicAttackStands.Add(new Stand(20f, new Vector2(5f, 0), .18f, .25f, 1f, 1f));
        jumpAttackStands.Add(new Stand(25f, new Vector2(5f, -1f), .19f, .45f, 0f, 0f));
        jumpSpecialAttackStands.Add(new Stand(35f, new Vector2(6f, 13f), .35f, 0, 0f, 0f));
    }
    void Start() {
        player ??= GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    protected override void DamageTarget(IDamagable target, Damage damage) {
        base.DamageTarget(target, damage);
    }
    public override void WeaponBasicAttack(int comboCount) {
        base.WeaponBasicAttack(comboCount);
        Transform area = basicAttackArea[comboCount].transform;
        Collider2D[] targets = Physics2D.OverlapBoxAll(area.position, area.localScale, area.rotation.z, 256);
        foreach(Collider2D target in targets) {
            LivingEntity targetLivingEntity = target.GetComponent<LivingEntity>();
            if(targetLivingEntity) {
                Stand stand = basicAttackStands[comboCount];
                Vector2 force = stand.forceCoef * new Vector2(targetLivingEntity.transform.position.x>player.transform.position.x ? 1 : -1, 1);
                Damage damage = new Damage(stand.damageCoef, force, player.gameObject);
                targetLivingEntity.OnDamage(damage);
            }
        }
    }
    public override void WeaponJumpAttack(int comboCount) {
        base.WeaponJumpAttack(comboCount);
        Transform area = jumpAttackArea[comboCount].transform;
        Collider2D[] targets = Physics2D.OverlapBoxAll(area.position, area.localScale, area.rotation.z, 256);
        foreach(Collider2D target in targets) {
            LivingEntity targetLivingEntity = target.GetComponent<LivingEntity>();
            if(targetLivingEntity) {
                Stand stand = jumpAttackStands[comboCount];
                Vector2 force = stand.forceCoef * new Vector2(targetLivingEntity.transform.position.x>player.transform.position.x ? 1 : -1, 1);
                Damage damage = new Damage(stand.damageCoef, force, player.gameObject);
                targetLivingEntity.OnDamage(damage);
            }
        }
    }
    public override void WeaponJumpSpecialAttack(int comboCount) {
        base.WeaponJumpSpecialAttack(comboCount);
        Transform area = jumpSpecialAttackArea[comboCount].transform;
        Collider2D[] targets = Physics2D.OverlapBoxAll(area.position, area.localScale, area.rotation.z, 256);
        foreach(Collider2D target in targets) {
            LivingEntity targetLivingEntity = target.GetComponent<LivingEntity>();
            if(targetLivingEntity) {
                Stand stand = jumpSpecialAttackStands[comboCount];
                Vector2 force = stand.forceCoef * new Vector2(targetLivingEntity.transform.position.x>player.transform.position.x ? 1 : -1, 1);
                Damage damage = new Damage(stand.damageCoef, force, player.gameObject);
                targetLivingEntity.OnDamage(damage);
            }
        }
    }
}
