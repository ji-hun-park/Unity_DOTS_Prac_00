using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public int SpawnCount = 10000;
    public Vector2 SpawnArea = new Vector2(5000, 5000);

    public class SpawnerBaker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            // 스포너 자체는 위치 변경이 필요 없으므로 None
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SpawnerComponent
            {
                // 게임 오브젝트 프리팹을 DOTS의 Entity 프리팹으로 변환
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                SpawnCount = authoring.SpawnCount,
                SpawnArea = authoring.SpawnArea
            });
        }
    }
}

