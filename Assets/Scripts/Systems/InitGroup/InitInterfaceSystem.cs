using Leopotam.EcsLite;

namespace Client {
    sealed class InitInterfaceSystem : IEcsInitSystem 
    {
        public void Init (IEcsSystems systems) 
        {
            EcsWorld world = systems.GetWorld();

            var entity = world.NewEntity();

            ref var interfaceComp = ref world.GetPool<InterfaceComponent>().Add(entity);
            

        }
    }
}