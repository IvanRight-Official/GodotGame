using FirstGodotGame.Script.Envm;

using Godot;

namespace FirstGodotGame.Script.Actor;

public partial class BaseCharacter : CharacterBody2D
{
    /// <summary>
    /// 存储当前输入方向
    /// </summary>
    private Vector2 _inputDirection = Vector2.Zero;

    /// <summary>
    /// 存储当前输入方向
    /// </summary>
    protected Vector2 InputDirection
    {
        get => _inputDirection;
        set => _inputDirection = value;
    }

    /// <summary>
    /// 动画节点
    /// </summary>
    private AnimatedSprite2D _animatedSprite2D;

    /// <summary>
    /// 动画方向
    /// </summary>
    private string _animationDirection = nameof(DirectionEnum.Down);

    public override void _Ready()
    {
        // https://docs.godotengine.org/zh-cn/4.4/tutorials/scripting/c_sharp/c_sharp_differences.html#onready-annotation
        // 绑定godot中Player场景的动画节点
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
    }

    /// <summary>
    /// 获取方向
    /// </summary>
    /// <returns>返回当前或上一次的方向</returns>
    protected string GetDirection()
    {
        if (_inputDirection == Vector2.Zero) return _animationDirection;
        _animationDirection = _inputDirection.X > 0 ? nameof(DirectionEnum.Right) :
            _inputDirection.X < 0 ? nameof(DirectionEnum.Left) :
            _inputDirection.Y > 0 ? nameof(DirectionEnum.Down) : nameof(DirectionEnum.Up);
        return _animationDirection;
    }
}
