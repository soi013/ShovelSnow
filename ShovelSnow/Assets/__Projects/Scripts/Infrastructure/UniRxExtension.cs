using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace JPLab2.Infrastructure
{

    public static class UniRxExtension
    {
        public static IObservable<bool> IsTouchCollisionAsObservable(this Component compHasCollision, Func<Collision, bool> predict) =>
            Observable
                .Merge(
                    compHasCollision.OnCollisionEnterAsObservable().Where(x => predict(x)).Select(_ => true),
                    compHasCollision.OnCollisionStayAsObservable().Where(x => predict(x)).Select(_ => true),
                    compHasCollision.OnCollisionExitAsObservable().Where(x => predict(x)).Select(_ => false));
    }
}
