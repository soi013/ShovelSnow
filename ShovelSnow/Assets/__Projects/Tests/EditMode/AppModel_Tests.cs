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
            //�e�X�g�𑁂��I��点�邽�߁A100�{���ɐݒ肷��
            var appSpeed = new AppSpeed(GainSpeed);
            var playerM = new PlayerModel(appSpeed);
            var appM = new AppModel(playerM, appSpeed);

            //�f�t�H���g��Ԃ͏�������
            Assert.AreEqual(appM.State.Value, AppState.Initializing);

            //��������������ƃv���C��
            appM.Initialize();
            Assert.AreEqual(appM.State.Value, AppState.Playing);

            //�䂩�痎������Q�[���I�[�o�[
            playerM.Position.Value = new Vector3(0, -100, 0);
            Assert.AreEqual(appM.State.Value, AppState.GameOver);
        }

        [Test]
        public void AppState_NormalFluent_StartToEnd()
        {
            //�e�X�g�𑁂��I��点�邽�߁A100�{���ɐݒ肷��
            var appSpeed = new AppSpeed(GainSpeed);
            var playerM = new PlayerModel(appSpeed);
            var appM = new AppModel(playerM, appSpeed);

            //�f�t�H���g��Ԃ͏�������
            appM.State.Value
                .Should().Be(AppState.Initializing);

            //��������������ƃv���C��
            appM.Initialize();
            appM.State.Value
                .Should().Be(AppState.Playing);

            //�䂩�痎������Q�[���I�[�o�[
            playerM.Position.Value = new Vector3(0, -100, 0);
            appM.State.Value
                .Should().Be(AppState.GameOver);
        }

        [UnityTest]
        public IEnumerator PlayTime_10SecAfter_Over10Sec() => UniTask.ToCoroutine(async () =>
        {
            //�e�X�g�𑁂��I��点�邽�߁A100�{���ɐݒ肷��
            var appSpeed = new AppSpeed(GainSpeed);
            var playerM = new PlayerModel(appSpeed);
            var appM = new AppModel(playerM, appSpeed);

            //�f�t�H���g��Ԃ�0�b
            appM.PlayTime.Value
                .Should().Be(0f);

            //��������������J�E���g�J�n
            appM.Initialize();
            await UniTask.Delay(10_000 / GainSpeed);

            //���������P�O�b���炢�̂͂�
            appM.PlayTime.Value
                .Should().BeInRange(5f, 20f);

            var playTimeBeforeDeath = appM.PlayTime.Value;

            //�䂩�痎������Q�[���I�[�o�[
            playerM.Position.Value = new Vector3(0, -100, 0);

            await UniTask.Delay(10_000 / GainSpeed);

            //�Q�[���I�[�o�[��͎��Ԃ��o�߂��Ȃ��͂��Ȃ̂ŁA���������P�O�b���炢�̂͂�
            appM.PlayTime.Value
                .Should().BeInRange(playTimeBeforeDeath * 0.9f, playTimeBeforeDeath * 1.1f);
        });
    }
}
