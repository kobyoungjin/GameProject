using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class PlayerIdle : BaseState<Player>
    {
        private Player player;
        private Animator animator;

        public PlayerIdle(Player owner)
        {
            this.player = owner;
        }

        public override void Enter()
        {
            //Debug.Log("PlayerIdleEnter");

            animator = player.GetAnimator();
            this.player.SetCurrentState(PlayerState.idle);
            animator.SetBool("idle", true);
            animator.SetBool("isIdle", true);
        }

        public override void Excute()
        {
            //Debug.Log("PlayerIdleExcute");
            //Debug.Log(player.GetInput().MoveInput);
            RaycastHit click;
            if (player.GetInput().MoveInput)
            {
                if (Physics.Raycast(player.GetMainCamera().ScreenPointToRay(Input.mousePosition), out click))  // Ŭ���� ���� �����ɽ�Ʈ
                {
                    player.SetPosition(click.point);

                    float distance = player.GetDistance(click.point);
                    if (click.transform.gameObject.CompareTag("NPC") && distance <= 0.1f)
                    {
                        return;
                    }

                    player.ChangeState(PlayerState.running);
                }
                return;

            }

            if (player.GetInput().AttackInput)
            {
                Vector3 mPosition = Input.mousePosition;
                Vector3 oPosition = player.transform.position;

                mPosition.z = oPosition.z - Camera.main.transform.position.z;
                Vector3 target = Camera.main.ScreenToWorldPoint(mPosition);
                float dz = target.z - oPosition.z;
                float dx = target.x - oPosition.x;
                float rotateDegree = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg;
                player.transform.rotation = Quaternion.Euler(0f, rotateDegree, 0f);

                player.ChangeState(PlayerState.attack);
                return;
            }

            if (player.GetInput().KeyCodeQ)
            {
                player.ChangeState(PlayerState.abilityAttack);
                return;
            }
        }

        public override void PhysicsExcute()
        {
            
            return;
        }

        public override void Exit()
        {
            //Debug.Log("PlayerIdleExit");
            player.SetPrevState(PlayerState.idle);
            animator.SetBool("idle", false);
            animator.SetBool("isIdle", false);
        }
    }

    public class PlayerCombatIdle : BaseState<Player>
    {
        private Player player;
        private Animator animator;
        private float time = 0;
        public PlayerCombatIdle(Player owner)
        {
            this.player = owner;
        }

        public override void Enter()
        {
            //Debug.Log("PlayerCombatIdleEnter");

            animator = player.GetAnimator();
            this.player.SetCurrentState(PlayerState.combatIdle);
            animator.SetBool("combatIdle", true);
        }

        public override void Excute()
        {
            //Debug.Log("PlayerCombatIdleExcute");
            
            if (player.GetInput().MoveInput)
            {
                RaycastHit click;
                // Ŭ�����ϸ� ��ġ ����
                if (Physics.Raycast(player.GetMainCamera().ScreenPointToRay(Input.mousePosition), out click))  // Ŭ���� ���� �����ɽ�Ʈ
                {
                    if (click.transform.gameObject.CompareTag("NPC"))
                    {
                        return;
                    }

                    player.SetPosition(click.point);
                    player.ChangeState(PlayerState.running);
                }
                return;
            }

            // ����
            if (player.GetInput().AttackInput)
            {
                Vector3 mPosition = Input.mousePosition;
                Vector3 oPosition = player.transform.position;

                mPosition.z = oPosition.z - Camera.main.transform.position.z;
                Vector3 target = Camera.main.ScreenToWorldPoint(mPosition);
                float dz = target.z - oPosition.z;
                float dx = target.x - oPosition.x;
                float rotateDegree = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg;
                player.transform.rotation = Quaternion.Euler(0f, rotateDegree, 0f);

                player.ChangeState(PlayerState.attack);
                return;
            }


            // Q ��ų 
            if (player.GetInput().KeyCodeQ)
            {
                player.ChangeState(PlayerState.abilityAttack);
                return;
            }
        }

        public override void PhysicsExcute()
        {
            time += Time.deltaTime;
            if (time > 5.5f && !Input.anyKeyDown)
            {
                time = 0;
                player.ChangeState(PlayerState.idle);
                return;
            }

            
            return;
        }

        public override void Exit()
        {
            time = 0;
            // Debug.Log("PlayerCombatIdleExit");
            player.SetPrevState(PlayerState.combatIdle);
            animator.SetBool("combatIdle", false);
        }
    }

    public class PlayerRunning : BaseState<Player>
    {
        private Player player;
        private Animator animator;

        public PlayerRunning(Player owner)
        {
            this.player = owner;
        }

        public override void Enter()
        {
            //Debug.Log("PlayerRunningEnter");

            animator = player.GetAnimator();

            player.SetCurrentState(PlayerState.running);
            animator.SetBool("running", true);
        }

        public override void Excute()
        {
            //Debug.Log("PlayerRunningExcute");

            if (player.GetInput().MoveInput)
            {
                RaycastHit click;

                if (Physics.Raycast(player.GetMainCamera().ScreenPointToRay(Input.mousePosition), out click))  // Ŭ���� ���� �����ɽ�Ʈ
                {
                    if (click.transform.gameObject.CompareTag("NPC"))
                    {
                        return;
                    }

                    player.SetPosition(click.point);
                }
                return;
            }

            if (player.GetInput().AttackInput)
            {
                player.ChangeState(PlayerState.attack);
                return;
            }

            if (player.GetInput().KeyCodeQ)
            {
                player.ChangeState(PlayerState.abilityAttack);
                return;
            }
        }

        public override void PhysicsExcute()
        {
            if (player.GetIsMove())
            {
                if (Vector3.Distance(player.GetTargetPosition(), player.transform.position) <= 0.1f)
                {
                    player.SetIsMove(false);
                    player.ChangeState(PlayerState.combatIdle);
                    return;
                }
            }

            var pos = player.GetTargetPosition() - player.transform.position;
            pos.y = 0f;

            // ������ ��ġ���� �̵�
            var rotate = Quaternion.LookRotation(pos);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotate, Time.deltaTime * 5f);  // õõ�� ȸ��
            player.transform.position += pos.normalized * Time.deltaTime * player.GetSpeed();
            return;
        }

        public override void Exit()
        {
            //Debug.Log("PlayerRunningExit");

            player.prevState = PlayerState.running;
            animator.SetBool("running", false);
        }
    }

    public class PlayerAttack : BaseState<Player>
    {
        private Player player;
        private Animator animator;

        public PlayerAttack(Player owner)
        {
            this.player = owner;
        }

        public override void Enter()
        {
            //Debug.Log("PlayerAttackEnter");

            animator = player.GetAnimator();
            this.player.SetCurrentState(PlayerState.attack);
            animator.SetBool("attack", true);
        }

        public override void Excute()
        {
            // Debug.Log("PlayerAttackExcute");

            if (player.GetInput().MoveInput)
            {
                RaycastHit click;

                if (Physics.Raycast(player.GetMainCamera().ScreenPointToRay(Input.mousePosition), out click))  // Ŭ���� ���� �����ɽ�Ʈ
                {
                    if (click.transform.gameObject.CompareTag("NPC"))
                    {
                        return;
                    }

                    player.SetPosition(click.point);
                    player.ChangeState(PlayerState.running);
                }
                return;
            }

            // ������ �̵� �Է��� ������ ������ �ִϸ��̼� ���
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("DefaltAttack")
                    && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
            {
                return;
            }
            else
            {
                player.ChangeState(PlayerState.combatIdle);
            }

            if (player.GetInput().AttackInput)
            {
                animator.Play("DefaltAttack", -1, 0.1f);
                return;
            }
        }

        public override void Exit()
        {
            //Debug.Log("PlayerAttackExit");
            player.SetPrevState(PlayerState.attack);
            animator.SetBool("attack", false);
        }

        public override void PhysicsExcute()
        {

        }
    }

    public class PlayerAbilityAttack : BaseState<Player>
    {
        private Player player;
        private Animator animator;

        public PlayerAbilityAttack(Player owner)
        {
            this.player = owner;
        }

        public override void Enter()
        {
            //Debug.Log("PlayerAbilityEnter");

            animator = player.GetAnimator();
            this.player.SetCurrentState(PlayerState.abilityAttack);

            animator.SetBool("abilityAttack", true);
        }

        public override void Excute()
        {
            //Debug.Log("PlayerAbilityExcute");
            // ������ �ִϸ��̼� ���
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Ability")
             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                player.ChangeState(PlayerState.combatIdle);
                return;
            }
        }

        public override void Exit()
        {
            //Debug.Log("PlayerAbilityExit");
            player.SetPrevState(PlayerState.abilityAttack);
            animator.SetBool("abilityAttack", false);
        }

        public override void PhysicsExcute()
        {

        }
    }
}





