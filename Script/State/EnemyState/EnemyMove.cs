using FirstGodotGame.Script.Actor;

using Godot;

namespace FirstGodotGame.Script.State.EnemyState;

public partial class EnemyMove : Core.State
{
    /// <summary>
    /// 玩家逃脱半径
    /// </summary>
    [Export]
    public int PlayerEscapeRadius { get; set; } = 250;

    public override void Update()
    {
        base.Update();
        Actor.UpdateAnimation();
        DetectPlayer();
    }

    /// <summary>
    /// 检测玩家距离，并判断是否进入移动状态
    /// </summary>
    private void DetectPlayer()
    {
        // 计算敌人和玩家之间的距离
        var enemy = (Enemy)Actor;
        float distance = enemy.GlobalPosition.DistanceTo(enemy.Player.GlobalPosition);
        if (distance > PlayerEscapeRadius)
            // 玩家在附近，切换状态
            StateMachine.ChangeState("Idle");
    }
}
