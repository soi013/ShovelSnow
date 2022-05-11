using JPLab2.Model;
using JPLab2.View;
using UnityEngine;

namespace JPLab2.Presenter
{

    //public class PlayerPresenter : MonoBehaviour
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
        }

        internal void Start()
        {
            Debug.Log($"{this.GetType().Name} {nameof(Start)} 00");
        }

        public void Update()
        {
            if (playerModel.IsDead.Value)
                return;

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
