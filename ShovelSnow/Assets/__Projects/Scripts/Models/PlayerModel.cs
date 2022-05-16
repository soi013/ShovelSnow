using JPLab2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ReactiveProperty<IReadOnlyList<string>> SnowsHit { get; } = new(new string[0]);

        public PlayerModel(AppSpeed appSpeed)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            double thresholdDeathOnSnowMilliSec = 3000;
            IObservable<bool> isDeadByPositionStream = Position
                .Skip(1)
                .Select(x => IsDeadByPosition(x))
                .Do(x =>
                {
                    if (x)
                        Debug.Log($"IsDeadByPosition");
                });

            IObservable<bool> isDeadBySnowStream = SnowsHit
                .Select(x => IsTooManySnowHits(x))
                .Buffer(TimeSpan.FromMilliseconds(thresholdDeathOnSnowMilliSec / appSpeed.Gain))
                .Select(x => x.Any() && x.All(x => x))
                .Do(x =>
                {
                    if (x)
                        Debug.Log($"DeadBySnow");
                });

            IsDead = Observable
                .Merge(
                    isDeadByPositionStream,
                    isDeadBySnowStream)
                .TakeWhile(x => !IsDead.Value)
                .ToReadOnlyReactiveProperty(false);

            CanMove = Observable.CombineLatest(
                    IsTouchingGround,
                    IsDead,
                    (ground, dead) => ground && !dead)
                .ToReadOnlyReactiveProperty();

            SnowsHit
                .Subscribe(x => Debug.Log($"SnowHits {x.ConcatenateString(", ")}"));
        }

        private bool IsTooManySnowHits(IReadOnlyList<string> snows)
        {
            const int thresholdCountSnow = 3;
            return snows.Count >= thresholdCountSnow;
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
        ReactiveProperty<IReadOnlyList<string>> SnowsHit { get; }
    }
}