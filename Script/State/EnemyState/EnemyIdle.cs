namespace FirstGodotGame.Script.State.EnemyState;

public partial class EnemyIdle : Core.State
{
    public override void Update()
    {
        base.Update();
        Actor.UpdateAnimation();
    }
}
