namespace FirstGodotGame.Script.State.PlayerState;

public partial class PlayerIdle : Core.State
{
    public override void UpdatePhysics(double delta)
    {
        // 必须调用父类方法， 这是为了以后扩展的时候， 不需要修改代码
        base.UpdatePhysics(delta);

        // 判断是否按下了按键， 如果存在按键， 则切换状态
        if (Actor.InputDirection.Length() > 0) StateMachine.ChangeState("Run");
    }

    public override void Update()
    {
        // 必须调用父类方法， 这是为了以后扩展的时候， 不需要修改代码
        base.Update();
        Actor.UpdateAnimation();
    }
}
