using FirstGodotGame.Script.Actor;

using Godot;

namespace FirstGodotGame.Script.State.EnemyState;

public partial class EnemyIdle : Core.State
{
    /// <summary>
    /// 玩家检测半径
    /// </summary>
    [Export]
    public int PlayerDetectionRadius { get; set; } = 150;


    public override void Update()
    {
        base.Update();
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
        if (distance <= PlayerDetectionRadius)
            // 玩家在附近，切换状态
            StateMachine.ChangeState("Move");
    }
}
