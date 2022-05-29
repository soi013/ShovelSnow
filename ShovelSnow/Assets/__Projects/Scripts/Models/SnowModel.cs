using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace JPLab2.Model
{
    public class SnowModel : ISnowModel
    {
        /// <summary>
        /// á‚ª~‚éŠÔŠu
        /// </summary>
        public float SnowFallIntervalMilliSeconds { get; set; } = 250;

        /// <summary>
        /// ~‚Á‚½á‚ÌƒRƒŒƒNƒVƒ‡ƒ“
        /// </summary>
        public IReadOnlyReactiveCollection<SnowElement> Snows => snows;

        private readonly ReactiveCollection<SnowElement> snows = new();

        /// <summary>
        /// ˆê‰ñ‚É~‚éá‚Ì”
        /// </summary>
        public ReadOnlyReactiveProperty<int> CurrentFallCount { get; private set; }
        public readonly int CountStartFall = 1;

        public SnowModel(IAppModel appModel, AppSpeed appSpeed)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            Observable.Interval(TimeSpan.FromMilliseconds(SnowFallIntervalMilliSeconds / appSpeed.Gain))
                 .TakeWhile(_ => appModel.State.Value != AppState.GameOver)
                 .SelectMany(index => Enumerable.Range(0, CurrentFallCount.Value)
                    .Select(_ => new SnowElement()))
                 .Subscribe(x => snows.Add(x));

            const int intervalUpdateFallCountMilliSec = 10000;
            const int baseFallCount = 2.0;

            CurrentFallCount = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(intervalUpdateFallCountMilliSec / appSpeed.Gain))
                .TakeWhile(_ => appModel.State.Value != AppState.GameOver)
                .Select(i => (int)(MathF.Pow(baseFallCount, i) * CountStartFall))
                .ToReadOnlyReactiveProperty(CountStartFall);

            CurrentFallCount.Subscribe(x =>
                Debug.Log($"Fall speed changed. count = {x}"));
        }
    }

    public interface ISnowModel
    {
        IReadOnlyReactiveCollection<SnowElement> Snows { get; }
    }
}
