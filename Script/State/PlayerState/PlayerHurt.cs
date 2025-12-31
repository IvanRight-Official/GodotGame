using System;

using Godot;

namespace FirstGodotGame.Script.State.PlayerState;

public partial class PlayerHurt : PlayerState
{
    public override async void Enter()
    {
        try
        {
            base.Enter();
            Player.UpdateAnimation();

            // 处理无敌状态
            Player.HandleInvincible(true);
            // 无敌两秒
            await ToSignal(GetTree().CreateTimer(2), Timer.SignalName.Timeout);
            // 恢复
            Player.HandleInvincible(false);

            // 存在重复绑定
            // GetTree().CreateTimer(2).Timeout += () =>
            // {
            //     Player.HandleInvincible(false);
            // };
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }
    }

    public override void Update()
    {
        base.Update();
        // 如果受击动画还在播放，就不切换状态
        if (Player.AnimatedSprite2D.IsPlaying()) return;
        // 受击动画已经播放完了， 开始切换状态
        StateMachine.ChangeState("Idle");
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
