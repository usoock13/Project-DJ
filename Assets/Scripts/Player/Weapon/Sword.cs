using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {
    public List<AnimationClip> basicAttackAnimations;
    public List<AnimationClip> specialAttackAnimations;

    void Awake() {
        basicAttackStands.Add(new Stand(1, basicAttackAnimations[0], 1, 1));
        basicAttackStands.Add(new Stand(1, basicAttackAnimations[1], 0, 0));
    }
}
