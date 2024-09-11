using Content.Shared._Internal_Abyss.Map;
using Robust.Shared.Map;
using Robust.Shared.Random;
using Robust.Shared.Network;
using Robust.Shared.Timing;

namespace Content.Shared._Internal_Abyss.Fauna.MegaFauna.Ahs_Drake;

public sealed class DrakeArenaSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IAMapSystem _iaMap = default!;
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly IGameTiming _timing = default!;


    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<DrakeArenaAction>(OnSummonAction);
    }

    private void OnSummonAction(DrakeArenaAction args)
    {
        if (args.Handled || args.Coords is not { } coords)
            return;

        if (args.Coords is null)
            return;

        if (!TryComp<DrakeArenaComponent>(args.Performer, out var arenaComp))
            return;

        List<MapCoordinates> coordsToSpawnWalls = _iaMap.CreateCoordinatesSqaure(args.Coords.Value, arenaComp.Size + 1, false);
        foreach (var wallCoords in coordsToSpawnWalls)
        {
            if (_net.IsServer)
                Spawn(arenaComp.ArenaWall, wallCoords);
        }

        arenaComp.CoordsToSpawnLava = _iaMap.CreateCoordinatesSqaure(args.Coords.Value, arenaComp.Size);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<DrakeArenaComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.CoordsToSpawnLava.Count == 0)
                continue;

            for (int rep = 0; rep < comp.Repeit; rep++)
            {
                if (comp.Cooldown != TimeSpan.Zero && _timing.CurTime + comp.Delay < comp.Cooldown)
                    continue;

                int randomPosInt = _random.Next(comp.CoordsToSpawnLava.Count);
                for (int i = 0; i < comp.CoordsToSpawnLava.Count; i++)
                {
                    if (_net.IsServer)
                    {
                        if (randomPosInt == i)
                        {
                            Spawn(comp.GreenZone, comp.CoordsToSpawnLava[i]);
                            continue;
                        }

                        Spawn(comp.ArenaLava, comp.CoordsToSpawnLava[i]);
                    }
                }
                comp.Cooldown = _timing.CurTime + comp.Delay;
            }
            comp.CoordsToSpawnLava = new List<MapCoordinates>();
            comp.Cooldown = TimeSpan.Zero;
        }
    }
}
