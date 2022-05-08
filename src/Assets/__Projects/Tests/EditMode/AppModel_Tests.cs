using JPLab2.Model;
using NUnit.Framework;

public class AppModel_Tests
{
    [Test]
    public void AppState_Normal_Playing()
    {
        var playerM = new PlayerModel();
        var appM = new AppModel(playerM);
        Assert.AreEqual(appM.State.Value, AppState.Initializing);

        appM.Initialize();
        Assert.AreEqual(appM.State.Value, AppState.Playing);
    }
}
