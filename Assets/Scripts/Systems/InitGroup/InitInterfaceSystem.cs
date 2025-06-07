using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    sealed class InitInterfaceSystem : IEcsInitSystem 
    {
        public void Init (IEcsSystems systems) 
        {
            EcsWorld world = systems.GetWorld();

            var entity = world.NewEntity();

            ref var interfaceComp = ref world.GetPool<InterfaceComponent>().Add(entity);
            ref var balanceComp = ref world.GetPool<BalanceComponent>().Get(SaveManager.PlayerEntity);

            var mainCanvas = GameObject.FindObjectOfType<MainCanvas>(true);
            mainCanvas.Init(world);

            interfaceComp.Canvas = mainCanvas;

            PlayerObserver.BuisnessLevelChange();
            PlayerObserver.BalanceChange(balanceComp.Value);
            PlayerObserver.UpgradeLevelChange();
        }
    }
}