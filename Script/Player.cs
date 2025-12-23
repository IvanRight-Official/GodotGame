using Godot;
using System;

namespace FirstGodotGame.Script
{
	public partial class Player : CharacterBody2D
	{

        /// <summary>
        /// 移动速度，单位为 像素/秒
        /// </summary>
		private const float Speed = 150;
        /// <summary>
        /// 加速度
        /// </summary>
        private const float Accelerate = 15f;
        /// <summary>
        /// 存储当前输入方向
        /// </summary>
        private Vector2 _inputDirection = Vector2.Zero;

		public override void _PhysicsProcess(double delta)
		{
            // 获取输入动作的向量值
            _inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
            // 平滑过渡到目标速度
            // Velocity.Lerp：线性插值方法  第一个参数：_inputDirection * Speed - 目标速度向量 第二个参数：(float)(Accelerate * delta) - 插值因子
			Velocity = Velocity.Lerp(_inputDirection * Speed, (float)(Accelerate * delta));
            // 执行基于 Velocity 的移动和碰撞检测
			MoveAndSlide();
		}
	}

}
