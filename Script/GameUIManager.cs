using Godot;

namespace FirstGodotGame.Script;

public partial class GameUIManager : CanvasLayer
{
    private ProgressBar _healthBar;
    private Control _gameOver;
    private Button _restart;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _healthBar = GetNode<ProgressBar>("Control_HUD/ProgressBar");
        _gameOver = GetNode<Control>("Control_GameOver");
        _restart = GetNode<Button>("Control_GameOver/Button_Restart");

        _restart.Pressed += OnGameRestart;

        _gameOver.Visible = false;
        _healthBar.Value = 100;

        GameManager.Instance.UpdateHealth += OnUpdateHealth;
        GameManager.Instance.GameOver += OnGameOver;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        GameManager.Instance.UpdateHealth -= OnUpdateHealth;
        GameManager.Instance.GameOver -= OnGameOver;
    }

    private void OnGameRestart()
    {
        GetTree().ReloadCurrentScene();
    }

    public void OnUpdateHealth(int currentHealth, int maxHealth)
    {
        _healthBar.Value = currentHealth / (float)maxHealth * 100;
    }

    public void OnGameOver()
    {
        _gameOver.Visible = true;
    }
}
