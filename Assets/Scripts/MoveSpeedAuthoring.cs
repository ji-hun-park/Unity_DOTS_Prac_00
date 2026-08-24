using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MoveSpeedAuthoring : MonoBehaviour
{
    public float Speed = 5f;
    public Vector3 Direction = new Vector3(0, 0, 1);

    public class MoveSpeedBaker : Baker<MoveSpeedAuthoring>
    {
        public override void Bake(MoveSpeedAuthoring authoring)
        {
            // TransformUsageFlags.Dynamic indicates this entity will move
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new MoveSpeedComponent
            {
                Value = authoring.Speed,
                Direction = authoring.Direction
            });
        }
    }
}

