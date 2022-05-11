using JPLab2.Model;
using JPLab2.View;
using UniRx;
using UnityEngine;

namespace JPLab2.Presenter
{
    public class SnowPresenter
    {
        private readonly ISnowModel snowModel;
        private readonly SnowManager snowView;
        private readonly float scaleSnowFallRange = 7;

        public SnowPresenter(ISnowModel snowModel, SnowManager snowView)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            this.snowModel = snowModel;
            this.snowView = snowView;

            BindWithModel();
        }

        private void BindWithModel()
        {
            Debug.Log($"{this.GetType().Name} {nameof(BindWithModel)} 00");

            snowModel.Snows.ObserveAdd()
                .Select(s => s.Value)
                .Select(s =>
                     new Vector3(
                        s.PositionX * scaleSnowFallRange,
                        Random.Range(18f, 22f),
                        s.PositionZ * scaleSnowFallRange))
                .Subscribe(s =>
                    snowView.FallSnow(s,
                        Random.Range(0, 90f)));
        }
    }
}
