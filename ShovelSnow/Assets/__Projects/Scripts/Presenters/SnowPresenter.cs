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
            const float scaleSnowFallRangeY = 2f;
            const float offsetY = 20f;

            snowModel.Snows.ObserveAdd()
                        .Select(s => s.Value)
                        .Select(s =>
                             new Vector3(
                                s.Position.x * scaleSnowFallRangeXZ,
                                s.Position.y * scaleSnowFallRangeY + offsetY,
                                s.Position.z * scaleSnowFallRangeXZ))
                        .Subscribe(s =>
                            snowView.FallSnow(s));
        }
    }
}
