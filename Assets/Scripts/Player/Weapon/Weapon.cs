using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
    public enum WeaponType {
        Melee,
        Range
    }
    public float attackPower;
    public float attackSpeed;
    int attackStandsSize;
    public WeaponType weaponType;
    public List<Stand> basicAttackStands { get; private set; } = new List<Stand>();
    public List<Stand> specialAttackStands { get; private set; } = new List<Stand>();
    public List<Stand> jumpAttackStands { get; private set; } = new List<Stand>();
    public int basicStandsSize {
        get { return basicAttackStands.Count; }
    }
    public int specialStandsSize {
        get { return basicAttackStands.Count; }
    }
    public int jumpStandsSize {
        get { return basicAttackStands.Count; }
    }
    public struct Stand {
        public float damageCoef;
        public AnimationClip animation;
        public float moveCoef;
        public float minMove;
        
        public Stand(float _damageCoef, AnimationClip _anim, float _moveCoef = 1, float _minMove = 0) {
            damageCoef = _damageCoef;
            animation = _anim;
            moveCoef = _moveCoef;
            minMove = _minMove;
        }
    }
}