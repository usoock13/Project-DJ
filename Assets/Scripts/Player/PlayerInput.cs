using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour 
{
    Player player;
    void Start() {
        player = GetComponent<Player>();
    }
    void InputMove() {
        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        if(moveDirection != Vector2.zero) {
            player.BasicMove(moveDirection);
        }
    }
    void OutputMove() {
        if(Input.GetButtonUp("Horizontal")) {
            player.BasicMoveStop();
        }
    }
    void InputJump() {
        if(Input.GetButtonDown("Jump")) {
            player.Jump();
        }
    }
    void OutputJump() {
        if(Input.GetButtonUp("Jump")) {
            player.StopJump();
        }
    }
    void InputDownJump() {
        if(Input.GetButtonDown("Vertical")) {
            player.DownJump();
        }
    }
    void InputAttack() {
        if(Input.GetButtonDown("Fire1")) {
            player.BasicAttack();
        }
    }
    void InputSpecialAttack() {
        if(Input.GetButtonDown("Fire2")) {
            player.SpecialAttack();
        }
    }
    void InputDodge() {
        if(Input.GetButtonDown("Dodge")) {
            player.Dodge();
        }
    }
    void Update() {
        InputMove();
        InputJump();
        InputDownJump();
        InputAttack();
        InputSpecialAttack();
        InputDodge();
        OutputJump();
        OutputMove();
    }
}