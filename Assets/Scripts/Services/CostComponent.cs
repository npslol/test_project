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
            ref var costComp = ref world.GetPool<Client.CostComponent>().Add(entity);
            costComp.CostBase = Cost;
            costComp.Cost = Cost;
        }
    }
}

namespace Client
{
    public struct CostComponent
    {
        public int CostBase;
        public int Cost;

        public void RecalculateCost(int level)
        {
            Cost = (level + 1) * CostBase;
        }
    }
}