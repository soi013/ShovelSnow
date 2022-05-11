using UnityEngine;

namespace JPLab2.View
{
    public class Player : MonoBehaviour
    {
        public Player()
        {
            Debug.Log($"{this.GetType().Name} ctor 00");
        }

        void Start()
        {
            Debug.Log($"{this.GetType().Name} {nameof(Start)} 00");
        }
    }
}
