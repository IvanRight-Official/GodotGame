using Godot;

namespace FirstGodotGame.Script.Actor;

public partial class Enemy : BaseCharacter
{
    /// <summary>
    /// 目标线
    /// </summary>
    private Line2D _targetLine;

    /// <summary>
    /// 玩家
    /// </summary>
    private Player _player;

    public Player Player => _player;

    /// <summary>
    /// 玩家所属角度
    /// </summary>
    private float _playerAngle;

    public override void _Ready()
    {
        base._Ready();
        DebugModel();
        // 拿到节点数 -> 拿到根节点 -> 根据节点Path 拿到玩家节点
        _player = GetTree().Root.GetNode<Player>("SceneRoot/Level/Player");
    }

    /// <summary>
    /// 是否开启 debug 模式
    /// </summary>
    private void DebugModel()
    {
        if (EnableDebug)
            // 获取目标线， 起始节点和终结节点已经在编辑器中设置了，不需要代码设置了
            _targetLine = GetNode<Line2D>("Line2D");
        else
            GetNode<Line2D>("Line2D").Visible = false;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        // 本地缓存， 避免与 godot 交互两次
        Vector2 playerGlobalPosition = _player.GlobalPosition;
        ProcessDebugModel(playerGlobalPosition);
        // 获取玩家和敌人之间的角度
        // 使用 Godot 原生方法获取方向，不手动翻转 Y
        GetPlayerAngle(playerGlobalPosition);
    }

    /// <summary>
    /// 处理debug模式
    /// </summary>
    /// <param name="playerGlobalPosition">玩家全局位置</param>
    private void ProcessDebugModel(Vector2 playerGlobalPosition)
    {
        // 开启Debug 模式
        if (EnableDebug)
            // 更新玩家和敌人之间的连线
            _targetLine.SetPointPosition(1, ToLocal(playerGlobalPosition));
    }

    /// <summary>
    /// 获取玩家角度
    /// </summary>
    private void GetPlayerAngle(Vector2 playerGlobalPosition)
    {
        // _playerDirection = (playerGlobalPosition - GlobalPosition).Normalized();
        // _playerDirection.Y = -_playerDirection.Y;
        // _playerAngle = Mathf.RadToDeg(_playerDirection.Angle());
        // if (_playerAngle < 0) _playerAngle += 360;
        //风险： 在企业级开发中，绝对禁止手动翻转轴向，除非有极其特殊的理由。
        //Godot 所有的内置函数（如 LookAt, AngleTo, Rotation）都遵循“Y 轴向下”的规则。
        //手动翻转会导致以后集成 Godot 原生导航（Navigation）或光线检测时，逻辑出现 180 度的偏差。

        // 更好的做法： 直接使用 (playerGlobalPosition - GlobalPosition).Angle()，
        // 并接受 Godot 的角度定义（右 0°，下 90°，左 180°，上 270°/-90°）。

        // _playerDirection = (playerGlobalPosition - GlobalPosition).Normalized();
        Vector2 dir = GlobalPosition.DirectionTo(playerGlobalPosition);
        _playerAngle = Mathf.RadToDeg(dir.Angle());
        if (_playerAngle < 0) _playerAngle += 360;
    }

    protected override string GetDirection()
    {
        // string direction = _playerAngle switch
        // {
        //     > 135 and <= 225 => "Left",
        //     > 225 and <= 315 => "Down",
        //     > 315 or <= 45 => "Right",
        //     _ => "Up"
        // };
        // return direction;

        return _playerAngle switch
        {
            > 45 and <= 135 => "Down", // Godot 中 90 是下
            > 135 and <= 225 => "Left", // 180 是左
            > 225 and <= 315 => "Up", // 270 是上
            _ => "Right" // 0/360 是右
        };
    }

    // 性能低下的实现
    // private void UpdateConnectionLine()
    // {
    //     if (_player == null || _targetLine == null) return;
    //
    //     // 清除现有点
    //     _targetLine.ClearPoints();
    //
    //     // 添加敌人位置作为起点
    //     _targetLine.AddPoint(GlobalPosition);
    //
    //     // 添加玩家位置作为终点
    //     _targetLine.AddPoint(_player.GlobalPosition);
    // }
}
