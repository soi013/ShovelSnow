
using JPLab2.Model;
using System;
using UniRx;
using UnityEngine;

namespace JPLab2.Presenter
{
    public class MainCanvasPresenter
    {
        private readonly IAppModel appModel;
        private readonly MainCanvas mainCameraCanvasView;
        private readonly ReadOnlyReactiveProperty<string> currentStatusText;

        public MainCanvasPresenter(IAppModel appModel, MainCanvas mainCameraCanvasView)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            this.appModel = appModel;
            this.mainCameraCanvasView = mainCameraCanvasView;

            var statusText = appModel.State
                .Select(x => $"[{x}] ");

            var timeSec = Observable.Interval(TimeSpan.FromMilliseconds(100))
                .Where(_ => appModel.State.Value == AppState.Playing)
                .Select(_ => Time.realtimeSinceStartup);

            currentStatusText = Observable.CombineLatest(
                 statusText,
                 timeSec,
                 (s, t) => $"{s} {t:0000.000}sec")
                 .ToReadOnlyReactiveProperty();

            currentStatusText
                .Subscribe(x => mainCameraCanvasView.ChangeText(x));
        }

        internal void Start()
        {
            Debug.Log($"{this.GetType().Name} {nameof(Start)} 00");
        }
    }
}
