using FirstGodotGame.Script.Actor;

using Godot;
using Godot.Collections;

namespace FirstGodotGame.Script.State.PlayerState;

public partial class PlayerAttack : PlayerState
{
    /// <summary>
    /// 攻击区域
    /// </summary>
    private Area2D _attackArea;

    /// <summary>
    /// 当前攻击区域
    /// </summary>
    private CollisionShape2D _currentAttackShape;

    /// <summary>
    /// 攻击区域集合
    /// </summary>
    private Dictionary<string, CollisionShape2D> _attackObjects = new();

    /// <summary>
    /// 攻击动画特效场景
    /// </summary>
    private PackedScene _slashScene;

    public override void Enter()
    {
        base.Enter();
        Player.UpdateAnimation();

        _currentAttackShape = _attackObjects["CollisionShape2D_" + Player.AnimationDirection];
    }

    public override void Update()
    {
        base.Update();

        int currentAttackFrame = Player.AnimatedSprite2D.Frame;
        // 攻击动画播放到第 2 帧时， 开启攻击区域 到第五帧时， 关闭攻击区域
        _currentAttackShape.Disabled = currentAttackFrame is < 2 or >= 5;

        // 如果攻击动画还在播放，就不切换状态
        if (Player.AnimatedSprite2D.IsPlaying()) return;
        // 攻击动画已经播放完了， 开始切换状态
        StateMachine.ChangeState("Idle");
    }

    public override void Init(BaseCharacter actor)
    {
        base.Init(actor);

        _attackArea = Owner.GetNode<Area2D>("Area2D_Attack_Hitbox");
        _attackArea.AreaEntered += OnAreaEntered;
        foreach (Node child in _attackArea.GetChildren())
            if (child is CollisionShape2D collisionShape2D)
            {
                collisionShape2D.Disabled = true;
                _attackObjects[child.Name] = collisionShape2D;
            }

        _slashScene = ResourceLoader.Load<PackedScene>("uid://vx7r3letihh3");
    }

    public override void Exit()
    {
        base.Exit();
        _currentAttackShape.Disabled = true;
    }

    /// <summary>
    /// 攻击碰撞检测
    /// </summary>
    /// <param name="area">碰撞者</param>
    public void OnAreaEntered(Area2D area)
    {
        // GD.Print(area.GetParent());
        Node node = area.GetParent();
        if (node is Enemy enemy)
        {
            enemy.HandleHit(Player.AttackDamage, Player.GlobalPosition);
            if (!enemy.IsDead) HandleSpawnSlash(enemy.GlobalPosition);
        }

        if (area is Grass grass) grass.HandleCut();
    }

    private void HandleSpawnSlash(Vector2 enemyPosition)
    {
        SwordSlash slash = _slashScene.Instantiate<SwordSlash>();
        slash.GlobalPosition = enemyPosition + new Vector2(0, -25);
        // 添加到全局节点
        GetTree().Root.AddChild(slash);
    }
}
