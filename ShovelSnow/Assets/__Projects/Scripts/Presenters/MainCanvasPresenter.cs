
using JPLab2.Model;
using UniRx;
using UnityEngine;

namespace JPLab2.Presenter
{
    public class MainCanvasPresenter
    {
        public MainCanvasPresenter(IAppModel appModel, MainCanvas mainCameraCanvasView)
        {
            Debug.Log($"{this.GetType().Name} ctor 00");

            appModel.State
                   .Subscribe(state =>
                    Debug.Log($"Presenter state = {state}"));

            appModel.State
                   .Subscribe(state =>
                   mainCameraCanvasView.StateText.Value = state.ToString());

            appModel.PlayTime
                .Subscribe(x =>
                    mainCameraCanvasView.PlayTime.Value = x);
        }
    }
}
