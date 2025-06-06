using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    sealed class RunDelayIncomeSystem : IEcsRunSystem, IEcsInitSystem
    {
        EcsWorld _world;
        EcsFilter _filter;
        EcsPool<DelayIncomeComponent> _delayPool;
        EcsPool<AddIncomeEvent> _incomeAddPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<DelayIncomeComponent>().End();
            _delayPool = _world.GetPool<DelayIncomeComponent>();
            _incomeAddPool = _world.GetPool<AddIncomeEvent>();
        }

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter)
            {
                ref var delayComp = ref _delayPool.Get(entity);
                delayComp.CurrentDelay -= Time.deltaTime;

                if (delayComp.CurrentDelay <= 0)
                {
                    delayComp.CurrentDelay = delayComp.Delay;

                    _incomeAddPool.Add(entity);

                    Debug.Log("Income!");
                }
            }
        }
    }
}