using Leopotam.EcsLite;
using System.IO;

namespace Client 
{
    sealed class InitBuisnessSystem : IEcsInitSystem 
    {
        public void Init (IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var entitiesConfig = ConfigModule.GetConfig<EntitiesConfig>();

            for (int i = 0; i < entitiesConfig.Entities.Count; i++)
            {
                var entity = world.NewEntity();

                entitiesConfig.Entities[i].InitEntity(world, entity);
                if (i == 0 && !File.Exists(SaveManager.SaveFilePath))
                {
                    world.GetPool<InitFirstBusinessEvent>().Add(entity);
                    world.GetPool<AddLevelUpEvent>().Add(entity);
                }
                else
                    world.GetPool<Client.LockComponent>().Add(entity);
            }
            SaveManager.LoadGame(world);
        }
    }
}