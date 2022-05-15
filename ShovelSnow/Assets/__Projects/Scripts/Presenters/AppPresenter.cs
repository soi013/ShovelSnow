using JPLab2.Model;
using UnityEngine;
using Zenject;

namespace JPLab2.Presenter
{
    public class AppPresenter : MonoBehaviour
    {
        private IAppModel appModel;

        private PlayerPresenter playerP;
        private SnowPresenter snowP;
        private MainCanvasPresenter mainCameraP;

        public AppPresenter()
        {
            Debug.Log($"{this.GetType().Name} ctor 00");
        }

        [Inject]
        public void Injection(IAppModel appModel, PlayerPresenter playerP, SnowPresenter snowP, MainCanvasPresenter mainCameraP)
        {
            Debug.Log($"{this.GetType().Name} {nameof(Injection)} 00");

            //Presenterのインスタンスはここで生成して確保する
            this.appModel = appModel;
            this.playerP = playerP;
            this.snowP = snowP;
            this.mainCameraP = mainCameraP;
        }

        void Start()
        {
            Debug.Log($"{this.GetType().Name} {nameof(Start)} 00");
            appModel.Initialize();
        }
    }
}
