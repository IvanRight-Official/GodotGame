// 2025-12-26 20:15:51

using FirstGodotGame.Script.Actor;

namespace FirstGodotGame.Script.State.EnemyState;

public abstract partial class EnemyState : Core.State
{
    protected Enemy Enemy => (Enemy)Actor;

    /// <summary>
    /// 获取敌人和玩家之间的距离
    /// </summary>
    /// <returns>距离</returns>
    protected float GetEnemyAndPlayerDistance()
    {
        // 计算敌人和玩家之间的距离
        float distance = Enemy.GlobalPosition.DistanceTo(Enemy.Player.GlobalPosition);
        return distance;
    }
}
