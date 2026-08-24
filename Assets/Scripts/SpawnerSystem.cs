using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // SpawnerComponent가 있는 Entity가 있을 때만 시스템 실행
        state.RequireForUpdate<SpawnerComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Entity 생성을 안전하게 처리하기 위해 EntityCommandBuffer 사용 (동일 프레임 내 처리이므로 Temp 할당)
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        // 랜덤 생성기 초기화 (시드값을 고정하거나 시간에 따라 변경 가능)
        var random = Unity.Mathematics.Random.CreateFromIndex(1234);

        // 모든 스포너 컴포넌트를 순회합니다. (WithEntityAccess를 통해 해당 Entity ID도 가져옵니다)
        foreach (var (spawner, entity) in SystemAPI.Query<RefRO<SpawnerComponent>>().WithEntityAccess())
        {
            var spawnCount = spawner.ValueRO.SpawnCount;
            var spawnArea = spawner.ValueRO.SpawnArea;
            var prefab = spawner.ValueRO.Prefab;

            // 한 번에 대량의 Entity를 인스턴스화하여 배열로 받음
            var instances = new NativeArray<Entity>(spawnCount, Allocator.Temp);
            ecb.Instantiate(prefab, instances);

            for (int i = 0; i < spawnCount; i++)
            {
                // 지정된 영역 내에서 무작위 위치 계산
                float x = random.NextFloat(-spawnArea.x / 2f, spawnArea.x / 2f);
                float z = random.NextFloat(-spawnArea.y / 2f, spawnArea.y / 2f);
                var position = new float3(x, 0, z);

                // 생성된 Entity의 LocalTransform 컴포넌트를 덮어씌워 위치 설정
                ecb.SetComponent(instances[i], LocalTransform.FromPosition(position));
            }

            instances.Dispose();

            // 생성은 한 번만 실행되도록 스포너 Entity를 삭제합니다.
            ecb.DestroyEntity(entity);
        }

        // 커맨드 버퍼에 쌓인 명령(Instantiate, SetComponent, DestroyEntity 등)을 실제 World에 반영
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

