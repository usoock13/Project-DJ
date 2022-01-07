using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public abstract class Weapon : DamageEntity {
    public enum WeaponType {
        Melee,
        Range
    }
    public float attackPower;
    public float attackSpeed;
    int attackStandsSize;
    public AnimatorController weaponAnimatorController;
    public WeaponType weaponType;
    public List<Stand> basicAttackStands { get; private set; } = new List<Stand>();
    public List<Stand> specialAttackStands { get; private set; } = new List<Stand>();
    public List<Stand> jumpAttackStands { get; private set; } = new List<Stand>();
    public List<Stand> jumpSpecialAttackStands { get; private set; } = new List<Stand>();
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
        public Vector2 forceCoef;
        public float beforeDelay;
        public float afterDelay;
        public float moveCoef;
        public float minMove;
        
        public Stand(float _damageCoef, Vector2 _forceCoef, float _beforeDelay, float _afterDelay, float _moveCoef = 1, float _minMove = 0) {
            damageCoef = _damageCoef;
            forceCoef = _forceCoef;
            beforeDelay = _beforeDelay;
            afterDelay = _afterDelay;
            moveCoef = _moveCoef;
            minMove = _minMove;
        }
    }
    public virtual void WeaponBasicAttack(int comboCount) {}
    public virtual void WeaponSpecialBasicAttack(int comboCount) {}
    public virtual void WeaponJumpAttack(int comboCount) {}
    public virtual void WeaponJumpSpecialAttack(int comboCount) {}
}