using UniRx;
using UnityEngine;

namespace JPLab2.Model
{
    internal class AppModel : IAppModel
    {
        public IReadOnlyReactiveProperty<AppState> State => state;
        private readonly ReactiveProperty<AppState> state = new(AppState.Initializing);

        public AppModel(IPlayerModel playerModel)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            State
                .Pairwise()
                .Subscribe(p =>
                    Debug.Log($"{this.GetType().Name} StateChange {p.Previous} -> {p.Current}"));

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