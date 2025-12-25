using Godot;
using Godot.Collections;

namespace FirstGodotGame.Script;

public partial class GrassGeneratorTileMapLayer : TileMapLayer
{
    /// <summary>
    /// 引入 Grass.tscn
    /// </summary>
    [Export]
    public PackedScene GrassScene { get; set; }

    /// <summary>
    /// 草地的偏移量
    /// </summary>
    [Export]
    public int OffSet { get; set; } = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // 将 用于绘制草地层的 TileMapLayer 设置为不可见
        Visible = false;
        // 获取所有已使用的单元格
        Array<Vector2I> usedCells = GetUsedCells();
        // 遍历所有已使用的单元格
        foreach (Vector2I cell in usedCells)
        {
            // 1. 使用内置方法获取图块在 Layer 本地的中心位置坐标
            // 这会自动考虑 TileSet 设定的图块大小 （16x16，32x32等）
            Vector2 localPosition = MapToLocal(cell);
            Grass grass = GrassScene.Instantiate<Grass>();
            // 2. 将本地坐标转换为全局坐标
            grass.GlobalPosition = ToGlobal(localPosition);
            // 设置 grass 的全局位置
            // grass.GlobalPosition = GlobalPosition + cell * 32 + new Vector2(16, 16);
            // 将草地挂载到上一级的Level节点下
            // 3. 异步挂载到父节点（通常是开启了 Y-Sort 的容器）
            GetParent().CallDeferred(Node.MethodName.AddChild, grass);
        }

        // 4. 重要： 任务完成后销毁这个“生成器层”，以释放不必要的内存
        QueueFree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
