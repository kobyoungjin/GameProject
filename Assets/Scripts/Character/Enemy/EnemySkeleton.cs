using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    public class EnemySkeleton : MonoBehaviour
    { 
        float speed = 2;
        Vector3 destination;
        Vector3 startPos;

        SphereCollider sphereCollider;

        private GameObject player;
        private Animator animator;
        private float maxDistanceToCheck = 6.0f;
        private float currentDistance;

        public StateMachine<EnemySkeleton> newState;
        public BaseState<EnemySkeleton>[] arrState = new BaseState<EnemySkeleton>[(int)EnemySkeletonState.end];

        public EnemySkeletonState currentState;
        public EnemySkeletonState prevState;

        // Patrol 
        public NavMeshAgent navMeshAgent;
        public List<Transform> wayPoints;
        public int nextPoint = 0;

        private Transform playerTrans;
        private Transform enemyTrans;

        private float attackDistance = 1.0f;
        private float traceDistance = 8.0f;

        public bool isDie = false;
        private WaitForSeconds ws;

        // ���� ���� �Ǵ� ����
        private readonly float pattrollSpeed = 2.0f;
        private readonly float traceSpeed = 2.2f;
        private float damping = 1.0f;

        private bool isPatrolling;
        // patrolling ������Ƽ ����
        public bool Patrolling
        {
            get { return isPatrolling; }
            set
            {
                isPatrolling = value;
                if (isPatrolling)
                {
                    navMeshAgent.speed = pattrollSpeed;
                    // ���� ������ ȸ�����
                    damping = 1.5f;
                    MoveWayPoint();
                }
            }
        }

        //���� ����� ��ġ�� �����ϴ� ����
        private Vector3 traceTarget;
        public Vector3 Tracing
        {
            get { return traceTarget; }
            set
            {
                traceTarget = value;

                float distance = Vector3.Distance(traceTarget, GetStartPos());
                Debug.Log(distance);
                if (distance > 15.0f && !Patrolling)
                {
                    TraceTarget(startPos);
                    navMeshAgent.speed = traceSpeed * 2.0f;
                    return;
                }
                navMeshAgent.speed = traceSpeed;
                // ���� ������ ȸ�����
                damping = 7.0f;
                TraceTarget(traceTarget);
            }
        }

        // hp
        private readonly float hp = 100.0f;

        public float Speed
        {
            get { return navMeshAgent.velocity.magnitude; }
        }

        public EnemySkeleton()
        {
            Init();
        }

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            animator = gameObject.GetComponent<Animator>();

            if (player != null)
                playerTrans = player.GetComponent<Transform>();

            enemyTrans = GetComponent<Transform>();
            ws = new WaitForSeconds(0.3f);
        }

        void Start()
        {
            startPos = this.transform.position;
            sphereCollider = this.GetComponent<SphereCollider>();

            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.autoBraking = false;       // �յ��� �ӵ��� ����
            navMeshAgent.updateRotation = false;   //�ڵ����� ȸ���ϴ� ����� ��Ȱ��ȭ
            navMeshAgent.speed = pattrollSpeed;

            GameObject wayPointsGroup = GameObject.Find("WayPointTransforms");

            if (wayPointsGroup != null)
            {
                wayPointsGroup.GetComponentsInChildren<Transform>(wayPoints);
                wayPoints.RemoveAt(0);
            }
            
            Enter();
        }

        void Update()
        {
            Excute();         
        }

        private void FixedUpdate()
        {
            PhysicsExcute();
        }

        public void Init()
        {
            newState = new StateMachine<EnemySkeleton>();

            arrState[(int)EnemySkeletonState.pattroll] = new EnemySkeletonPattrol(this);
            arrState[(int)EnemySkeletonState.trace] = new EnemySkeletonTrace(this);
            arrState[(int)EnemySkeletonState.attack] = new EnemySkeletonAttack(this);

            newState.SetState(arrState[(int)EnemySkeletonState.pattroll], this);
        }

        public NavMeshAgent navInit(NavMeshAgent nav)
        {
            nav = transform.GetComponent<NavMeshAgent>();
            nav.autoBraking = false;       // �յ��� �ӵ��� ����
            nav.updateRotation = false;   //�ڵ����� ȸ���ϴ� ����� ��Ȱ��ȭ
            nav.speed = GetPattrollSpeed();

            return nav;
        }

        public void ChangeState(EnemySkeletonState skeletonState)
        {
            for (int i = 0; i < (int)EnemySkeletonState.end; ++i)
            {
                if (i == (int)skeletonState)
                    newState.ChangeState(arrState[(int)skeletonState]);
            }
        }

        public void Enter()
        {
            newState.Enter();
        }

        public void Excute()
        {
            newState.Excute();
        }

        public void PhysicsExcute()
        {
            newState.PhysicsExcute();
        }

        public void Exit()
        {
            newState.Exit();
        }

        private void TraceTarget(Vector3 pos)
        {
            if (navMeshAgent.isPathStale)
                return;

            navMeshAgent.destination = pos;
            navMeshAgent.isStopped = false;
        }

        public void LookingForward()
        {
            if (navMeshAgent.isStopped == false)
            {
                Quaternion rot = Quaternion.LookRotation(navMeshAgent.desiredVelocity);       
                enemyTrans.rotation = Quaternion.Slerp(enemyTrans.rotation, rot, Time.deltaTime * damping);
            }
        }

        public void CheckToPoint()
        {
            if (!isPatrolling)
                return;

            // �̵��� �������� �����ߴ��� ���� ���
            if (navMeshAgent.velocity.sqrMagnitude >= 0.2f * 0.2f
                && navMeshAgent.remainingDistance <= 0.5f)
            {
                ++nextPoint;
                nextPoint = nextPoint % wayPoints.Count;  // ���� ������ �迭 ���

                MoveWayPoint();
            }
        }

        public void MoveWayPoint()
        {
            if (navMeshAgent.isPathStale)   // �ִܰŸ� ��� ����� ������ �ʾ����� ������ �������� ����
                return;

            navMeshAgent.destination = wayPoints[nextPoint].position;
            navMeshAgent.isStopped = false;
        }

        public void Stop()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            isPatrolling = false;
        }

        public float GetDistance()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        public List<Transform> GetWayPoints()
        {
            return wayPoints;
        }

        public bool GetPattrolling()
        {
            return isPatrolling;
        }

        public void SetPattrolling(bool isAct)
        {
            isPatrolling = isAct;
        }

        public void SetCurrentState(EnemySkeletonState newState)
        {
            currentState = newState;
        }

        public void SetPrevState(EnemySkeletonState newState)
        {
            prevState = newState;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        public Vector3 GetPosition()
        {
            return destination;
        }

        public void SetDestinationPos(Vector3 pos)
        {
            destination = pos;
        }

        public Vector3 GetDestinationPos()
        {
            return destination;
        }

        public float GetSpeed()
        {
            return navMeshAgent.velocity.magnitude;
        }

        public float GetDumping()
        {
            return damping;
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        public float GetPattrollSpeed()
        {
            return pattrollSpeed;
        }

        public float GetAttackDistance()
        {
            return attackDistance;
        }

        public float GetTraceDistance()
        {
            //return traceDistance;
            return GetComponent<SphereCollider>().radius;
        }

        public Vector3 GetStartPos()
        {
            return startPos;
        }

        public GameObject GetPlayer()
        {
            return player;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                animator.SetBool("isInPlayer", true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                animator.SetBool("isInPlayer", false);
            }
        }
    }
}

