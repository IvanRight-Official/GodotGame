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
    /// key： 状态名称
    /// </summary>
    private readonly Dictionary<string, State> _states = new();

    /// <summary>
    /// 用于调试使用的标签
    /// </summary>
    public Label DebugLabel;

    public override void _Ready()
    {
        DebugLabel = GetParent().GetNode<Label>("DebugLabel");
        // 获取所有子节点，即状态节点
        Array<Node> children = GetChildren();
        foreach (Node child in children)
            if (child is State state)
            {
                state.StateMachine = this;
                _states.Add(state.Name, state);
                // 状态初始化
                state.Init();
            }

        // _currentState = GetChild<State>(0);
        // _currentState.Enter();
        ChangeState(InitialState?.Name);
    }

    public override void _Process(double delta)
    {
        // 更新当前状态， 因为我们不会涉及到渲染相关的更新任务，所以不需要传delta参数
        _currentState?.Update();
        // 如果按下了空格键，则切换到Run状态， 用来debug使用
        // if (Input.IsPhysicalKeyPressed(Key.Space)) ChangeState("Run");
    }

    public override void _PhysicsProcess(double delta)
    {
        _currentState?.UpdatePhysics(delta);
    }

    /// <summary>
    /// 更改状态
    /// </summary>
    /// <param name="stateName">状态名称</param>
    public void ChangeState(string stateName)
    {
        if (!_states.TryGetValue(stateName, out State value))
        {
            GD.PrintErr($"State '{stateName}' not found.");
            return;
        }

        _currentState?.Exit();
        _currentState = value;
        _currentState.Enter();
    }
}
