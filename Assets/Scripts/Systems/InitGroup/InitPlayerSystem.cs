using Leopotam.EcsLite;

namespace Client
{
    sealed class InitPlayerSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var entity = world.NewEntity();

            world.GetPool<PlayerComponent>().Add(entity);
            world.GetPool<BalanceComponent>().Add(entity);
        }
    }
}