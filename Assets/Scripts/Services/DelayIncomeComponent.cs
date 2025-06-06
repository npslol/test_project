using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "DelayIncomeComponent", menuName = "Components/DelayIncomeComponent")]
public class DelayIncomeComponent : ScriptableObjectComponent
{
    public float Delay;

    public override void AddComponent(EcsWorld world, int entity)
    {
        if (!world.GetPool<Client.DelayIncomeComponent>().Has(entity))
        {
            world.GetPool<Client.DelayIncomeComponent>().Add(entity).Delay = Delay;
        }
    }
}

namespace Client
{
    public struct DelayIncomeComponent
    {
        public float Delay;
        public float CurrentDelay;
    }
}