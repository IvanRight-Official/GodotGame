namespace FirstGodotGame.Script.State.EnemyState;

public partial class EnemyHurt : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        Enemy.UpdateAnimation();
    }

    public override void Update()
    {
        base.Update();
        // 如果攻击动画还在播放，就不切换状态
        if (Enemy.AnimatedSprite2D.IsPlaying()) return;
        // 攻击动画已经播放完了， 开始切换状态
        StateMachine.ChangeState("Idle");
    }
}
