using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace JPLab2.View
{
    public class Player : MonoBehaviour
    {
        public Rigidbody Body => body;
        [SerializeField] private Rigidbody body;

        public IReadOnlyReactiveProperty<bool> IsTouchingGround => isTouchingGround;
        private readonly ReactiveProperty<bool> isTouchingGround = new();

        public Player()
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            IsTouchingGround.Subscribe(x =>
                Debug.Log($"IsTouchGround = {x}"));
        }

        void Start()
        {
            Debug.Log($"{this.GetType().Name} {nameof(Start)} 00");
            Bind();
        }

        private void Bind()
        {
            Observable
                .Merge(
                   this.OnCollisionStayAsObservable().Where(x => IsGround(x)).Select(_ => true),
                   this.OnCollisionExitAsObservable().Where(x => IsGround(x)).Select(_ => false))
                .Subscribe(x =>
                    isTouchingGround.Value = x);
        }

        private bool IsGround(Collision touchCollision)
        {
            const string groundTag = "Ground";
            return touchCollision.gameObject.CompareTag(groundTag);
        }
    }
}
