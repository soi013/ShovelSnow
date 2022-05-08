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

        public SnowModel(IAppModel appModel, IScheduler scheduler)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            Observable.Interval(TimeSpan.FromMilliseconds(SnowFallIntervalMilliSeconds), scheduler)
                 .TakeWhile(_ => appModel.State.Value != AppState.GameOver)
                 .SelectMany(index => Enumerable.Range(0, CurrentFallCount.Value)
                    .Select(_ => new SnowElement()))
                 .Subscribe(x => snows.Add(x));

            const int intervalUpdateFallCountSec = 10;
            const int countStartFall = 1;

            CurrentFallCount = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(intervalUpdateFallCountSec), scheduler)
                .Select(i => (int)(MathF.Pow(2, i) * countStartFall))
                .ToReadOnlyReactiveProperty();

            CurrentFallCount.Subscribe(x =>
                Debug.Log($"Fall speed changed. count = {x}"));
        }
    }

    public interface ISnowModel
    {
        IReadOnlyReactiveCollection<SnowElement> Snows { get; }
    }
}
