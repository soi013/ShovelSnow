using TMPro;
using UniRx;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshStatus;

    public ReactiveProperty<string> StateText { get; } = new("-----");
    public ReactiveProperty<float> PlayTime { get; } = new();

    void Start()
    {
        Debug.Log($"{this.GetType().Name} {nameof(Start)} 00");

        Observable.CombineLatest(
                StateText.Select(s => $"[{s}] "),
                PlayTime.Select(t => $"{t:0000.000}"),
                    (s, t) => $"{s} {t} sec")
            .Subscribe(x => ChangeText(x));
    }

    public void ChangeText(string text) => textMeshStatus.text = text;
}
