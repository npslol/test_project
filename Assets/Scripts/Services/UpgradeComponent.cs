using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeComponent", menuName = "Components/UpgradeComponent")]
public class UpgradeComponent : ScriptableObjectComponent
{
    public List<ScriptableObjectComponent> Components;

    public override void AddComponent(EcsWorld world, int entity)
    {
        var newEnity = world.NewEntity();

        world.GetPool<Client.LockComponent>().Add(newEnity);
        ref var upgradeComp = ref world.GetPool<Client.UpgradeComponent>().Add(newEnity);
        upgradeComp.OwnerEntity = entity;

        foreach (var component in Components)
        {
            component.AddComponent(world, newEnity);
        }
    }
}

namespace Client
{
    public struct UpgradeComponent
    {
        public int OwnerEntity;
    }
}