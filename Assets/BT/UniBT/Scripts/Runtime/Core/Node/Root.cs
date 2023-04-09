using UnityEngine;

namespace UniBT
{
    public class Root : NodeBehavior  // �ֻ��� ��� Ŭ����
    {
        [SerializeReference]
        private NodeBehavior child;

#if UNITY_EDITOR
        [HideInEditorWindow]
        public System.Action UpdateEditor;
#endif
        public NodeBehavior Child
        {
            get => child;
#if UNITY_EDITOR
            set => child = value;
#endif
        }

        protected sealed override void OnRun()
        {
            child.Run(gameObject);
        }

        public override void Awake()
        {
            child.Awake();
        }

        public override void Start()
        {
           child.Start();
        }

        public override void PreUpdate()  // ������Ʈ ����
        {
            child.PreUpdate();
        }

        protected sealed override Status OnUpdate()  // ������Ʈ ��
        {
#if UNITY_EDITOR
            UpdateEditor?.Invoke();
#endif
            return child.Update();
        }
        
        
        public override void PostUpdate()  // ������Ʈ ����
        {
            child.PostUpdate();
        }

        public override void Abort()  // �ߴ�
        {
            child.Abort();
        }

    }
}