using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    public Rigidbody2D playerRigidbody;
    public BoxCollider2D playerCollider;
    // 이동 관련 변수
    float moveSpeed = 10f;
    float jumpPower = 1500f;
    bool isGrounding = false;
    int maxAdditialJumpCount = 1;
    int currentAddtialJumpCount = 0;
    
    // 상태 관련 변수
    StateMachine playerStateMachine;
    StateIdle playerStateIdle = new StateIdle("idle");
    StateMove playerStateMove = new StateMove("move");
    StateMove playerStateJump = new StateMove("jump");

    // 애니메이션 관련 변수
    Animator playerAnimator;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        currentAddtialJumpCount = maxAdditialJumpCount;
        playerAnimator = transform.Find("Player Sprite").GetComponent<Animator>();
        playerStateMachine = new StateMachine(playerStateIdle);
        InitialStates();
    }
    public void InitialStates() {
        playerStateMove.activeDelegate += () => { playerAnimator.SetBool("Move", true); };
        playerStateMove.inactiveDelegate += () => { playerAnimator.SetBool("Move", false); };
        playerStateJump.activeDelegate += () => { playerAnimator.SetBool("Jump", true); };
        playerStateJump.inactiveDelegate += () => { playerAnimator.SetBool("Jump", false); };
    }
    public void BasicMove(Vector2 moveDirection) {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        transform.localScale = new Vector3(-moveDirection.x, transform.localScale.y, 1);
        if(playerStateMachine.currentState.GetStateName() == playerStateIdle.GetStateName())
            playerStateMachine.ChangeState(playerStateMove);
    }
    public void BasicMoveStop() {
        if(playerStateMachine.currentState.GetStateName() == playerStateMove.GetStateName()) {
            playerStateMachine.ChangeState(playerStateIdle);
        }
    }
    public void Jump() {
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, .1f, LayerMask.GetMask("Platform"));
        if(hit) {
            Platform currentPlatform = hit.collider.GetComponent<Platform>();
            if(currentPlatform) currentPlatform.PassPlayer();
        }
    }
    public void BottomGroundCheck() {
        int layerMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Platform");
        RaycastHit2D groundHit = Physics2D.BoxCast(playerRigidbody.position + new Vector2(0, .05f), new Vector2(1, .1f), 0, Vector2.down, .07f, layerMask);
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
    public void BasicAttack() {
        
    }
    void Update() {
        BottomGroundCheck();
    }
}