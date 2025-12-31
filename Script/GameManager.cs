using Godot;

namespace FirstGodotGame.Script;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }

    [Signal]
    public delegate void UpdateHealthEventHandler(int currentHealth, int maxHealth);

    public readonly string SignalUpdateHealth = "UpdateHealth";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
    }

    public void SetHealth(int currentHealth, int maxHealth)
    {
        EmitSignal(SignalName.UpdateHealth, currentHealth, maxHealth);
    }
}
