using UnityEngine;

namespace UniBT
{
    /// <summary>
    ///  update the children in order.
    ///  update only one child per frame.
    /// </summary>
    public class Rotator : Composite  // ���� ��带 ������� ������Ʈ
    {
        [SerializeField]
        private bool resetOnAbort;

        private int targetIndex;

        private NodeBehavior runningNode;

        protected override Status OnUpdate()
        {
            if (runningNode != null)  // ���� ���°� �������� ��� ���� �������� ��� ������Ʈ
            {
                return HandleStatus(runningNode.Update(), runningNode);
            }

            var target = Children[targetIndex];
            return HandleStatus(target.Update(), target);
        }

        private void SetNext()
        {
            targetIndex++;
            if (targetIndex >= Children.Count)
            {
                targetIndex = 0;
            }
        }

        private Status HandleStatus(Status status, NodeBehavior updated)
        {
            if (status == Status.Running)  // ���� ���°� running ���¸�
            {
                runningNode = updated;
            }
            else  // ���� �������� ��� null�� �ٲٰ� ���� �������� �ٲٱ�
            {
                runningNode = null;
                SetNext();
            }
            return status;
        }

        public override void Abort()
        {
            if (runningNode != null)
            {
                runningNode.Abort();
                runningNode = null;
            }

            if (resetOnAbort)
            {
                targetIndex = 0;
            }
        }
    }
}