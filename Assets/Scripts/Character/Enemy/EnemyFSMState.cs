using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    public class EnemySkeletonPattrol : BaseState<EnemySkeleton>
    {
        private EnemySkeleton skeleton;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private List<Transform> wayPoints;

        public int nextPoint = 0;

        public EnemySkeletonPattrol(EnemySkeleton owner)
        {
            this.skeleton = owner;
        }

        public override void Enter()
        {
            Debug.Log("SkeletonPattrolEnter");

            animator = skeleton.GetAnimator();

            navMeshAgent = skeleton.transform.GetComponent<NavMeshAgent>();
            navMeshAgent.autoBraking = false;       // �յ��� �ӵ��� ����
            navMeshAgent.updateRotation = false;   //�ڵ����� ȸ���ϴ� ����� ��Ȱ��ȭ
            navMeshAgent.speed = skeleton.GetPattrollSpeed();
            
            wayPoints = skeleton.GetWayPoints();
            this.skeleton.SetCurrentState(EnemySkeletonState.pattroll);
            animator.SetBool("pattrol", true);
            skeleton.SetPattrolling(true);
            animator.SetBool("isMove", true);

            skeleton.MoveWayPoint();
        }

        public override void Excute()
        {
            Debug.Log("SkeletonPattrolExcute");

            float distance = Vector3.Distance(skeleton.GetPlayer().transform.position, skeleton.transform.position);
            Debug.Log((int)distance);
            animator.SetFloat("distance", distance);

            if (distance <= skeleton.GetAttackDistance())
                skeleton.ChangeState(EnemySkeletonState.attack);

            if (distance <= skeleton.GetTraceDistance())
                skeleton.ChangeState(EnemySkeletonState.trace);
        }

        public override void PhysicsExcute()
        {
            if (!skeleton.GetPattrolling())
                return;
            
            animator.SetFloat("speed", navMeshAgent.velocity.magnitude);
            skeleton.Move();
        }

        public override void Exit()
        {
            Debug.Log("SkeletonPattrolExit");

            //navMeshAgent.Stop();
            skeleton.SetPattrolling(false);
            animator.SetBool("pattrol", false);
        }
    }
    
    public class EnemySkeletonTrace : BaseState<EnemySkeleton>
    {
        private EnemySkeleton skeleton;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        public EnemySkeletonTrace(EnemySkeleton owner)
        {
            this.skeleton = owner;
        }

        public override void Enter()
        {
            Debug.Log("SkeletonTraceEnter");

            animator = skeleton.GetAnimator();
            navMeshAgent = skeleton.transform.GetComponent<NavMeshAgent>();
            navMeshAgent.autoBraking = false;       // �յ��� �ӵ��� ����
            navMeshAgent.updateRotation = false;   //�ڵ����� ȸ���ϴ� ����� ��Ȱ��ȭ
            navMeshAgent.speed = skeleton.GetPattrollSpeed();

            skeleton.SetCurrentState(EnemySkeletonState.trace);
            animator.SetBool("isInPlayer", true);
            
            skeleton.Tracing = skeleton.GetPlayer().transform.position;
            //skeleton.TraceTarget(skeleton.GetPlayer().transform.position);
            animator.SetBool("isMove", true);

            
        }

        public override void Excute()
        {
            Debug.Log("SkeletonTraceExcute");

            

            float distance = Vector3.Distance(skeleton.GetPlayer().transform.position, skeleton.transform.position);
            animator.SetFloat("distance", distance);
            if (distance <= skeleton.GetAttackDistance())
                skeleton.ChangeState(EnemySkeletonState.attack);

            if (distance > skeleton.GetTraceDistance())
                skeleton.ChangeState(EnemySkeletonState.pattroll);
        }

        public override void PhysicsExcute()
        {
            skeleton.Tracing = skeleton.GetPlayer().transform.position;

            if (navMeshAgent.isStopped == false)
            {
                Quaternion rot = Quaternion.LookRotation(navMeshAgent.desiredVelocity);
                Debug.Log(rot);
                skeleton.transform.rotation = Quaternion.Slerp(skeleton.transform.rotation, rot, Time.deltaTime * skeleton.GetDumping());
            }
        }

        public override void Exit()
        {
            Debug.Log("SkeletonTraceExit");
        }
    }

    public class EnemySkeletonAttack : BaseState<EnemySkeleton>
    {
        private EnemySkeleton skeleton;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        public EnemySkeletonAttack(EnemySkeleton owner)
        {
            this.skeleton = owner;
        }

        public override void Enter()
        {
            Debug.Log("SkeletonAttackEnter");

            animator = skeleton.GetAnimator();
            this.skeleton.SetCurrentState(EnemySkeletonState.attack);
            //nimator.SetBool("attack", true);
            animator.SetBool("isMove", false);
            skeleton.Stop();
        }

        public override void Excute()
        {
            Debug.Log("SkeletonAttackExcute");

            float distance = Vector3.Distance(skeleton.GetPlayer().transform.position, skeleton.transform.position);
            animator.SetFloat("distance", distance);

            // ������ �̵� �Է��� ������ ������ �ִϸ��̼� ���
            if (distance > skeleton.GetAttackDistance() && distance <= skeleton.GetTraceDistance() 
                && animator.GetCurrentAnimatorStateInfo(0).IsName("Skeleton@Attack01")
                    && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                skeleton.ChangeState(EnemySkeletonState.trace);

            if (distance > skeleton.GetTraceDistance() && animator.GetCurrentAnimatorStateInfo(0).IsName("Skeleton@Attack01")
                    && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                skeleton.ChangeState(EnemySkeletonState.pattroll);


            //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")    // �ִϸ������� State attackã�� 
            //        && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)  // �ִϸ��̼� ���������� ��ٸ���
            //{
            //    skeleton.ChangeState(EnemySkeletonState.pattroll);
            //}
        }

        public override void Exit()
        {
            Debug.Log("SkeletonAttackExit");
        }

        public override void PhysicsExcute()
        {

        }
    }
}