using JPLab2.Model;
using JPLab2.View;
using UniRx;
using UnityEngine;
using Zenject;

namespace JPLab2.Presenter
{
    public class ZenInstaller : MonoInstaller<ZenInstaller>
    {
        [SerializeField] private GameObject playerGo;
        [SerializeField] private GameObject snowManagerGo;
        [SerializeField] private GameObject mainCanvasGo;

        public override void InstallBindings()
        {
            Debug.Log($"{this.GetType().Name} {nameof(InstallBindings)} 00");
            Container
                .Bind<IScheduler>().FromInstance(Scheduler.MainThread)
                .AsSingle();

            Container
                .Bind<IPlayerModel>().To<PlayerModel>()
                .AsSingle();

            Container
                .Bind<IAppModel>().To<AppModel>()
                .AsSingle();

            Container
                .Bind<ISnowModel>().To<SnowModel>()
                .AsSingle();

            Container
                .Bind<Player>().FromComponentOn(playerGo).AsCached();
            Container
                .Bind<SnowManager>().FromComponentOn(snowManagerGo).AsCached();
            Container
                .Bind<MainCanvas>().FromComponentOn(mainCanvasGo).AsCached();

            Container
                .Bind<PlayerPresenter>()
                .AsSingle();
            Container
                .Bind<SnowPresenter>()
                .AsSingle();
            Container
                .Bind<MainCanvasPresenter>()
                .AsSingle();

            Debug.Log($"{this.GetType().Name} {nameof(InstallBindings)} 90");
        }
    }
}
