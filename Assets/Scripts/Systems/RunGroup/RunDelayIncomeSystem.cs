using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    /// <summary>
    /// income generation system after the end of the delay
    /// </summary>
    sealed class RunDelayIncomeSystem : IEcsRunSystem, IEcsInitSystem
    {
        EcsWorld _world;
        EcsFilter _filter;
        EcsPool<DelayIncomeComponent> _delayPool;
        EcsPool<AddIncomeEvent> _incomeAddPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<DelayIncomeComponent>().Exc<LockComponent>().End();
            _delayPool = _world.GetPool<DelayIncomeComponent>();
            _incomeAddPool = _world.GetPool<AddIncomeEvent>();
        }

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter)
            {
                ref var delayComp = ref _delayPool.Get(entity);

                delayComp.CurrentDelay -= Time.deltaTime;

                PlayerObserver.DelayUpdate(entity, 1 - delayComp.CurrentDelay / delayComp.Delay);

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