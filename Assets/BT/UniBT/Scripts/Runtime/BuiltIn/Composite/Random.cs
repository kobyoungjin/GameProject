namespace UniBT
{
    public class Random : Composite
    {
        private NodeBehavior runningNode;

        protected override Status OnUpdate()
        {
            if (runningNode != null)  // ���� ���°� ���� ���� ��� ���� ���� ��带 ������Ʈ
            {
                return HandleStatus(runningNode.Update(), runningNode);
            }

            var result = UnityEngine.Random.Range(0, Children.Count);
            var target = Children[result];
            return HandleStatus(target.Update(), target);
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