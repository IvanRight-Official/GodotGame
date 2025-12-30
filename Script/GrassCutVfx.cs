using Godot;

namespace FirstGodotGame.Script;

public partial class GrassCutVfx : AnimatedSprite2D
{
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (IsPlaying()) return;

        QueueFree();
    }
}
