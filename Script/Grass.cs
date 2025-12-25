using Godot;

namespace FirstGodotGame.Script;

public partial class Grass : Area2D
{
    /// <summary>
    /// 前景图片
    /// </summary>
    private Sprite2D _frontSprite2D;

    /// <summary>
    /// 暴露前景图片的get方法
    /// 惰性加载模式
    /// </summary>
    public Sprite2D FrontSprite2D => _frontSprite2D ??= GetNode<Sprite2D>("Sprite2D");

    /// <summary>
    /// 背景图片
    /// </summary>
    private Sprite2D _backSprite2D;

    /// <summary>
    /// 暴露背景图片的get方法
    /// 惰性加载模式
    /// </summary>
    public Sprite2D BackSprite2D => _backSprite2D ??= GetNode<Sprite2D>("Sprite2D_Back");

    /// <summary>
    /// 进入草地的动画
    /// </summary>
    private Tween _frontTween;

    /// <summary>
    /// 前景图片缩放
    /// </summary>
    private Vector2 _frontEnterScale = new(1f, 0.5f);

    /// <summary>
    /// 前景图片恢复
    /// </summary>
    private Vector2 _frontLeaveScale = new(1f, 1f);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // 关闭 Process 回调， 提高性能
        SetProcess(false);

        // 绑定godot中Grass场景的背景图片
        _backSprite2D ??= GetNode<Sprite2D>("Sprite2D_Back");
        _frontSprite2D ??= GetNode<Sprite2D>("Sprite2D");

        // 创建补间动画器：前景图
        Tween frontTween = GetTree().CreateTween().SetLoops();
        float frontStartSkew = Mathf.DegToRad(GD.RandRange(-10, 10));
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

    /// <summary>
    /// 进入检测事件
    /// </summary>
    /// <param name="body">进入的对象</param>
    public void OnBodyEntered(Node2D body)
    {
        // GD.Print($"{body.Name} 进入了 grass");
        CreateNewGrassTween(_frontEnterScale, 0.1f);
    }

    /// <summary>
    /// 离开检测事件
    /// </summary>
    /// <param name="body">离开的对象</param>
    public void OnBodyExited(Node2D body)
    {
        // GD.Print($"{body.Name} 离开了 grass");
        CreateNewGrassTween(_frontLeaveScale, 0.5f);
    }

    /// <summary>
    /// 创建新的草地补间动画
    /// </summary>
    /// <param name="targetScale">目标尺寸</param>
    /// <param name="duration">持续时间</param>
    private void CreateNewGrassTween(Vector2 targetScale, float duration)
    {
        if (_frontTween != null) _frontTween.Kill();

        _frontTween = GetTree().CreateTween();
        _frontTween.TweenProperty(_frontSprite2D, "scale", targetScale, duration)
            .SetEase(Tween.EaseType.Out);
    }
}
