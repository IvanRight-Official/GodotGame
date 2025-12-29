using FirstGodotGame.Script.Actor;

using Godot;

namespace FirstGodotGame.Script.Core;

/// <summary>
/// 状态抽象类
/// </summary>
[GlobalClass]
public abstract partial class State : Node
{
    /// <summary>
    /// 状态机节点
    /// </summary>
    private StateMachine _stateMachine;

    /// <summary>
    /// 设置状态机节点，必须要加上[Export]和get，才能调试时在编辑器中看到
    /// </summary>
    [Export]
    public StateMachine StateMachine
    {
        get => _stateMachine;
        set => _stateMachine = value;
    }

    /// <summary>
    /// 状态机所属的实体，使用泛型或具体类
    /// </summary>
    /// <returns></returns>
    private BaseCharacter _actor;

    /// <summary>
    /// 状态机所属的实体，必须要加上[Export]和get，才能调试时在编辑器中看到
    /// </summary>
    [Export]
    public BaseCharacter Actor
    {
        get => _actor;
        set => _actor = value;
    }

    /// <summary>
    /// 每一物理帧触发 (PhysicsProcess)
    /// </summary>
    /// <param name="delta">上次更新以来经过的时间</param>
    public virtual void UpdatePhysics(double delta)
    {
    }

    /// <summary>
    /// 更新
    /// </summary>
    public virtual void Update()
    {
        if (_stateMachine.DebugLabel != null)
            _stateMachine.DebugLabel.Text = Name + " / " + Actor.CurrentHealth;
    }

    /// <summary>
    /// 进入状态时触发
    /// </summary>
    public virtual void Enter()
    {
    }

    /// <summary>
    /// 退出状态时触发
    /// </summary>
    public virtual void Exit()
    {
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init(BaseCharacter actor)
    {
        _actor = actor;
    }
}
