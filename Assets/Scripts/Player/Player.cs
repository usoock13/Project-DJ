using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour 
{
    public Rigidbody2D playerRigidbody;
    public BoxCollider2D playerCollider;
    // 이동 관련 변수
    float moveSpeed = 10f;
    bool canMove = true;
    float jumpPower = 1500f;
    bool isGrounding = false;
    int maxAdditialJumpCount = 2;
    int currentAddtialJumpCount = 0;

    // 공격 관련 변수
    public Weapon equipmedWeapon;
    float attackPower = 1f;
    float attackSpeed = 2f;
    IEnumerator attackCoroutine;
    int maxBasicCombo = 3;
    int currentBasicCombo = 0;
    
    // 상태 관련 변수
    StateMachine playerStateMachine;
    StateIdle playerStateIdle = new StateIdle("Idle");
    StateMove playerStateMove = new StateMove("Move");
    StateJump playerStateJump = new StateJump("Jump");
    StateDodge playerStateDodge = new StateDodge("Dodge");
    StateAttack playerStateAttack = new StateAttack("Attack");
    StateAttack playerStateAttackDelay = new StateAttack("Attack Delay");

    // 애니메이션 관련 변수
    Animator playerAnimator;
    AnimatorOverrideController playerAnimatorOverrideController;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        currentAddtialJumpCount = maxAdditialJumpCount;

        playerAnimator = transform.Find("Player Sprite").GetComponent<Animator>();
        playerAnimatorOverrideController = new AnimatorOverrideController(playerAnimator.runtimeAnimatorController);
        playerAnimator.runtimeAnimatorController = playerAnimatorOverrideController;
        // playerAnimatorOverrideController["PlayerBasicAttack"] = equipmedWeapon.basicAttackStands[1].animation;

        playerStateMachine = new StateMachine(playerStateIdle);
        InitialStates();
    }
    public void InitialStates() {
        playerStateMove.activeDelegate += () => { playerAnimator.SetBool("Move", true); };
        playerStateMove.inactiveDelegate += () => { playerAnimator.SetBool("Move", false); };
        playerStateJump.activeDelegate += () => { playerAnimator.SetBool("Jump", true); };
        playerStateJump.inactiveDelegate += () => { playerAnimator.SetBool("Jump", false); };
        playerStateDodge.activeDelegate += () => { 
            playerAnimator.SetBool("Dodge", true);
            canMove = false;
            playerRigidbody.AddForce(new Vector2(playerRigidbody.transform.localScale.x, 0) * moveSpeed * 2.8f, ForceMode2D.Impulse);
        };
        playerStateDodge.inactiveDelegate += () => { 
            playerAnimator.SetBool("Dodge", false);
            canMove = true;
            playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
        };
        playerStateAttack.activeDelegate += () => {
            playerAnimator.SetBool("Attack", true);
            canMove = false;
            attackCoroutine = BasicAttackCoroutine();
            StartCoroutine(attackCoroutine);
        };
        playerStateAttack.inactiveDelegate += () => {
            playerAnimator.SetBool("Attack", false);
            canMove = true;
            StopCoroutine(attackCoroutine);
        };
        playerStateAttackDelay.activeDelegate += () => {
            playerAnimator.SetBool("Attack", true);
            attackCoroutine = BasicAttackDelayCoroutine();
            StartCoroutine(attackCoroutine);
        };
        playerStateAttackDelay.inactiveDelegate += () => {
            playerAnimator.SetBool("Attack", false);
            StopCoroutine(attackCoroutine);
        };
    }
    public void BasicMove(Vector2 moveDirection) {
        RaycastHit2D moveDirectionHit = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), .01f,LayerMask.GetMask("Ground"));
        if(!canMove) return;
        if(!moveDirectionHit) transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        transform.localScale = new Vector3(moveDirection.x, transform.localScale.y, 1);
        if(playerStateMachine.currentState.GetStateName() != playerStateJump.GetStateName())
            playerStateMachine.ChangeState(playerStateMove);
    }
    public void BasicMoveStop() {
        if(playerStateMachine.currentState.GetStateName() == playerStateMove.GetStateName()) {
            playerStateMachine.ChangeState(playerStateIdle);
        }
    }
    public void Jump() {
        if(!canMove) return;
        if(isGrounding) {
            isGrounding = false;
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.AddForce(Vector2.up * jumpPower);
        } else if (currentAddtialJumpCount > 0) {
            currentAddtialJumpCount -= 1;
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.AddForce(Vector2.up * jumpPower);
        }
    }
    public void StopJump() {
        if(playerRigidbody.velocity.y > 0) {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y/2);
        }
    }
    public void DownJump() {
        if(!canMove) return;
        Vector2 rayStartPosition = playerRigidbody.position - (new Vector2(0, playerCollider.bounds.size.y/2-.05f));
        RaycastHit2D hit = Physics2D.BoxCast(rayStartPosition, new Vector2(1, .1f), 0, Vector2.down, .1f, LayerMask.GetMask("Platform"));
        if(hit) {
            Platform currentPlatform = hit.collider.GetComponent<Platform>();
            if(currentPlatform) currentPlatform.PassPlayer();
        }
    }
    public void BottomGroundCheck() {
        if(playerStateMachine.currentState.GetStateName() == playerStateDodge.GetStateName()) return;
        Vector2 rayStartPosition = playerRigidbody.position - (new Vector2(0, playerCollider.bounds.size.y/2-.05f));
        int layerMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Platform");
        RaycastHit2D groundHit = Physics2D.BoxCast(rayStartPosition, new Vector2(1, .1f), 0, Vector2.down, .07f, layerMask);
        bool isfase = false;
        if(groundHit) 
            isfase = Mathf.Abs((groundHit.collider.transform.position.y + (groundHit.collider.transform.localScale.y/2)) - groundHit.point.y) < .009f;
        if(groundHit && playerRigidbody.velocity.y <= 0 && isfase) {
            isGrounding = true;
            currentAddtialJumpCount = maxAdditialJumpCount;
            if(playerStateMachine.currentState.GetStateName() == playerStateJump.GetStateName()) 
                playerStateMachine.ChangeState(playerStateIdle);
        } else {
            isGrounding = false;
            playerStateMachine.ChangeState(playerStateJump);
        }
    }
    public void Dodge() {
        if(playerStateMachine.currentState.GetStateName() != playerStateDodge.GetStateName()
            && playerStateMachine.currentState.GetStateName() != playerStateJump.GetStateName()) {
            StartCoroutine(DodgeCoroutine());
        }
    }
    public IEnumerator DodgeCoroutine() {
        playerStateMachine.ChangeState(playerStateDodge);
        yield return new WaitForSeconds(.35f);
        playerStateMachine.ChangeState(playerStateIdle);
    }
    public void BasicAttack() {
        if((playerStateMachine.currentState.GetStateName() == playerStateIdle.GetStateName()
        || playerStateMachine.currentState.GetStateName() == playerStateMove.GetStateName())
        && currentBasicCombo < maxBasicCombo
        ){
            if(playerStateMachine.currentState.GetStateName() == playerStateAttackDelay.GetStateName()) {
                if(currentBasicCombo>=maxBasicCombo) return;
                currentBasicCombo++;
                playerStateMachine.ChangeState(playerStateAttack);
            } else {
                currentBasicCombo = 1;
                playerStateMachine.ChangeState(playerStateAttack);
            }
        }
    }
    public IEnumerator BasicAttackCoroutine() {
        yield return new WaitForSeconds(1/attackSpeed);
        playerStateMachine.ChangeState(playerStateAttackDelay);
    }
    public IEnumerator BasicAttackDelayCoroutine() {
        yield return new WaitForSeconds(.7f/attackSpeed);
        playerStateMachine.ChangeState(playerStateIdle);
    }
    void Update() {
        BottomGroundCheck();
    }
}