using System.Linq;

using FirstGodotGame.Script.Actor;

using Godot;
using Godot.Collections;

namespace FirstGodotGame.Script.Core;

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

    public State CurrentState => _currentState;

    /// <summary>
    /// 状态列表
    /// （
    /// 这里存在命名空间冲突， Godot.Collections和System.Collections.Generic都有Dictionary
    /// 但是 _states 不会和 godot 的api进行交互 所以使用.net的 Dictionary
    /// ）
    /// key： 状态名称
    /// </summary>
    private readonly System.Collections.Generic.Dictionary<string, State> _states = new();

    /// <summary>
    /// 用于调试使用的标签
    /// </summary>
    public Label DebugLabel { get; set; }

    public override void _Ready()
    {
        DebugLabel = Owner.GetNode<Label>("DebugLabel");
        // 获取所有子节点，即状态节点 这里使用 Godot.Collections.Array 是因为与 godot 的api进行交互
        Array<Node> children = GetChildren();
        foreach (Node child in children)
            if (child is State state)
            {
                state.StateMachine = this;
                _states.Add(state.Name, state);
                // 状态初始化
                state.Init(GetParent<BaseCharacter>());
            }

        // 默认进入第一个状态
        if (_states.Count > 0) InitialState ??= _states.Values.First();

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
