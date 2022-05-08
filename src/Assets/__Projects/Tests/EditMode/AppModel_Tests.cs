using FluentAssertions;
using JPLab2.Model;
using NUnit.Framework;
using UnityEngine;

public class AppModel_Tests
{
    [Test]
    public void AppState_NormalOld_StartToEnd()
    {
        var playerM = new PlayerModel();
        var appM = new AppModel(playerM);

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
        var playerM = new PlayerModel();
        var appM = new AppModel(playerM);

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
}
