using Leopotam.EcsLite;

namespace Client 
{
    sealed class InitBuisnessSystem : IEcsInitSystem 
    {
        public void Init (IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var entitiesConfig = ConfigModule.GetConfig<EntitiesConfig>();

            foreach (var baseEntity in entitiesConfig.Entities)
            {
                var entity = world.NewEntity();

                baseEntity.InitEntity(world, entity);
            }
        }
    }
}