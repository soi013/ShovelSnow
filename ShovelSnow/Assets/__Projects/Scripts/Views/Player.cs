using JPLab2.Infrastructure;
using System.Collections.Generic;
using System.Linq;
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

        Vector3 offsetOrigin = new(0, 0.5f, 0);
        Vector3 direction = Vector3.up;
        Vector3 size = new(0.4f, 1f, 0.4f);

        public IReadOnlyReactiveProperty<bool> IsTouchingGround => isTouchingGround;
        private readonly ReactiveProperty<bool> isTouchingGround = new();

        public ReactiveProperty<bool> CanMove { get; } = new ReactiveProperty<bool>(true);
        public IReadOnlyReactiveProperty<IReadOnlyList<string>> SnowsHit => snowsHit;
        private readonly ReactiveProperty<IReadOnlyList<string>> snowsHit = new(new string[0]);

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
            this.IsTouchCollisionAsObservable(x => IsGround(x))
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

            this.FixedUpdateAsObservable()
                .Select(_ => SearchHitSnow())
                .Subscribe(x =>
                    snowsHit.Value = x);
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

        void OnDrawGizmos()
        {
            Vector3 origin = transform.position + offsetOrigin;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(origin + new Vector3(0f, size.y, 0f) / 2, size);
        }

        private string[] SearchHitSnow()
        {
            Vector3 origin = transform.position + offsetOrigin;
            float distance = size.y / 2;
            Debug.DrawRay(origin, direction * distance, Color.yellow);

            var hitInfos = Physics.BoxCastAll(origin, size / 2f, direction, Quaternion.identity, maxDistance: distance);

            const string snowTag = "Snow";

            return hitInfos
                .Select(x => x.collider.gameObject)
                .Where(g => g != null && g.CompareTag(snowTag) == true)
                .Select(x => x.name)
                .ToArray();
        }
    }
}
