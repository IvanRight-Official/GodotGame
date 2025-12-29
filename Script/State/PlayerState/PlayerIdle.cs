using Godot;

namespace FirstGodotGame.Script.State.PlayerState;

public partial class PlayerIdle : PlayerState
{
    public override void Update()
    {
        // 必须调用父类方法， 这是为了以后扩展的时候， 不需要修改代码
        base.Update();
        Player.UpdateAnimation();


        // 判断是否按下了按键， 如果存在按键， 则切换状态
        if (Player.InputDirection.Length() > 0) StateMachine.ChangeState("Run");

        // 检测是否按下了攻击按键
        if (Input.IsActionJustPressed("Attack")) StateMachine.ChangeState("Attack");
    }
}
