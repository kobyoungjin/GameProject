using UnityEngine;

namespace UniBT
{
    public class Selector : Composite
    {
        [SerializeField]
        private bool abortOnConditionChanged = true;

        private NodeBehavior runningNode;

        public override bool CanUpdate()  // �� ���� �ڽ��� ������Ʈ�� �� ���� �� ������Ʈ�� �� �ִ�.
        {
            foreach (var child in Children)
            {
                if (child.CanUpdate())
                {
                    return true;
                }
            }
            return false;
        }

        protected override Status OnUpdate()
        {
            if (runningNode != null)  // ���� ���°� ���� ���� ��� ���� ���� ��带 ������Ʈ
            {
                if (abortOnConditionChanged && IsConditionChanged(runningNode))  // �켱 �������� ���� ��尡 ���� �Ұ����ϸ�
                {
                    runningNode.Abort();
                    return UpdateWhileFailure(0);
                }
                var currentOrder = Children.IndexOf(runningNode);
                var status = runningNode.Update();
                if (status == Status.Failure)  // ���� �������� ��尡 Failed�̸�
                {
                    // update next nodes
                    return UpdateWhileFailure(currentOrder + 1);
                }

                return HandleStatus(status, runningNode);
            }

            return UpdateWhileFailure(0);
        }

        private bool IsConditionChanged(NodeBehavior runningChild)  // �ڽź��� �켱 ������ ���� ����� ������ ������Ʈ�� �� �ִ� ���
        {
            var priority = Children.IndexOf(runningChild);
            for (var i = 0; i < priority; i++)
            {
                var candidate = Children[i];
                if (candidate.CanUpdate())
                {
                    return true;
                }
            }

            return false;
        }

        private Status UpdateWhileFailure(int start)  
        {
            for (var i = start; i < Children.Count; i++)
            {
                var target = Children[i];
                var childStatus = target.Update();
                if (childStatus == Status.Failure)
                {
                    continue;
                }
                return HandleStatus(childStatus, target);
            }

            return HandleStatus(Status.Failure, null);
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