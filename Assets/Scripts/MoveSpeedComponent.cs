using Unity.Entities;
using Unity.Mathematics;

public struct MoveSpeedComponent : IComponentData
{
    public float Value;
    public float3 Direction;
}

