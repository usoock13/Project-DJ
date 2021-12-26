using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {
    public List<AnimationClip> basicAttackAnimations;
    public List<AnimationClip> specialAttackAnimations;

    void Start() {
        // basicAttackStands.Add(new Stand(1, basicAttackAnimations[0], 1, 0));
        // print(basicAttackAnimations[0]);
    }
}
