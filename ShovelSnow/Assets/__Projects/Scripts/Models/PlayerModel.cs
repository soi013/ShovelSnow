using UniRx;
using UnityEngine;

namespace JPLab2.Model
{
    public class PlayerModel : IPlayerModel
    {
        public IReadOnlyReactiveProperty<bool> IsDead { get; }
        public IReactiveProperty<Vector3> Position { get; } = new ReactiveProperty<Vector3>(Vector3.zero);
        public IReactiveProperty<bool> IsTouchingGround { get; } = new ReactiveProperty<bool>();
        public IReadOnlyReactiveProperty<bool> CanMove { get; }

        public PlayerModel()
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            IsDead = Position
                        .Skip(1)
                        .Select(x => IsDeadByPosition(x))
                        .ToReadOnlyReactiveProperty(false);

            CanMove = Observable.CombineLatest(
                    IsTouchingGround,
                    IsDead,
                    (ground, dead) => ground && !dead)
                .ToReadOnlyReactiveProperty();
        }

        private bool IsDeadByPosition(Vector3 postionPlayer)
        {
            const int baseLineY = 2;
            return postionPlayer.y < baseLineY;
        }
    }

    public interface IPlayerModel
    {
        IReadOnlyReactiveProperty<bool> IsDead { get; }
        IReactiveProperty<Vector3> Position { get; }
        IReactiveProperty<bool> IsTouchingGround { get; }
        IReadOnlyReactiveProperty<bool> CanMove { get; }
    }
}