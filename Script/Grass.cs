using Godot;

namespace FirstGodotGame.Script;

public partial class Grass : Area2D
{
    /// <summary>
    /// 前景图片
    /// </summary>
    private Sprite2D _frontSprite2D;

    /// <summary>
    /// 背景图片
    /// </summary>
    private Sprite2D _backSprite2D;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // 绑定godot中Grass场景的背景图片
        _backSprite2D = GetNode<Sprite2D>("Sprite2D_Back");
        _frontSprite2D = GetNode<Sprite2D>("Sprite2D");

        // 创建补间动画器：前景图
        Tween frontTween = GetTree().CreateTween().SetLoops();
        float frontStartSkew = Mathf.DegToRad(GD.RandRange(-10, -10));
        float frontEndSkew = -frontStartSkew;
        frontTween.TweenProperty(_frontSprite2D, "skew", frontEndSkew, 1.5f).From(frontStartSkew);
        frontTween.TweenProperty(_frontSprite2D, "skew", frontStartSkew, 1.5f).From(frontEndSkew);
        frontTween.SetEase(Tween.EaseType.InOut);

        // 创建补间动画器：背景图
        float backStartSkew = frontEndSkew * 0.5f;
        float backEndSkew = -backStartSkew;
        Tween backTween = GetTree().CreateTween().SetLoops();
        backTween.TweenProperty(_backSprite2D, "skew", backEndSkew, 1.5f).From(backStartSkew);
        backTween.TweenProperty(_backSprite2D, "skew", backStartSkew, 1.5f).From(backEndSkew);
        backTween.SetEase(Tween.EaseType.InOut);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
