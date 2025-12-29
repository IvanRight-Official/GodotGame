namespace FirstGodotGame.Script.State.PlayerState;

public partial class PlayerAttack : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        Player.UpdateAnimation();
    }

    public override void Update()
    {
        base.Update();
        if (StateMachine.AnimatedSprite2D.IsPlaying()) return;

        StateMachine.ChangeState("Idle");
    }
}
