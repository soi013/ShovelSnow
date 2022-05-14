using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace JPLab2.View
{
    public class Player : MonoBehaviour
    {
        public Rigidbody Body => body;
        [SerializeField] private Rigidbody body;

        private const float speedMove = 3f;
        private const float speedRotate = 1f;
        public IReadOnlyReactiveProperty<bool> IsTouchingGround => isTouchingGround;
        private readonly ReactiveProperty<bool> isTouchingGround = new();

        public ReactiveProperty<bool> CanMove { get; } = new ReactiveProperty<bool>(true);


        public Player()
        {
            Debug.Log($"{this.GetType().Name} ctor 00");
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

            this.FixedUpdateAsObservable()
                .Where(_ => CanMove.Value)
                .Select(_ => Input.GetAxis("Vertical"))
                .Where(v => v != 0)
                .Select(verticalInput => new
                {
                    verticalInput,
                    horizontalInput = Input.GetAxis("Horizontal")

                })
                .Subscribe(a => Move(a.verticalInput, a.horizontalInput));
        }

        private bool IsGround(Collision touchCollision)
        {
            const string groundTag = "Ground";
            return touchCollision.gameObject.CompareTag(groundTag);
        }


        private void Move(float verticalInput, float horizontalInput)
        {
            Body.velocity = speedMove * verticalInput * transform.forward;

            Rotation(verticalInput, horizontalInput);
        }

        private void Rotation(float verticalInput, float horizontalInput)
        {
            if (horizontalInput == 0)
                return;

            Body.angularVelocity = horizontalInput * verticalInput * speedRotate * transform.up;
        }
    }
}
