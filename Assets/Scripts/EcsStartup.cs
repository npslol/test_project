using Leopotam.EcsLite;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Client 
{
    sealed class EcsStartup : MonoBehaviour 
    {
        EcsWorld _world;        
        IEcsSystems _systems;

        private void Awake()
        {
            ConfigModule.InitConfigs(this, onComplete);
        }

        void onComplete()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems
                .Add(new InitPlayerSystem())
                .Add(new InitBuisnessSystem())
                .Add(new InitInterfaceSystem())

                .Add(new RunDelayIncomeSystem())
                .Add(new RunIncomeUpgradeSystem())
                .Add(new RunLevelUpSystem())
                .Add(new RunAddIncomeSystem())


                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();
        }

        void Update () {
            // process systems here.
            _systems?.Run ();
        }

        void OnDestroy () {
            if (_systems != null) 
            {
                _systems.Destroy ();
                _systems = null;
            }
            
            if (_world != null) {
                _world.Destroy ();
                _world = null;
            }
        }
        void OnApplicationQuit()
        {
            SaveManager.SaveGame(_world);  // Сохраняем ECS мир при выходе из игры
        }
    }
}