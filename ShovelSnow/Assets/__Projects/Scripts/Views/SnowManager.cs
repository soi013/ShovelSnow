using UnityEngine;

namespace JPLab2.View
{
    public class SnowManager : MonoBehaviour
    {
        public GameObject snowPrefab;

        public SnowManager()
        {
            Debug.Log($"{this.GetType().Name} ctor 00");
        }

        private void Start()
        {
            Debug.Log($"{this.GetType().Name} {nameof(Start)} 00");
        }

        public GameObject FallSnow(Vector3 position)
        {
            float rotation = Random.Range(0, 90f);

            return Instantiate(
                snowPrefab,
                position,
                Quaternion.Euler(rotation, rotation / 2, 0f),
                this.transform);
        }
    }
}
