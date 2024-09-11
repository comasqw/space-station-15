using Robust.Shared.Map;

namespace Content.Shared._Internal_Abyss.Map;

public abstract partial class IAMapSystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    public List<MapCoordinates> CreateCoordinatesSqaure(EntityCoordinates user, int size, bool fill = true)
    {
        List<MapCoordinates> coordinatesSquare = new();
        for (int y = -size; y <= size; y++)
        {
            for (int x = -size; x <= size; x++)
            {
                if (fill || (x == -size || x == size || y == -size || y == size))
                {
                    EntityCoordinates newCoord = new EntityCoordinates(
                    user.EntityId,
                    user.Position.X + (float)x,
                    user.Position.Y + (float)y
                );
                    MapCoordinates newMapCoords = _transform.ToMapCoordinates(newCoord);
                    coordinatesSquare.Add(newMapCoords);
                }
            }
        }
        return coordinatesSquare;
    }
}
