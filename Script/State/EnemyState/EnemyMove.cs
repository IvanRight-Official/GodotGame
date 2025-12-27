using FirstGodotGame.Script.Actor;

using Godot;

namespace FirstGodotGame.Script.State.EnemyState;

public partial class EnemyMove : EnemyState
{
    /// <summary>
    /// 移动速度，单位为 像素/秒
    /// </summary>
    [Export]
    public int Speed { get; set; } = 50;

    /// <summary>
    /// 加速度
    /// </summary>
    private const float Accelerate = 15f;

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

    /// <summary>
    /// 定时器
    /// </summary>
    private Timer _timerUpdatePlayerPosition;

    public override void Init(BaseCharacter actor)
    {
        base.Init(actor);
        _navigationAgent2D = Owner.GetNode<NavigationAgent2D>("NavigationAgent2D");
        _navigationAgent2D.VelocityComputed += OnVelocityComputed;
        _timerUpdatePlayerPosition = Owner.GetNode<Timer>("Timer_UpdatePlayerPosition");
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
        // 将检测玩家距离和移动到玩家放到定时器中执行，避免CPU压力过大
        // DetectPlayer();
        // TargetPlayer();
    }

    public override void UpdatePhysics(double delta)
    {
        base.UpdatePhysics(delta);

        // Do not query when the map has never synchronized and is empty.
        if (NavigationServer2D.MapGetIterationId(_navigationAgent2D.GetNavigationMap()) ==
            0) return;

        // 1. 必须要先获取导航系统计算出的下一个路径点位置， 否则 IsTargetReached() 不会生效
        Vector2 nextPathPosition = _navigationAgent2D.GetNextPathPosition();
        // 2. 如果已经到达目标位置，则不需要再移动
        if (_navigationAgent2D.IsTargetReached()) return;
        // Enemy.GlobalPosition: 获取敌人的全局世界坐标位置
        //_navigationAgent2D.GetNextPathPosition(): 获取导航系统计算出的下一个路径点的位置
        //DirectionTo(): Godot引擎的内置方法，计算从当前位置到目标位置的单位方向向量
        // 3. 实际作用
        //计算敌人应该朝哪个方向移动才能到达下一个导航路径点
        //将结果存储在 _direction 字段中，供物理更新时使用
        //这个方向向量通常会在 UpdatePhysics 方法中用于实际的移动逻辑
        _direction = Enemy.GlobalPosition.DirectionTo(nextPathPosition);
        // Enemy.Velocity = Enemy.Velocity.Lerp(_direction * Speed, (float)delta);
        // Enemy.MoveAndSlide();
        // 4. 计算“期望速度” (注意：这里不要直接 MoveAndSlide)
        Vector2 desiredVelocity = _direction * Speed;

        // 5. 提交给避障系统（这会触发 VelocityComputed 信号） 避障开启后， 会
        if (_navigationAgent2D.AvoidanceEnabled)
        {
            // 不需要 delta 参与运算， 在回调中已经处理好了
            _navigationAgent2D.Velocity = desiredVelocity;
        }
        else
        {
            // 如果没开避障，则手动处理
            Enemy.Velocity = Enemy.Velocity.Lerp(desiredVelocity, (float)delta);
            Enemy.MoveAndSlide();
        }
    }

    public override void Enter()
    {
        base.Enter();
        _timerUpdatePlayerPosition.Start();
        // 立即寻路一次，消除第一帧延迟
        TargetPlayer();
    }

    public override void Exit()
    {
        base.Exit();
        _timerUpdatePlayerPosition.Stop();
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
    /// 指定目标位置为玩家
    /// </summary>
    private void TargetPlayer()
    {
        _navigationAgent2D.TargetPosition = Enemy.Player.GlobalPosition;
    }

    /// <summary>
    /// 定时器：<see cref="_timerUpdatePlayerPosition"/> 的 Timeout 的信号监听
    /// 默认 0.5 秒触发一次
    /// </summary>
    public void OnTimerUpdatePlayerPositionTimeout()
    {
        // 检测玩家距离和移动到玩家 改为 定时器触发， 不再 Update() 中频繁执行， 减轻CPU 压力
        DetectPlayer();
        TargetPlayer();
    }

    /// <summary>
    /// 避障行动
    /// </summary>
    /// <param name="safeVelocity"></param>
    public void OnVelocityComputed(Vector2 safeVelocity)
    {
        // 只有在当前移动状态才处理回调
        if (StateMachine.CurrentState != this) return;

        // 直接使用避障计算出的安全速度
        // [严谨] 不要用 +=，不要乘 delta，直接赋值
        Enemy.Velocity = safeVelocity;

        // 在这里执行物理位移
        Enemy.MoveAndSlide();
    }
    // 有问题的实现，详解doc
    // public void OnNavigationAgent2dVelocityComputed(Vector2 safeVelocity)
    // {
    //     if (StateMachine.CurrentState == this && !_navigationAgent2D.IsTargetReached())
    //         Enemy.Velocity += safeVelocity * (float)GetPhysicsProcessDeltaTime();
    // }
}
