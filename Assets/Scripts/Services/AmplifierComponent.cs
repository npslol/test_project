using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "AmplifierComponent", menuName = "Components/AmplifierComponent")]
public class AmplifierComponent : ScriptableObjectComponent
{
    public float PercentageValue;

    public override void AddComponent(EcsWorld world, int entity)
    {
        if (!world.GetPool<Client.AmplifierComponent>().Has(entity))
        {
            world.GetPool<Client.AmplifierComponent>().Add(entity).PercentageValue = PercentageValue;
        }
    }
}

namespace Client
{
    public struct AmplifierComponent
    {
        public float PercentageValue;
    }
}