using Godot;
using Godot.Collections;

namespace FirstGodotGame.Script.Util;

[Tool]
public partial class RandomRegion : Sprite2D
{
    /// <summary>
    /// 随机区域数组，包含：
    /// 前景草地图片数组、树木图片数组等
    /// </summary>
    [Export] public Array<Rect2> RandomRectArray;

    /// <summary>
    /// 当前随机对象的种子
    /// </summary>
    private ulong _seed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetProcess(false);
        UpdateRandomRegion();
    }

    private void UpdateRandomRegion()
    {
        if (RandomRectArray?.Count > 0)
        {
            // 通过坐标设置为种子
            _seed = (ulong)(GlobalPosition.X + GlobalPosition.Y);
            GD.Seed(_seed);
            // 随机获取一个索引
            int randomIndex = GD.RandRange(0, RandomRectArray.Count - 1);
            // 设置该区域的 图片
            RegionRect = RandomRectArray[randomIndex];
        }
    }

    public override void _EnterTree()
    {
        SetNotifyTransform(true);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationTransformChanged)
            // 在编辑器模式中更新随机区域
            if (Engine.IsEditorHint())
                UpdateRandomRegion();
    }
}
