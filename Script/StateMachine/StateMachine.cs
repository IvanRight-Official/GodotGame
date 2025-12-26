using Godot;
using Godot.Collections;

namespace FirstGodotGame.Script.StateMachine;

/// <summary>
/// 状态机
/// </summary>
public partial class StateMachine : Node
{
    /// <summary>
    /// 初始化状态
    /// </summary>
    [Export] public State InitialState;

    /// <summary>
    /// 当前状态
    /// </summary>
    private State _currentState;

    /// <summary>
    /// 状态列表
    /// </summary>
    private readonly Dictionary<string, State> _states = new();

    public override void _Ready()
    {
        Array<Node> children = GetChildren();
        foreach (Node child in children)
            if (child is State state)
            {
                state.StateMachine = this;
                // 初始化状态
                state.Init();
            }

        _currentState = GetChild<State>(0);
        _currentState.Enter();
    }

    public override void _Process(double delta)
    {
        // 更新当前状态， 因为我们不会涉及到渲染相关的更新任务，所以不需要传delta参数
        _currentState?.Update();
    }

    public override void _PhysicsProcess(double delta)
    {
        _currentState?.UpdatePhysics(delta);
    }
}
