using UnityEngine;

namespace UniBT
{
    public class Sequence : Composite  
    {
        [SerializeField]
        private bool abortOnConditionChanged = true;  // ���� ���� ��庸�� �켱 ������ ���� ��尡 ���� �Ұ��������� ���� ���� ��带 �ߴ��մϴ�.

        private NodeBehavior runningNode;

        public override bool CanUpdate()
        {
            foreach (var child in Children)  // �� ���� ��� �ڽ��� ������Ʈ�� �� ���� �� ������Ʈ�� �� �ִ�.
            {
                if (!child.CanUpdate())
                {
                    return false;
                }
            }
            return true;
        }

        protected override Status OnUpdate()
        {
            if (runningNode != null)  // ���� ���°� ���� ���� ��� ���� ���� ��带 ������Ʈ
            {
                if (abortOnConditionChanged && IsConditionChanged(runningNode))  // �켱 �������� ���� ��尡 ���� �Ұ����ϸ�
                {
                    runningNode.Abort();
                    return UpdateWhileSuccess(0);
                }

                var currentOrder = Children.IndexOf(runningNode);
                var status = runningNode.Update();
                if (status == Status.Success)  // ���� �������� ��尡 ������ ������� 
                {
                    // update next nodes
                    return UpdateWhileSuccess(currentOrder + 1);
                }

                return HandleStatus(status, runningNode);
            }

            return UpdateWhileSuccess(0);

        }

        private bool IsConditionChanged(NodeBehavior runningChild)  // �ڽź��� �켱 ������ ���� ����� ������ ������Ʈ�� �� ���� ���
        {
            var priority = Children.IndexOf(runningChild);
            for (var i = 0; i < priority; i++)
            {
                var candidate = Children[i];
                if (!candidate.CanUpdate()) // �켱���� ���� ��尡 �����̰� �ȵǸ� // NodeBehaviorŬ������ b���� canUpdate
                {
                    return true;
                }
            }

            return false;
        }

        private Status UpdateWhileSuccess(int start)  // ���� �������� ��尡 �Ϸ�Ǵ� ����Ǵ� �Լ�
        {
            for (var i = start; i < Children.Count; i++)
            {
                var target = Children[i];
                var childStatus = target.Update();
                if (childStatus == Status.Success)
                {
                    continue;
                }
                return HandleStatus(childStatus, target);  // ���� ��� ����
            }

            return HandleStatus(Status.Success, null);  
        }

        private Status HandleStatus(Status status, NodeBehavior updated)
        {
            runningNode = status == Status.Running ? updated : null;
            return status;
        }

        public override void Abort()
        {
            if (runningNode != null)
            {
                runningNode.Abort();
                runningNode = null;
            }
        }
    }
}