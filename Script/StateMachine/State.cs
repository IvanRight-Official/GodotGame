using Godot;

namespace FirstGodotGame.Script.StateMachine;

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
    /// 每一物理帧触发 (PhysicsProcess)
    /// </summary>
    /// <param name="delta">上次更新以来经过的时间</param>
    protected virtual void UpdatePhysics(double delta)
    {
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected virtual void Update()
    {
    }

    /// <summary>
    /// 进入状态时触发
    /// </summary>
    protected virtual void Enter()
    {
    }

    /// <summary>
    /// 退出状态时触发
    /// </summary>
    protected virtual void Exit()
    {
    }

    /// <summary>
    /// 准备
    /// </summary>
    protected virtual void IsReady()
    {
    }
}
