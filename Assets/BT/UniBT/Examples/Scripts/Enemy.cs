using UnityEngine;
using UnityEngine.AI;

namespace UniBT.Examples.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public enum STEP
        {      // ���¸� ��Ÿ���� ����ü.
            NONE = -1,          // ���� ���� ����.
            PATROLL = 0,       // ���� ����
            TRACE,              // �߰� ���� 
            ATTACK,             // ���� ����
        }

        public bool Attacking { get; private set; }

        [SerializeField]
        private Transform player;
        
        private Rigidbody rigid;
        
        private NavMeshAgent navMeshAgent;

        public float Distance { get; private set; }
        public STEP step = STEP.NONE;       // ���� ����.

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            this.step = STEP.NONE;          // �� �ܰ� ���¸� �ʱ�ȭ.
            navMeshAgent.isStopped = false;
        }

        private void FixedUpdate()
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }

        private void Update()
        {
            Distance = Vector3.Distance(transform.position, player.position);
            //Debug.Log("�Ÿ�: " + (int)Distance);
            Debug.Log("����: " + step.ToString());
            Debug.Log(Attacking);

            if (Distance <= 1.5f) step = STEP.ATTACK;
            else if (Distance <= 10.0f)
            {
                step = STEP.TRACE;
                CancelAttack();
            }
            else step = STEP.PATROLL;
        }

        public void Attack(float force)
        {
            Attacking = true;
            navMeshAgent.enabled = false;
            //rigid.isKinematic = false;
            rigid.AddForce(Vector3.up * force, ForceMode.Impulse);

        }

        private void OnCollisionStay(Collision other)
        {
            // TODO other.collider.name cause GC.Alloc by Object.GetName
            if (Attacking && other.collider.name == "Ground" && Mathf.Abs(rigid.velocity.y) < 0.1)
            {
                CancelAttack();
            }
        }

        public void CancelAttack()
        {
            navMeshAgent.enabled = true;
            //rigid.isKinematic = true;
            Attacking = false;
        }
        
        public Transform GetPlayerPos()
        {
            return player;
        }
    }
}