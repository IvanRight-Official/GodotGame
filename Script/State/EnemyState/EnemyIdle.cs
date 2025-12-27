using Godot;

namespace FirstGodotGame.Script.State.EnemyState;

public partial class EnemyIdle : EnemyState
{
    /// <summary>
    /// 玩家检测半径
    /// </summary>
    [Export]
    public int PlayerDetectionRadius { get; set; } = 150;

    /// <summary>
    /// 可视化视野工具， 为什么不放到 Enemy 中？
    /// 因为 Enemy.cs 的 _Ready 方法 比 EnemyState.cs 的后执行，所以只能放到这里
    /// </summary>
    private Polygon2D _polygon2D;

    public override void _Ready()
    {
        base._Ready();
        if (Enemy.EnableDebug)
            // 拿到多边形节点
            _polygon2D = Owner.GetNode<Polygon2D>("Polygon2D");
    }

    public override void Update()
    {
        base.Update();
        DetectPlayer();
    }

    public override void Enter()
    {
        base.Enter();
        if (Enemy.EnableDebug)
            // 千万要记住，只绘制一次的逻辑，一定不要写到 Update 或者 UpdatePhysics 中
            CreatePolygonCircle();
    }

    public override void Exit()
    {
        base.Exit();
        if (Enemy.EnableDebug) _polygon2D.Polygon = [];
    }

    /// <summary>
    /// 创建圆形
    /// </summary>
    private void CreatePolygonCircle()
    {
        var points = new Vector2[36];
        for (int i = 0; i < 36; i++)
        {
            float angle = Mathf.DegToRad(i * 10);
            var pointVector2 = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            pointVector2 *= PlayerDetectionRadius;
            points[i] = pointVector2;
        }

        _polygon2D.Polygon = points;
    }

    /// <summary>
    /// 检测玩家距离，并判断是否进入移动状态
    /// </summary>
    private void DetectPlayer()
    {
        if (GetEnemyAndPlayerDistance() <= PlayerDetectionRadius)
            // 玩家在附近，切换状态
            StateMachine.ChangeState("Move");
    }
}
