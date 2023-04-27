using UnityEngine;

namespace UniBT.Examples.Scripts.Behavior
{
    public class LookAtAction : Action
    {
        [SerializeField]
        private bool useLerp = false;

        [SerializeField]
        private Transform target;

        private Transform transform;
        private Animator animator;

        public override void Awake()
        {
            transform = gameObject.transform;
            animator = gameObject.GetComponent<Animator>();
        }

        protected override Status OnUpdate()
        {
            if (useLerp)
            {
                var from = transform.position;
                var direction = target.position - from; 
                var goal = Quaternion.LookRotation(direction);  
                if (Quaternion.Angle(goal, transform.rotation) < 5.0f)  // �ޱ۰��� 5���� ������ ok
                {
                    return Status.Success;
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, goal, Time.deltaTime * 4); // ȸ��
                return Status.Running;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                transform.LookAt(target);  // Ÿ���� �Ĵٺ���
            }
                
            return Status.Running;

        }

    }
}