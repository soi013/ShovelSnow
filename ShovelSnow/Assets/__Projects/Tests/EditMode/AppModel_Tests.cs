using Cysharp.Threading.Tasks;
using FluentAssertions;
using JPLab2.Model;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace JPLab2.Tests
{
    public class AppModel_Tests
    {
        private const int GainSpeed = 100;

        [Test]
        public void AppState_NormalOld_StartToEnd()
        {
            //テストを早く終わらせるため、100倍速に設定する
            var appSpeed = new AppSpeed(GainSpeed);
            var playerM = new PlayerModel(appSpeed);
            var appM = new AppModel(playerM, appSpeed);

            //デフォルト状態は初期化中
            Assert.AreEqual(appM.State.Value, AppState.Initializing);

            //初期化処理するとプレイ中
            appM.Initialize();
            Assert.AreEqual(appM.State.Value, AppState.Playing);

            //台から落ちたらゲームオーバー
            playerM.Position.Value = new Vector3(0, -100, 0);
            Assert.AreEqual(appM.State.Value, AppState.GameOver);
        }

        [Test]
        public void AppState_NormalFluent_StartToEnd()
        {
            //テストを早く終わらせるため、100倍速に設定する
            var appSpeed = new AppSpeed(GainSpeed);
            var playerM = new PlayerModel(appSpeed);
            var appM = new AppModel(playerM, appSpeed);

            //デフォルト状態は初期化中
            appM.State.Value
                .Should().Be(AppState.Initializing);

            //初期化処理するとプレイ中
            appM.Initialize();
            appM.State.Value
                .Should().Be(AppState.Playing);

            //台から落ちたらゲームオーバー
            playerM.Position.Value = new Vector3(0, -100, 0);
            appM.State.Value
                .Should().Be(AppState.GameOver);
        }

        [UnityTest]
        public IEnumerator PlayTime_10SecAfter_Over10Sec() => UniTask.ToCoroutine(async () =>
        {
            //テストを早く終わらせるため、100倍速に設定する
            var appSpeed = new AppSpeed(GainSpeed);
            var playerM = new PlayerModel(appSpeed);
            var appM = new AppModel(playerM, appSpeed);

            //デフォルト状態は0秒
            appM.PlayTime.Value
                .Should().Be(0f);

            //初期化処理するカウント開始
            appM.Initialize();
            await UniTask.Delay(10_000 / GainSpeed);

            //だいたい１０秒ぐらいのはず
            appM.PlayTime.Value
                .Should().BeInRange(5f, 20f);

            var playTimeBeforeDeath = appM.PlayTime.Value;

            //台から落ちたらゲームオーバー
            playerM.Position.Value = new Vector3(0, -100, 0);

            await UniTask.Delay(10_000 / GainSpeed);

            //ゲームオーバー後は時間が経過しないはずなので、だいたい１０秒ぐらいのはず
            appM.PlayTime.Value
                .Should().BeInRange(playTimeBeforeDeath * 0.9f, playTimeBeforeDeath * 1.1f);
        });
    }
}
