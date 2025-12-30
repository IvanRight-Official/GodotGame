using FirstGodotGame.Script.Core;
using FirstGodotGame.Script.Envm;

using Godot;

namespace FirstGodotGame.Script.Actor;

[GlobalClass]
public abstract partial class BaseCharacter : CharacterBody2D
{
    /// <summary>
    /// 是否开启debug模式
    /// </summary>
    [Export]
    public bool EnableDebug { get; set; } = true;

    /// <summary>
    /// 最大生命值
    /// </summary>
    [Export]
    public int MaxHealth { get; set; } = 100;

    /// <summary>
    /// 攻击伤害
    /// </summary>
    [Export]
    public int AttackDamage { get; set; } = 50;

    /// <summary>
    /// 是否死亡
    /// </summary>
    private bool _isDead = false;

    public bool IsDead => _isDead;

    private int _currentHealth;

    /// <summary>
    /// 当前生命值
    /// </summary>
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            if (_currentHealth == 0) _isDead = true;
        }
    }

    /// <summary>
    /// 存储当前输入方向
    /// </summary>
    private Vector2 _inputDirection = Vector2.Zero;

    /// <summary>
    /// 存储当前输入方向， 如果要在状态对象中获取，必须修改为public
    /// </summary>
    public Vector2 InputDirection
    {
        get => _inputDirection;
        set => _inputDirection = value;
    }

    /// <summary>
    /// 动画节点
    /// </summary>
    private AnimatedSprite2D _animatedSprite2D;

    public AnimatedSprite2D AnimatedSprite2D => _animatedSprite2D;

    private StateMachine _stateMachine;

    /// <summary>
    /// 动画方向
    /// </summary>
    private string _animationDirection = nameof(DirectionEnum.Down);

    public string AnimationDirection => _animationDirection;


    /// <summary>
    /// 受击方向
    /// </summary>
    public Vector2 HurtDirection { get; set; }

    public override void _Ready()
    {
        // https://docs.godotengine.org/zh-cn/4.4/tutorials/scripting/c_sharp/c_sharp_differences.html#onready-annotation
        // 绑定godot中Player场景的动画节点
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _stateMachine = GetNode<StateMachine>("StateMachine");

        // 初始化生命值为最大值
        _currentHealth = MaxHealth;
    }

    /// <summary>
    /// 获取方向
    /// </summary>
    /// <returns>返回当前或上一次的方向</returns>
    protected virtual string GetDirection()
    {
        if (_inputDirection == Vector2.Zero) return _animationDirection;
        _animationDirection = _inputDirection.X > 0 ? nameof(DirectionEnum.Right) :
            _inputDirection.X < 0 ? nameof(DirectionEnum.Left) :
            _inputDirection.Y > 0 ? nameof(DirectionEnum.Down) : nameof(DirectionEnum.Up);
        return _animationDirection;
    }

    /// <summary>
    /// 更新动画
    /// </summary>
    public void UpdateAnimation()
    {
        _animatedSprite2D.Play(_stateMachine.CurrentState.Name + "_" + GetDirection());
    }

    /// <summary>
    /// 处理受击
    /// </summary>
    /// <param name="damage">伤害</param>
    /// <param name="hitPosition"></param>
    ///
    public void HandleHit(int damage, Vector2 hitPosition = default)
    {
        if (_isDead) return;
        // 添加受击闪烁动画
        StartBlink();
        // 减去伤害
        CurrentHealth -= damage;
        // 受击方向
        HurtDirection = (GlobalPosition - hitPosition).Normalized();
        // 判定是否死亡
        if (_isDead)
            _stateMachine.ChangeState("Die");
        else
            _stateMachine.ChangeState("Hurt");
    }

    /// <summary>
    /// 处理闪烁
    /// </summary>
    /// <param name="mixWeight">混合权重</param>
    private void HandleBlink(float mixWeight)
    {
        AnimatedSprite2D.SetInstanceShaderParameter("Blink", mixWeight);
    }

    private void StartBlink()
    {
        Tween blinkTween = GetTree().CreateTween();
        blinkTween.TweenMethod(Callable.From<float>(HandleBlink), 1.0, 0.0, 0.3);
    }
}
