using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : LivingEntity 
{
    public Rigidbody2D playerRigidbody;
    public BoxCollider2D playerCollider;

    protected new float healthPoint = 100f;

    // 이동 관련 변수
    const float moveSpeed = 10f;
    float moveCoef = 1.2f;
    bool canMove = true;
    float jumpPower = 1500f;
    bool isGrounding = false;
    int maxAdditialJumpCount = 1;
    int currentAddtialJumpCount = 0;
    IEnumerator dodgeCoroutine;

    // 공격 관련 변수
    public Weapon equipedWeapon;
    float attackPower = 1f;
    float attackSpeed = 1.2f;
    IEnumerator attackCoroutine;
    IEnumerator jumpAttackCoroutine;
    IEnumerator jumpSpecialAttackCoroutine;
    int maxBasicCombo = 1;
    int currentBasicCombo = 0;
    bool reservationInput = false;
    
    // 상태 관련 변수
    StateMachine playerStateMachine;
    StateIdle playerStateIdle = new StateIdle("Idle");
    StateMove playerStateMove = new StateMove("Move");
    StateJump playerStateJump = new StateJump("Jump");
    StateDodge playerStateDodge = new StateDodge("Dodge");
    StateAttack playerStateAttack = new StateAttack("Attack");
    StateAttack playerStateAttackDelay = new StateAttack("Attack Delay");
    StateAttack playerStateJumpAttack = new StateAttack("Jump Attack");
    StateAttack playerStateSpecialAttack = new StateAttack("Special Attack");
    StateAttack playerStateJumpSpecialAttack = new StateAttack("Jump Special Attack");

    // 애니메이션 관련 변수
    Animator playerAnimator;
    AnimatorOverrideController playerAnimatorOverrideController;
    float originAnimationSpeed = 1f;

    new void Start() {
        base.Start();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        currentAddtialJumpCount = maxAdditialJumpCount;

        playerAnimator = transform.Find("Player Sprite").GetComponent<Animator>();
        playerAnimatorOverrideController = new AnimatorOverrideController(playerAnimator.runtimeAnimatorController);
        playerAnimator.runtimeAnimatorController = playerAnimatorOverrideController;

        playerStateMachine = new StateMachine(playerStateIdle);
        InitialStates();

        maxBasicCombo = equipedWeapon.basicStandsSize;
    }
    public void InitialStates() {
        playerStateMove.activeDelegate += () => {
            playerAnimator.speed = moveCoef;
            playerAnimator.SetBool("Move", true);
        };
        playerStateMove.inactiveDelegate += () => {
            playerAnimator.speed = originAnimationSpeed;
            playerAnimator.SetBool("Move", false);
        };
        playerStateJump.activeDelegate += () => { playerAnimator.SetBool("Jump", true); };
        playerStateJump.inactiveDelegate += () => { playerAnimator.SetBool("Jump", false); };
        playerStateDodge.activeDelegate += () => { 
            dodgeCoroutine = DodgeCoroutine();
            StartCoroutine(dodgeCoroutine);
            playerAnimator.speed = moveCoef;
            playerAnimator.SetBool("Dodge", true);
            canMove = false;
            playerRigidbody.AddForce(new Vector2(playerRigidbody.transform.localScale.x, 0) * moveSpeed * moveCoef * 100f, ForceMode2D.Force);
        };
        playerStateDodge.inactiveDelegate += () => { 
            playerAnimator.speed = originAnimationSpeed;
            StopCoroutine(dodgeCoroutine);
            playerAnimator.SetBool("Dodge", false);
            canMove = true; 
            playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
        };
        playerStateAttack.activeDelegate += () => {
            playerAnimator.Rebind();
            playerAnimator.speed = attackSpeed;
            playerAnimator.SetBool("Attack", true);
            canMove = false;
            attackCoroutine = BasicAttackCoroutine();
            playerAnimatorOverrideController["HeroKnight_Attack1"] = equipedWeapon.basicAttackStands[currentBasicCombo].animation;
            StartCoroutine(attackCoroutine);
        };
        playerStateAttack.inactiveDelegate += () => {
            canMove = true;
            playerAnimator.speed = originAnimationSpeed;
            StopCoroutine(attackCoroutine);
            playerAnimator.SetBool("Attack", false);
        };
        playerStateAttackDelay.activeDelegate += () => {
            playerAnimator.speed = attackSpeed;
            playerAnimator.SetBool("Attack", true);
            attackCoroutine = BasicAttackDelayCoroutine();
            StartCoroutine(attackCoroutine);
        };
        playerStateAttackDelay.inactiveDelegate += () => {
            playerAnimator.speed = originAnimationSpeed;
            playerAnimator.SetBool("Attack", false);
            StopCoroutine(attackCoroutine);
        };
        playerStateJumpAttack.activeDelegate += () => {
            playerAnimator.speed = attackSpeed;
            playerAnimator.SetBool("Jump Attack", true);
            jumpAttackCoroutine = JumpAttackCoroutine();
            StartCoroutine(jumpAttackCoroutine);
        };
        playerStateJumpAttack.inactiveDelegate += () => {
            playerAnimator.speed = originAnimationSpeed;
            playerAnimator.SetBool("Jump Attack", false);
            StopCoroutine(jumpAttackCoroutine);
        };
        playerStateSpecialAttack.activeDelegate += () => {
            playerAnimator.speed = attackSpeed;
            playerAnimator.SetBool("Special Attack", true);
        };
        playerStateSpecialAttack.inactiveDelegate += () => {
            playerAnimator.speed = originAnimationSpeed;
            playerAnimator.SetBool("Special Attack", false);
        };
        playerStateJumpSpecialAttack.activeDelegate += () => {
            playerAnimator.speed = attackSpeed;
            playerAnimator.SetBool("Jump Special Attack", true);
            jumpSpecialAttackCoroutine = JumpSpecialAttackCoroutine();
            StartCoroutine(jumpSpecialAttackCoroutine);
        };
        playerStateJumpSpecialAttack.inactiveDelegate += () => {
            canMove = true;
            playerAnimator.speed = originAnimationSpeed;
            playerAnimator.SetBool("Jump Special Attack", false);
            StopCoroutine(jumpSpecialAttackCoroutine);
        };
    }
    public void BasicMove(Vector2 moveDirection) {
        RaycastHit2D moveDirectionHit = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), .01f,LayerMask.GetMask("Ground"));
        if(!canMove) return;
        if(!moveDirectionHit)
            transform.Translate(moveDirection * moveSpeed * moveCoef * Time.deltaTime);
        if(playerStateMachine.currentState.GetStateName() != playerStateJumpAttack.GetStateName())
            transform.localScale = new Vector3(moveDirection.x, transform.localScale.y, 1);
        if(playerStateMachine.currentState.GetStateName() != playerStateJump.GetStateName()
        && playerStateMachine.currentState.GetStateName() != playerStateJumpAttack.GetStateName()
        && playerStateMachine.currentState.GetStateName() != playerStateJumpSpecialAttack.GetStateName())
            playerStateMachine.ChangeState(playerStateMove);
    }
    public void BasicMoveStop() {
        if(playerStateMachine.currentState.GetStateName() == playerStateMove.GetStateName()) {
            playerStateMachine.ChangeState(playerStateIdle);
        }
    }
    public void Jump() {
        // if(!canMove) return; // 공격 중 이동(점프, 구르기 포함) 가능 여부
        if(playerStateMachine.currentState.GetStateName() == playerStateJumpAttack.GetStateName()
        || playerStateMachine.currentState.GetStateName() == playerStateJumpSpecialAttack.GetStateName())
            return;
        if(isGrounding) {
            isGrounding = false;
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.AddForce(Vector2.up * jumpPower);
            playerStateMachine.ChangeState(playerStateJump);
        } else if (currentAddtialJumpCount > 0) {
            currentAddtialJumpCount -= 1;
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.AddForce(Vector2.up * jumpPower);
            playerStateMachine.ChangeState(playerStateJump);
        }
    }
    public void StopJump() {
        if(playerRigidbody.velocity.y > 0) {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y/2);
        }
    }
    public void DownJump() {
        // if(!canMove) return; // 공격 중 이동(점프, 구르기 포함) 가능 여부
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
        bool isface = false;
        if(groundHit) 
            isface = Mathf.Abs((groundHit.collider.transform.position.y + (groundHit.collider.transform.localScale.y/2)) - groundHit.point.y) < .009f;
        if(groundHit && playerRigidbody.velocity.y <= 0 && isface) {
            isGrounding = true;
            currentAddtialJumpCount = maxAdditialJumpCount;
            if(playerStateMachine.currentState.GetStateName() == playerStateJump.GetStateName()
            || playerStateMachine.currentState.GetStateName() == playerStateJumpAttack.GetStateName()
            ) {
                playerStateMachine.ChangeState(playerStateIdle);
            } else if (playerStateMachine.currentState.GetStateName() == playerStateJumpSpecialAttack.GetStateName()) {
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
                playerRigidbody.AddForce(new Vector2(0, 15f), ForceMode2D.Impulse);
                playerStateMachine.ChangeState(playerStateJump);
                equipedWeapon.WeaponJumpSpecialAttack(0);
            }
        } else {
            isGrounding = false;
            if(playerStateMachine.currentState.GetStateName() != playerStateJumpAttack.GetStateName()
            && playerStateMachine.currentState.GetStateName() != playerStateJumpSpecialAttack.GetStateName()) {
                playerStateMachine.ChangeState(playerStateJump);
            }
        }
    }
    public void Dodge() {
        if(playerStateMachine.currentState.GetStateName() != playerStateDodge.GetStateName()) {
            playerStateMachine.ChangeState(playerStateDodge);
        }
    }
    public IEnumerator DodgeCoroutine() {
        yield return new WaitForSeconds(.35f/moveCoef);
        playerStateMachine.ChangeState(playerStateIdle);
    }
    public void BasicAttack() {
        if(playerStateMachine.currentState.GetStateName() == playerStateIdle.GetStateName()
        || playerStateMachine.currentState.GetStateName() == playerStateMove.GetStateName()
        ) { // 평타 ================================================
            currentBasicCombo = 0;
            playerStateMachine.ChangeState(playerStateAttack);
        } else if(playerStateMachine.currentState.GetStateName() == playerStateAttackDelay.GetStateName()) {
            // 평타 후딜 입력 =======================================
            if(currentBasicCombo>=maxBasicCombo-1) return;
            currentBasicCombo++;
            playerStateMachine.ChangeState(playerStateAttack);
        } else if(playerStateMachine.currentState.GetStateName() == playerStateAttack.GetStateName()) {
            // 콤보 선입력 ==========================================
            reservationInput = true;
        } else if(playerStateMachine.currentState.GetStateName() == playerStateJump.GetStateName()) {
            // 점프 공격 ============================================
            playerStateMachine.ChangeState(playerStateJumpAttack);
        }
    }
    public IEnumerator BasicAttackCoroutine() {
        yield return new WaitForSeconds(equipedWeapon.basicAttackStands[currentBasicCombo].beforeDelay/attackSpeed);
        equipedWeapon.WeaponBasicAttack(currentBasicCombo);
        yield return new WaitForSeconds(equipedWeapon.basicAttackStands[currentBasicCombo].afterDelay/attackSpeed);
        playerStateMachine.ChangeState(playerStateAttackDelay);
    }
    public IEnumerator BasicAttackDelayCoroutine() {
        if(reservationInput) {
            reservationInput = false;
            BasicAttack();
        }
        yield return new WaitForSeconds(.23f/attackSpeed);
        playerStateMachine.ChangeState(playerStateIdle);
    }
    public IEnumerator JumpAttackCoroutine() {
        yield return new WaitForSeconds(equipedWeapon.jumpAttackStands[0].beforeDelay/attackSpeed);
        equipedWeapon.WeaponJumpAttack(0);
        yield return new WaitForSeconds(1f/attackSpeed);
    }
    public void SpecialAttack() {
        if(playerStateMachine.currentState.GetStateName() == playerStateIdle.GetStateName()
        || playerStateMachine.currentState.GetStateName() == playerStateMove.GetStateName()) {
            playerStateMachine.ChangeState(playerStateSpecialAttack);
        } else if(playerStateMachine.currentState.GetStateName() == playerStateJump.GetStateName()) {
            playerStateMachine.ChangeState(playerStateJumpSpecialAttack);
        }
    }
    public IEnumerator JumpSpecialAttackCoroutine() {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
        playerRigidbody.AddForce(new Vector2(0, 20f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(equipedWeapon.jumpSpecialAttackStands[0].beforeDelay);
        canMove = false;
        playerRigidbody.AddForce(new Vector2(0, -45f), ForceMode2D.Impulse);
    }
    void Update() {
        BottomGroundCheck();
    }
}