using Godot;

namespace FirstGodotGame.Script.State.EnemyState;

public partial class EnemyMove : EnemyState
{
    /// <summary>
    /// 玩家逃脱半径
    /// </summary>
    [Export]
    public int PlayerEscapeRadius { get; set; } = 250;

    /// <summary>
    /// 导航 代理
    /// </summary>
    private NavigationAgent2D _navigationAgent2D;

    public override void _Ready()
    {
        base._Ready();
        _navigationAgent2D = Owner.GetNode<NavigationAgent2D>("NavigationAgent2D");
    }

    public override void Update()
    {
        base.Update();
        Enemy.UpdateAnimation();
        DetectPlayer();
        MoveToPlayer();
    }


    /// <summary>
    /// 检测玩家距离，并判断是否进入移动状态
    /// </summary>
    private void DetectPlayer()
    {
        // 计算敌人和玩家之间的距离
        if (GetEnemyAndPlayerDistance() > PlayerEscapeRadius)
            // 玩家不在附近，切换状态
            StateMachine.ChangeState("Idle");
    }

    /// <summary>
    /// 朝玩家移动
    /// </summary>
    private void MoveToPlayer()
    {
        _navigationAgent2D.TargetPosition = Enemy.Player.GlobalPosition;
    }
}
