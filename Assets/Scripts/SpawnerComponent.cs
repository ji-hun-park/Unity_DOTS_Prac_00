using Unity.Entities;
using Unity.Mathematics;

public struct SpawnerComponent : IComponentData
{
    public Entity Prefab;
    public int SpawnCount;
    public float2 SpawnArea;
}

