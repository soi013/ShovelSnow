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
        var playerM = new PlayerModel();
        var appM = new AppModel(playerM);

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
}
