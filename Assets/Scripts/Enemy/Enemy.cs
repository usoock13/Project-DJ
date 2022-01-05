using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : LivingEntity {
    SpriteRenderer enemySprite;
    Rigidbody2D enemyRigidbody;
    Animator enemyAniamtor;

    // 상태 관련 변수
    StateMachine enemyStatemachine;
    StateIdle enemyStateIdle = new StateIdle("Idle");
    StateMove enemyStateChasePlayer = new StateMove("Chase");
    StateHit enemyStateHit = new StateHit("Hit");

    // 건강 관련 변수
    IEnumerator hitEffectCoroutine;
    float hitTime = 0;

    public new void Start() {
        base.Start();
        enemySprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyAniamtor = transform.Find("Sprite").GetComponent<Animator>();
        
        enemyStatemachine = new StateMachine(enemyStateIdle);
        enemyStateHit.activeDelegate += () => {
            if(hitEffectCoroutine != null) 
                StopCoroutine(hitEffectCoroutine);
            hitEffectCoroutine = HitEffectCoroutine();
            StartCoroutine(hitEffectCoroutine);
        };
        enemyStateHit.inactiveDelegate += () => {
            StopCoroutine(hitEffectCoroutine);
            enemyAniamtor.SetBool("Hit", false);
        };
    }
    public override void OnDamage(Damage damage) {
        base.OnDamage(damage);
        hitTime += .3f;
        enemyAniamtor.Rebind();
        enemyAniamtor.SetBool("Hit", true);
        enemyRigidbody.AddForce(damage.force * new Vector2(transform.position.x>damage.origin.transform.position.x ? 1 : -1, 1), ForceMode2D.Impulse);
        enemyStatemachine.ChangeState(enemyStateHit);
    }
    public IEnumerator HitEffectCoroutine() {
        while(hitTime>0) {
            hitTime -= .02f;
            yield return new WaitForSeconds(.02f);
        }
        enemyStatemachine.ChangeState(enemyStateIdle);
        hitTime = 0;
    }
}