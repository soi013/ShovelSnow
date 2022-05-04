using System;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace JPLab2.Model
{
    internal class SnowModel : ISnowModel
    {
        IAppModel appModel;
        public float SnowFallIntervalMilliSeconds { get; set; } = 250;

        public IReadOnlyReactiveCollection<SnowElement> Snows => snows;

        public ReadOnlyReactiveProperty<int> CurrentFallCount { get; private set; }

        private readonly ReactiveCollection<SnowElement> snows = new();

        [Inject]
        public void Injection(IAppModel appModel)
        {
            Debug.Log($"{this.GetType().Name} {nameof(Injection)} 00");
            this.appModel = appModel;

            Observable.Interval(TimeSpan.FromMilliseconds(SnowFallIntervalMilliSeconds))
                 .TakeWhile(_ => appModel.State.Value != AppState.GameOver)
                 .SelectMany(index => Enumerable.Range(0, CurrentFallCount.Value)
                    .Select(_ => new SnowElement()))
                 .Subscribe(x => snows.Add(x));

            const int intervalUpdateFallCountSec = 10;
            const int countStartFall = 1;

            CurrentFallCount = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(intervalUpdateFallCountSec))
                .Select(i => (int)(MathF.Pow(2, i) * countStartFall))
                .ToReadOnlyReactiveProperty();
        }
    }

    public interface ISnowModel
    {
        IReadOnlyReactiveCollection<SnowElement> Snows { get; }
    }
}
