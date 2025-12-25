using Godot;
using Godot.Collections;

namespace FirstGodotGame.Script;

public partial class RandomFrontGrass : Sprite2D
{
    /// <summary>
    /// 前景草地图片数组
    /// </summary>
    [Export] public Array<Rect2> FrontGrassRectArray;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetProcess(false);

        if (FrontGrassRectArray?.Count > 0)
        {
            // 随机获取一个索引
            int randomIndex = GD.RandRange(0, FrontGrassRectArray.Count - 1);
            // 设置该区域的 图片
            RegionRect = FrontGrassRectArray[randomIndex];
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
