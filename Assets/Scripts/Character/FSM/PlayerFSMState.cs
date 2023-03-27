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

            if (player.GetInput().MoveInput)
            {
                RaycastHit click;

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
            }

            if (player.GetInput().AttackInput)
            {
                player.ChangeState(PlayerState.attack);
            }

            if (player.GetInput().KeyCodeQ)
            {
                player.ChangeState(PlayerState.abilityAttack);
            }
        }

        public override void PhysicsExcute()
        {

        }

        public override void Exit()
        {
            //Debug.Log("PlayerIdleExit");
            player.SetPrevState(PlayerState.idle);
            animator.SetBool("idle", false);;
        }
    }

    public class PlayerCombatIdle : BaseState<Player>
    {
        private Player player;
        private Animator animator;

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
                    if(click.transform.gameObject.CompareTag("NPC"))
                    {

                    }

                    player.SetPosition(click.point);
                    player.ChangeState(PlayerState.running);
                }
            }
            // ����
            if (player.GetInput().AttackInput)
            {
                player.ChangeState(PlayerState.attack);
            }
            // Q ��ų 
            if (player.GetInput().KeyCodeQ)
            {
                player.ChangeState(PlayerState.abilityAttack);
            }
        }

        public override void PhysicsExcute()
        {

        }

        public override void Exit()
        {
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
            animator.SetBool("isIdle", false);
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

                    }

                    player.SetPosition(click.point);
                }
            }

            if (player.GetInput().AttackInput)
            {

                player.ChangeState(PlayerState.attack);
            }

            if (player.GetInput().KeyCodeQ)
            {
                player.ChangeState(PlayerState.abilityAttack);
            }
        }

        public override void PhysicsExcute()
        {
            Vector3 pos = player.GetTargetPosition() - player.transform.position;
            pos.y = 0f;

            if (pos.magnitude <= 0.1f)
            {
                player.ChangeState(PlayerState.combatIdle);
                return;
            }

            // ������ ��ġ���� �̵�
            var rotate = Quaternion.LookRotation(pos);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotate, Time.deltaTime * 5f);  // õõ�� ȸ��
            player.transform.position += pos.normalized * Time.deltaTime * player.GetSpeed();
        }

        public override void Exit()
        {
            //Debug.Log("PlayerRunningExit");

            player.prevState = PlayerState.running;
            animator.SetBool("running", false);
            animator.SetBool("isIdle", true);
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
            animator.SetBool("isIdle", false);
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

                    }

                    player.SetPosition(click.point);
                    player.ChangeState(PlayerState.running);
                }
            }

            if (player.GetInput().AttackInput)
            {
                animator.Play("DefaltAttack", -1, 0.1f);
            }

            // ������ �̵� �Է��� ������ ������ �ִϸ��̼� ���
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("DefaltAttack")
                    && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)  
            {
                player.ChangeState(PlayerState.combatIdle);
            }
        }

        public override void Exit()
        {
            //Debug.Log("PlayerAttackExit");
            player.SetPrevState(PlayerState.attack);
            animator.SetBool("attack", false);
            animator.SetBool("isIdle", true);
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
            animator.SetBool("isIdle", false);
        }

        public override void Excute()
        {
            //Debug.Log("PlayerAbilityExcute");
            // ������ �ִϸ��̼� ���
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Ability")
             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                player.ChangeState(PlayerState.combatIdle);
            }
        }

        public override void Exit()
        {
            //Debug.Log("PlayerAbilityExit");
            player.SetPrevState(PlayerState.abilityAttack);
            animator.SetBool("abilityAttack", false);
            animator.SetBool("isIdle", true);
        }

        public override void PhysicsExcute()
        {

        }
    }
}





