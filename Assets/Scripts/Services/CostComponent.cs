using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "CostComponent", menuName = "Components/CostComponent")]
public class CostComponent : ScriptableObjectComponent
{
    public int Cost;

    public override void AddComponent(EcsWorld world, int entity)
    {
        if (!world.GetPool<Client.CostComponent>().Has(entity))
        {
            world.GetPool<Client.CostComponent>().Add(entity).Cost = Cost;
        }
    }
}

namespace Client
{
    public struct CostComponent
    {
        public int Cost;
    }
}