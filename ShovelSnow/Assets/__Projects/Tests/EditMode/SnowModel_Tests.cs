using Cysharp.Threading.Tasks;
using FluentAssertions;
using JPLab2.Model;
using System.Collections;
using UniRx;
using UnityEngine.TestTools;

namespace JPLab2.Tests
{
    public class SnowModel_Tests
    {
        [UnityTest]
        public IEnumerator Snows_Normal_IncreaseCount() => UniTask.ToCoroutine(async () =>
        {
            //テストを早く終わらせるため、100倍速に設定する
            var appSpeed = new AppSpeed(100);
            var playerM = new PlayerModel(appSpeed);
            var appM = new AppModel(playerM);

            var snowM = new SnowModel(appM, appSpeed);

            //最初は雪がない
            snowM.Snows
                   .Should().HaveCount(0);

            snowM.CurrentFallCount.Value
                .Should().Be(snowM.CountStartFall);

            await UniTask.Delay(10);

            appM.Initialize();

            await UniTask.Delay(50);

            snowM.Snows
                .Should().HaveCountGreaterThan(5);

            await UniTask.Delay(100);

            snowM.CurrentFallCount.Value
                .Should().Be(snowM.CountStartFall * 2);

            snowM.Snows
                .Should().HaveCountGreaterThan(20);

            await UniTask.Delay(500);

            snowM.Snows
                .Should().HaveCountGreaterThan(1000);

            playerM.Position.Value = new UnityEngine.Vector3(0, -100, 0);
        });
    }
}
