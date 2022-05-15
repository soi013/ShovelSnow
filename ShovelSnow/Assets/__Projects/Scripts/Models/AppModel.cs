using System;
using UniRx;
using UnityEngine;

namespace JPLab2.Model
{
    public class AppModel : IAppModel
    {
        public IReadOnlyReactiveProperty<AppState> State => state;
        private readonly ReactiveProperty<AppState> state = new(AppState.Initializing);

        public IReadOnlyReactiveProperty<float> PlayTime { get; }

        public AppModel(IPlayerModel playerModel)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            State
                .Pairwise()
                .Subscribe(p =>
                    Debug.Log($"{this.GetType().Name} StateChange {p.Previous} -> {p.Current}"));

            PlayTime = Observable.Interval(TimeSpan.FromMilliseconds(100))
                .Where(_ => State.Value == AppState.Playing)
                .Select(_ => Time.realtimeSinceStartup)
                .ToReadOnlyReactiveProperty();

            playerModel.IsDead
                .Where(x => x)
                .Subscribe(_ => DeathOfPlayer());
        }

        public void Initialize()
        {
            Debug.Log($"{this.GetType().Name} {nameof(Initialize)} 00");

            state.Value = AppState.Playing;
        }

        public void DeathOfPlayer()
        {
            state.Value = AppState.GameOver;
        }
    }

    public interface IAppModel
    {
        IReadOnlyReactiveProperty<AppState> State { get; }
        IReadOnlyReactiveProperty<float> PlayTime { get; }

        void DeathOfPlayer();
        void Initialize();
    }

    public enum AppState
    {
        Initializing,
        Playing,
        GameOver,
    }
}