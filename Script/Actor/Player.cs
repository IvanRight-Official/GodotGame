using FirstGodotGame.Script.Envm;

using Godot;

namespace FirstGodotGame.Script.Actor;

public partial class Player : BaseCharacter
{
    public override void _UnhandledInput(InputEvent @event)
    {
        // 获取输入动作的向量值
        InputDirection = Input.GetVector(nameof(DirectionEnum.Left),
            nameof(DirectionEnum.Right), nameof(DirectionEnum.Up), nameof(DirectionEnum.Down));
    }

    /*public override void _PhysicsProcess(double delta)
    {
        // 获取输入动作的向量值
        _inputDirection = Input.GetVector(nameof(DirectionEnum.Left),
            nameof(DirectionEnum.Right), nameof(DirectionEnum.Up), nameof(DirectionEnum.Down));
        // 平滑过渡到目标速度
        // Velocity.Lerp：线性插值方法  第一个参数：_inputDirection * Speed - 目标速度向量 第二个参数：(float)(Accelerate * delta) - 插值因子
        Velocity = Velocity.Lerp(_inputDirection * Speed, (float)(Accelerate * delta));
        // 播放的动画名称
        string animationName;
        if (Velocity.Length() > 20)
            animationName = "Run_" + GetDirection();
        else
            animationName = "Idle_" + GetDirection();
        // 开始播放动画
        _animatedSprite2D.Play(animationName);
        // 执行基于 Velocity 的移动和碰撞检测
        MoveAndSlide();
    }*/

    public override void setHealth(int health)
    {
        base.setHealth(health);
        GameManager.Instance.SetHealth(CurrentHealth, MaxHealth);
    }
}
