using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

// [UpdateBefore] or [UpdateAfter] can be used here if needed
[BurstCompile]
public partial struct MoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // System only updates when at least one entity has MoveSpeedComponent
        state.RequireForUpdate<MoveSpeedComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        // Schedule the job on worker threads
        new MoveJob
        {
            DeltaTime = deltaTime
        }.ScheduleParallel();
    }

    // IJobEntity automatically iterates over all entities that match the method arguments
    [BurstCompile]
    public partial struct MoveJob : IJobEntity
    {
        public float DeltaTime;

        // ref LocalTransform means we have read/write access.
        // in MoveSpeedComponent means read-only access.
        public void Execute(ref LocalTransform transform, in MoveSpeedComponent moveSpeed)
        {
            transform = transform.Translate(moveSpeed.Direction * moveSpeed.Value * DeltaTime);
        }
    }
}

