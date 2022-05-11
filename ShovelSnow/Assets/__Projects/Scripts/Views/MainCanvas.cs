using TMPro;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshStatus;

    void Start()
    {
        Debug.Log($"{this.GetType().Name} {nameof(Start)} 00");
    }

    public void ChangeText(string text) => textMeshStatus.text = text;
}
