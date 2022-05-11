using JPLab2.Model;
using JPLab2.View;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace JPLab2.Presenter
{
    public class PlayerPresenter
    {
        private const float speedMove = 10;
        private const float speedRotate = 40;

        private readonly IPlayerModel playerModel;
        private readonly Player playerView;

        public PlayerPresenter(IPlayerModel playerModel, Player playerView)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            this.playerModel = playerModel;
            this.playerView = playerView;

            this.playerView
                .UpdateAsObservable()
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

            //playerView.transform.Translate(speedMove * Time.deltaTime * verticalInput * Vector3.forward);
            playerView.transform.Translate(speedMove * Time.deltaTime * verticalInput * Vector3.forward);

            Rotation(verticalInput);
        }

        private void Rotation(float verticalInput)
        {
            var horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput == 0)
                return;

            playerView.transform.Rotate(Vector3.up, Time.deltaTime * horizontalInput * verticalInput * speedRotate);
        }
    }
}
