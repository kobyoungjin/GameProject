using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace FSM
{
    public class Player : MonoBehaviour
    {
        private InputManager inputManager;
        private Animator animator;
        private GameManager gameManager;
        private MouseManager mouseManager;

        private Vector3 destination;
        private RaycastHit click;
        private Rigidbody playerRigidbody;
               
        public StateMachine<Player> currentFSM;
        public BaseState<Player>[] arrState = new BaseState<Player>[(int)PlayerState.end];

        public PlayerState currentState;
        public PlayerState prevState;
        Weapon weapon;

        private float turnSpeed = 300f;
        private bool isMove;
        private bool isFireReady = false;

        float fireDelay;

        protected int exp;
        protected int gold;

        [SerializeField]
        protected int level;
        [SerializeField]
        protected int hp;
        [SerializeField]
        protected int maxHp;
        [SerializeField]
        protected int attackDamage;
        [SerializeField]
        protected int defense;
        [SerializeField]
        protected float moveSpeed;

        public int Level { get { return level; } set { level = value; } }
        public int Hp { get { return hp; } set { hp = value; } }
        public int MaxHp { get { return maxHp; } set { maxHp = value; } }
        public int AttackDamage { get { return attackDamage; } set { attackDamage = value; } }
        public int Defense { get { return defense; } set { defense = value; } }
        public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
        public int Exp { get { return exp; } set { exp = value; } }
        public int Gold { get { return gold; } set { gold = value; } }

        public Player()
        {
            Init();
        }

        void Awake()
        { 
            this.playerRigidbody = GetComponent<Rigidbody>();
        }          

        void Start()
        {
            level = 1;
            hp = 100;
            maxHp = 100;
            attackDamage = 5;
            defense = 5;
            moveSpeed = 4.0f;
            exp = 0;
            gold = 0;

            animator = GetComponent<Animator>();
            gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
            inputManager = gameManager.GetComponent<InputManager>();
            mouseManager = gameManager.GetComponent<MouseManager>();
            weapon = GameObject.FindGameObjectWithTag("Melee").GetComponent<Weapon>();

            //gameManager.SetText(this.gameObject);
            Enter();
        }

        void Update()
        {
            fireDelay += Time.deltaTime;

            Excute();
        }

        private void FixedUpdate()
        {
            PhysicsExcute();
        }

        // �ʱ�ȭ �Լ�
        public void Init()
        {
            currentFSM = new StateMachine<Player>();

            arrState[(int)PlayerState.idle] = new PlayerIdle(this);
            arrState[(int)PlayerState.combatIdle] = new PlayerCombatIdle(this);
            arrState[(int)PlayerState.running] = new PlayerRunning(this);
            arrState[(int)PlayerState.attack] = new PlayerAttack(this);
            arrState[(int)PlayerState.abilityAttack] = new PlayerAbilityAttack(this);

            currentFSM.SetState(arrState[(int)PlayerState.idle], this);
        }
       
        // ������Ʈ �ٲ��ִ� �Լ�
        public void ChangeState(PlayerState nextState)
        {
            currentFSM.ChangeState(arrState[(int)nextState]);
        }

        public void Enter()
        {
            currentFSM.Enter();
        }

        public void Excute()
        {
            currentFSM.Excute();
        }

        public void PhysicsExcute()
        {
            currentFSM.PhysicsExcute();
        }

        public void Exit()
        {
            currentFSM.Exit();
        }

        //public void TakeDamage(EnemySkeleton skeleton)
        //{
        //    if(hp <= 0)
        //    {
        //        ChangeState(PlayerState.die);
        //        return;
        //    }

        //    EnemySkeleton enemySkeleton = new EnemySkeleton
        //    {

        //    }
        //}

        public void Turn()
        {
            if (animator.GetInteger("attack") > 0) return;

            //RaycastHit hit;
            Vector3 targetPos = Vector3.zero;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out click, 10000f))
            {
                targetPos = click.point;
            }
            transform.LookAt(targetPos);
        }

        public void SetMousePoint()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out click))  // Ŭ���� ���� �����ɽ�Ʈ
            {
                SetPosition(click.point);
                if (click.collider.gameObject.layer == (int)Define.Layer.Monster || click.collider.gameObject.layer == (int)Define.Layer.NPC)
                {
                    GetMouseManager().SetMovePointer(false);
                }
                else
                {   
                    if(inputManager.GetIsPress() == false)
                        GetMouseManager().SetPos(click.point);

                    GetMouseManager().SetMovePointer(true);
                }

                float distance = Vector3.Distance(this.transform.position, destination);
                if (click.transform.gameObject.CompareTag("NPC") && distance <= 0.2f)
                {
                    return;
                }
            }
        }

        public void Attack()
        {
            if (weapon == null) return;

            
            isFireReady = weapon.rate < fireDelay;

            if(isFireReady)  // ���߿� ������ ���� �ɱ�
            {
                weapon.Use();
                fireDelay = 0;
            }
        }

        // ���콺 ��ǥ �����Լ�
        public void SetPosition(Vector3 pos)
        {
            destination = pos;
            isMove = true;
        }

        public Vector3 GetTargetPosition()
        {
            return destination;
        }

        public InputManager GetInput()
        {
            return inputManager;
        }
        public Animator GetAnimator()
        {
            return animator;
        }
        
        public void SetCurrentState(PlayerState newState)
        {
            currentState = newState;
        }

        public void SetPrevState(PlayerState newState)
        {
            prevState = newState;
        }

        public bool GetIsMove()
        {
            return isMove;
        }

        public void SetIsMove(bool isMove)
        {
            this.isMove = isMove;
        }

        public MouseManager GetMouseManager()
        {
            return mouseManager;
        }
    }        
}


