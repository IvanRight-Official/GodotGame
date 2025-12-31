using Godot;

namespace FirstGodotGame.Script.State.PlayerState;

public partial class PlayerDie : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        Player.UpdateAnimation();
    }

    public override void Update()
    {
        base.Update();
        if (Player.AnimatedSprite2D.IsPlaying()) return;
        // QueueFree();
    }

    public override void UpdatePhysics(double delta)
    {
        base.UpdatePhysics(delta);
        if (Player.AnimatedSprite2D.Frame == 0)
            Player.MoveAndCollide(Player.HurtDirection * 300 * (float)delta);
    }

    public override void Exit()
    {
        base.Exit();
        Player.HurtDirection = Vector2.Zero;
    }
}
