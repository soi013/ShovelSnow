using Cysharp.Threading.Tasks;
using FluentAssertions;
using JPLab2.Model;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace JPLab2.Tests
{
    public class PlayerModel_Tests
    {
        [Test]
        public void IsDead_UnderPostion_Dead()
        {
            //テストを早く終わらせるため、100倍速に設定する
            var appSpeed = new AppSpeed(100);
            var playerM = new PlayerModel(appSpeed);

            playerM.IsDead.Value
               .Should().BeFalse();

            //台から落ちたらゲームオーバー
            playerM.Position.Value = new Vector3(0, -100, 0);
            playerM.IsDead.Value
                .Should().BeTrue();
        }

        [UnityTest]
        public IEnumerator Snows_Normal_IncreaseCount() => UniTask.ToCoroutine(async () =>
        {
            //テストを早く終わらせるため、100倍速に設定する
            var appSpeed = new AppSpeed(100);
            var playerM = new PlayerModel(appSpeed);

            playerM.IsDead.Value
               .Should().BeFalse();

            playerM.SnowsHit.Value = new[] { "Snow1" };

            await UniTask.Delay(100);

            playerM.IsDead.Value
               .Should().BeFalse();

            //雪が乗り過ぎたら、ゲームオーバー
            playerM.SnowsHit.Value = new[] { "Snow1", "Snow2", "Snow3", "Snow4", "Snow5" };

            await UniTask.Delay(100);

            playerM.IsDead.Value
                .Should().BeTrue();
        });

        [Test]
        public void CanMove_NotTouchingGround_False()
        {
            //テストを早く終わらせるため、100倍速に設定する
            var appSpeed = new AppSpeed(100);
            var playerM = new PlayerModel(appSpeed);

            //宙に浮いているときは移動できない
            playerM.IsTouchingGround.Value = false;
            playerM.CanMove.Value
               .Should().BeFalse();

            //地面に戻ったら移動できる
            playerM.IsTouchingGround.Value = true;
            playerM.CanMove.Value
               .Should().BeTrue();
        }
    }
}
