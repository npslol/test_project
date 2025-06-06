using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "BuisnessComponent", menuName = "Components/BuisnessComponent")]
public class BuisnessComponent : ScriptableObjectComponent
{
    public override void AddComponent(EcsWorld world, int entity)
    {
        if (!world.GetPool<Client.BuisnessComponent>().Has(entity))
        {
            world.GetPool<Client.BuisnessComponent>().Add(entity);
        }
    }
}

namespace Client
{
    public struct BuisnessComponent
    {
        public int Level;
    }
}