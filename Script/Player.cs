using Godot;

using FirstGodotGame.Script.Envm;

namespace FirstGodotGame.Script;

public partial class Player : CharacterBody2D
{
    /// <summary>
    /// 移动速度，单位为 像素/秒
    /// </summary>
    [Export] public int Speed = 150;

    /// <summary>
    /// 加速度
    /// </summary>
    private const float Accelerate = 15f;

    /// <summary>
    /// 存储当前输入方向
    /// </summary>
    private Vector2 _inputDirection = Vector2.Zero;

    /// <summary>
    /// 动画节点
    /// </summary>
    private AnimatedSprite2D _animatedSprite2D;

    /// <summary>
    /// 动画方向
    /// </summary>
    private string _animationDirection;

    public override void _Ready()
    {
        // https://docs.godotengine.org/zh-cn/4.4/tutorials/scripting/c_sharp/c_sharp_differences.html#onready-annotation
        // 绑定godot中Player场景的动画节点
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        // 获取输入动作的向量值
        _inputDirection = Input.GetVector(nameof(DirectionEnum.Left),
            nameof(DirectionEnum.Right), nameof(DirectionEnum.Up), nameof(DirectionEnum.Down));
        // 平滑过渡到目标速度
        // Velocity.Lerp：线性插值方法  第一个参数：_inputDirection * Speed - 目标速度向量 第二个参数：(float)(Accelerate * delta) - 插值因子
        Velocity = Velocity.Lerp(_inputDirection * Speed, (float)(Accelerate * delta));
        // 播放的动画名称
        string animationName;
        if (Velocity.Length() > 20)
            animationName = "Run_" + GetDirection();
        else
            animationName = "Idle_" + GetDirection();
        // 开始播放动画
        _animatedSprite2D.Play(animationName);
        // 执行基于 Velocity 的移动和碰撞检测
        MoveAndSlide();
    }

    /// <summary>
    /// 获取方向
    /// </summary>
    /// <returns>返回当前或上一次的方向</returns>
    private string GetDirection()
    {
        if (_inputDirection == Vector2.Zero) return _animationDirection;
        _animationDirection = _inputDirection.X > 0 ? nameof(DirectionEnum.Right) :
            _inputDirection.X < 0 ? nameof(DirectionEnum.Left) :
            _inputDirection.Y > 0 ? nameof(DirectionEnum.Down) : nameof(DirectionEnum.Up);
        return _animationDirection;
    }
}
