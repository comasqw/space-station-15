using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Shared._Internal_Abyss.Fauna.MegaFauna.Ahs_Drake;

[RegisterComponent]
public sealed partial class DrakeArenaComponent : Component
{
    [DataField]
    public EntProtoId ArenaLava = "IAAshDrakeArenaPreLava";

    [DataField]
    public EntProtoId GreenZone = "IAAshDrakeArenaGreenZone";

    [DataField]
    public EntProtoId ArenaWall = "IAAshDrakeArenaWall";

    /// <summary>
    /// Square size.
    /// </summary>
    [DataField]
    public int Size = 2;

    [DataField]
    public int Repeit = 3;

    [DataField]
    public List<MapCoordinates> CoordsToSpawnLava = new();

    [DataField]
    public TimeSpan Delay = TimeSpan.FromSeconds(6);

    [DataField]
    public TimeSpan Cooldown = TimeSpan.Zero;
}
