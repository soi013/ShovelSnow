using JPLab2.Model;
using JPLab2.View;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace JPLab2.Presenter
{
    public class PlayerPresenter
    {
        public PlayerPresenter(IPlayerModel playerModel, Player playerView)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            playerModel
                .CanMove
                .Subscribe(x =>
                    playerView.CanMove.Value = x);

            playerView.FixedUpdateAsObservable()
                .TakeWhile(_ => !playerModel.IsDead.Value)
                .Subscribe(_ => playerModel.Position.Value = playerView.transform.position);

            playerView.IsTouchingGround
                .Subscribe(x =>
                    playerModel.IsTouchingGround.Value = x);
        }
    }
}
