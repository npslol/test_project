using System.Collections.Generic;
using UnityEngine;

public class MainLayout : MonoBehaviour
{
    private List<MainSlot> slots;

    public void Init(Leopotam.EcsLite.EcsWorld world)
    {
        slots = new List<MainSlot>();

        var interfaceConfig = ConfigModule.GetConfig<InterfaceConfig>();

        var filter = world.Filter<Client.BuisnessComponent>().Inc<Client.IncomeComponent>().End();

        foreach (var entity in filter)
        {
            var slot = GameObject.Instantiate(interfaceConfig.SlotRef, transform);

            slot.Init(world, entity);

            slots.Add(slot);
        }
    }
}
