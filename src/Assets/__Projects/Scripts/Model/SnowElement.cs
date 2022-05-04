using UnityEngine;

namespace JPLab2.Model
{
    public class SnowElement
    {
        private readonly float snowFallRange = 1f;

        public float PositionX { get; private set; }
        public float PositionZ { get; private set; }
        public SnowElement()
        {
            PositionX = Random.Range(-snowFallRange, snowFallRange);
            PositionZ = Random.Range(-snowFallRange, snowFallRange);
        }
    }
}