using Godot;

namespace FirstGodotGame.Script;

public partial class GameUIManager : CanvasLayer
{
    private ProgressBar _healthBar;
    private Control _gameOver;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _healthBar = GetNode<ProgressBar>("Control_HUD/ProgressBar");
        _gameOver = GetNode<Control>("Control_GameOver");

        _gameOver.Visible = false;
        _healthBar.Value = 100;

        GameManager.Instance.UpdateHealth += OnUpdateHealth;
    }

    public void OnUpdateHealth(int currentHealth, int maxHealth)
    {
        _healthBar.Value = currentHealth / (float)maxHealth * 100;
    }
}
