using JPLab2.Model;
using JPLab2.View;
using UniRx;
using UnityEngine;

namespace JPLab2.Presenter
{
    public class SnowPresenter
    {
        public SnowPresenter(ISnowModel snowModel, SnowManager snowView)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            const float scaleSnowFallRangeXZ = 7f;
            const float scaleSnowFallRangeY = 20f;

            snowModel.Snows.ObserveAdd()
                        .Select(s => s.Value.Position.Value)
                        .Select(s =>
                             new Vector3(
                                s.x * scaleSnowFallRangeXZ,
                                s.y * scaleSnowFallRangeY,
                                s.z * scaleSnowFallRangeXZ))
                        .Subscribe(s =>
                            snowView.FallSnow(s));
        }
    }
}
