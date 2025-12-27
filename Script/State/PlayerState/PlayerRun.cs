using Godot;

namespace FirstGodotGame.Script.State.PlayerState;

public partial class PlayerRun : PlayerState
{
    /// <summary>
    /// 移动速度，单位为 像素/秒
    /// </summary>
    [Export] public int Speed = 150;

    /// <summary>
    /// 加速度
    /// </summary>
    private const float Accelerate = 15f;

    public override void UpdatePhysics(double delta)
    {
        base.UpdatePhysics(delta);

        // 当按键抬起时， 切换为 Idle 状态
        if (Player.InputDirection == Vector2.Zero)
        {
            StateMachine.ChangeState("Idle");
            return;
        }

        // 平滑过渡到目标速度
        // Velocity.Lerp：线性插值方法  第一个参数：_inputDirection * Speed - 目标速度向量 第二个参数：(float)(Accelerate * delta) - 插值因子
        Player.Velocity =
            Player.Velocity.Lerp(Player.InputDirection * Speed, (float)(Accelerate * delta));
        // 执行基于 Velocity 的移动和碰撞检测
        Player.MoveAndSlide();
    }

    public override void Update()
    {
        // 必须调用父类方法， 这是为了以后扩展的时候， 不需要修改代码
        base.Update();
        Player.UpdateAnimation();
    }
}
