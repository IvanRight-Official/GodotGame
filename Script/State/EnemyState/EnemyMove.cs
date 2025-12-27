using FirstGodotGame.Script.Actor;

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

    /// <summary>
    /// 移动方向（归一化方向向量）
    /// </summary>
    private Vector2 _direction;

    public override void Init(BaseCharacter actor)
    {
        base.Init(actor);
        _navigationAgent2D = Owner.GetNode<NavigationAgent2D>("NavigationAgent2D");
    }

    // public override void _Ready()
    // {
    //     base._Ready();
    //     _navigationAgent2D = Owner.GetNode<NavigationAgent2D>("NavigationAgent2D");
    // }

    public override void Update()
    {
        base.Update();
        Enemy.UpdateAnimation();
        DetectPlayer();
        MoveToPlayer();
    }

    public override void UpdatePhysics(double delta)
    {
        base.UpdatePhysics(delta);
        // Enemy.GlobalPosition: 获取敌人的全局世界坐标位置
        //_navigationAgent2D.GetNextPathPosition(): 获取导航系统计算出的下一个路径点的位置
        //DirectionTo(): Godot引擎的内置方法，计算从当前位置到目标位置的单位方向向量
        //实际作用
        //计算敌人应该朝哪个方向移动才能到达下一个导航路径点
        //将结果存储在 _direction 字段中，供物理更新时使用
        //这个方向向量通常会在 UpdatePhysics 方法中用于实际的移动逻辑
        _direction = Enemy.GlobalPosition.DirectionTo(_navigationAgent2D.GetNextPathPosition());
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
