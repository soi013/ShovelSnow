using JPLab2.Model;
using JPLab2.View;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace JPLab2.Presenter
{
    public class PlayerPresenter
    {
        private const float speedMove = 3f;
        private float speedRotate = 1f;

        private readonly IPlayerModel playerModel;
        private readonly Player playerView;
        private readonly Rigidbody playerBody;

        public PlayerPresenter(IPlayerModel playerModel, Player playerView)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            this.playerModel = playerModel;
            this.playerView = playerView;
            this.playerBody = playerView.GetComponent<Rigidbody>();

            this.playerView
                .FixedUpdateAsObservable()
                .TakeWhile(_ => !playerModel.IsDead.Value)
                .Subscribe(_ => Update());
        }

        private void Update()
        {
            Move();
            playerModel.Position.Value = playerView.transform.position;
        }

        private void Move()
        {
            var verticalInput = Input.GetAxis("Vertical");
            if (verticalInput == 0)
                return;

            playerBody.velocity = speedMove * verticalInput * playerView.transform.forward;

            Rotation(verticalInput);
        }

        private void Rotation(float verticalInput)
        {
            var horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput == 0)
                return;

            playerBody.angularVelocity = horizontalInput * verticalInput * speedRotate * playerView.transform.up;
        }
    }
}
