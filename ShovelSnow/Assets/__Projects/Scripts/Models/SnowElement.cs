using UniRx;
using UnityEngine;

namespace JPLab2.Model
{
    public class SnowElement
    {
        private readonly float snowFallRange = 1f;
        private readonly float DeadLineY = 0f;

        public ReactiveProperty<Vector3> Position { get; }

        public ReadOnlyReactiveProperty<bool> IsAlive { get; }
        public SnowElement()
        {
            Position = new(
                new Vector3(
                    Random.Range(-snowFallRange, snowFallRange),
                    Random.Range(0.9f * snowFallRange, snowFallRange),
                    Random.Range(-snowFallRange, snowFallRange)));

            IsAlive = Position
                .Select(v => v.y > DeadLineY)
                .ToReadOnlyReactiveProperty();
        }
    }
}