namespace FirstGodotGame.Script.State.EnemyState;

public partial class EnemyDie : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        Enemy.UpdateAnimation();
    }

    public override void Update()
    {
        base.Update();
        // 如果死亡动画还在播放，就不切换状态
        if (Enemy.AnimatedSprite2D.IsPlaying()) return;
        // 死亡动画已经播放完了， 销毁节点
        Enemy.QueueFree();
    }
}
