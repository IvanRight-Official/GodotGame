// 2025-12-27 16:01:58

using FirstGodotGame.Script.Actor;

namespace FirstGodotGame.Script.State.PlayerState;

public abstract partial class PlayerState : Core.State
{
    protected Player Player => (Player)Actor;
}
