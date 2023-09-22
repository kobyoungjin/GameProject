using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Attack target.")]
    public class Attack : Action
    {
        [Tooltip("target Power")]
        public SharedInt force;
        [Tooltip("target animator")]
        public Animator animator;
        [Tooltip("The transform that the agent is moving towards")]
        public SharedGameObject enemy;

        public SharedString animationName;
        private bool isFireReady = false;
        Weapon weapon;
        float fireDelay;

        private bool triggered = false;

        public override void OnAwake()
        {
            animator = gameObject.GetComponent<Animator>();
        }

        public override void OnStart()
        {
            weapon = enemy.Value.transform.Find("troll/base/hips/spine/chest/upper_arm.R/forearm.R/hand.R/weapon/weapon_end").GetComponent<Weapon>();
            //Debug.Log(weapon.name);
        }

        public override TaskStatus OnUpdate()
        {
            if (weapon == null) return TaskStatus.Failure;
            fireDelay += Time.deltaTime;

            isFireReady = weapon.rate < fireDelay;

            if (isFireReady)  // ���߿� ������ ���� �ɱ�
            {
                weapon.Use();
                fireDelay = 0;
                enemy.Value.GetComponent<NavMeshAgent>().isStopped = false;
                return TaskStatus.Success;
            }
            enemy.Value.GetComponent<NavMeshAgent>().isStopped = true;

            return TaskStatus.Running;
        }

        public void CancelAttack()
        {
            enemy.Value.GetComponent<NavMeshAgent>().enabled = true;
            //rigid.isKinematic = true;
            animator.SetBool(animationName.Value, false);
        }

        public void OnAttack(int force)
        {
            enemy.Value.GetComponent<NavMeshAgent>().enabled = false;
            //animator.SetBool("Attacking", true);
            //rigid.isKinematic = false;
            //rigid.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    }
}
