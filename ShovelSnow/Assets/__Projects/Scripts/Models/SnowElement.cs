using UnityEngine;

namespace JPLab2.Model
{
    public class SnowElement
    {
        private readonly float snowFallRange = 1f;

        public Vector3 Position { get; }
        public SnowElement()
        {
            Position = new Vector3(
                Random.Range(-snowFallRange, snowFallRange),
                Random.Range(-snowFallRange, snowFallRange),
                Random.Range(-snowFallRange, snowFallRange));
        }
    }
}