using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
    public class Status : Action
    {
        [SerializeField]
        protected int level;
        [SerializeField]
        protected int hp;
        [SerializeField]
        protected int maxHp;
        [SerializeField]
        protected int attack;
        [SerializeField]
        protected int defense;
        [SerializeField]
        protected float moveSpeed;

        public int Level { get { return level; } set { level = value; } }
        public int Hp { get { return hp; } set { hp = value; } }
        public int MaxHp { get { return maxHp; } set { maxHp = value; } }
        public int Attack { get { return attack; } set { attack = value; } }
        public int Defense { get { return defense; } set { defense = value; } }
        public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

        private void Start()
        {
            level = 1;
            hp = 100;
            maxHp = 100;
            attack = 5;
            defense = 5;
            moveSpeed = 4.0f;
        }
    }
}
