using Godot;

namespace FirstGodotGame.Script;

public partial class EnemyManager : Node2D
{
    private int _childCount;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _childCount = GetChildCount();

        if (_childCount == 0) ProcessMode = ProcessModeEnum.Disabled;

        ChildOrderChanged += OnChildOrderChanged;
    }

    private void OnChildOrderChanged()
    {
        if (GetChildCount() == 0) GameManager.Instance.HandleGameOver();
    }
}
