
using FluentAssertions;
using JPLab2.Model;
using NUnit.Framework;
using UnityEngine;

public class PlayerModel_Tests
{
    [Test]
    public void IsDead_UnderPostion_Dead()
    {
        var playerM = new PlayerModel();

        playerM.IsDead.Value
           .Should().BeFalse();

        //台から落ちたらゲームオーバー
        playerM.Position.Value = new Vector3(0, -100, 0);
        playerM.IsDead.Value
            .Should().BeTrue();
    }
}
