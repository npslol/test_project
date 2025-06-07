using Leopotam.EcsLite;

namespace Client
{
    /// <summary>
    /// system for getting a new level of business
    /// </summary>
    sealed class RunLevelUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld _world;
        EcsFilter _filter;
        EcsFilter _playerFilter;
        EcsPool<AddLevelUpEvent> _levelupAddPool;
        EcsPool<IncomeComponent> _incomePool;
        EcsPool<CostComponent> _costPool;
        EcsPool<LockComponent> _lockPool;
        EcsPool<InitFirstBusinessEvent> _initFirstBusinessPool;
        EcsPool<BalanceComponent> _balancePool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<AddLevelUpEvent>().Inc<IncomeComponent>().Inc<CostComponent>().End();
            _playerFilter = _world.Filter<PlayerComponent>().Inc<BalanceComponent>().End();
            _levelupAddPool = _world.GetPool<AddLevelUpEvent>();
            _incomePool = _world.GetPool<IncomeComponent>();
            _lockPool = _world.GetPool<LockComponent>();
            _initFirstBusinessPool = _world.GetPool<InitFirstBusinessEvent>();
            _costPool = _world.GetPool<CostComponent>();
            _balancePool = _world.GetPool<BalanceComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var incomeComp = ref _incomePool.Get(entity);
                ref var costComp = ref _costPool.Get(entity);

                foreach (var playerEntity in _playerFilter)
                {
                    ref var balanceComp = ref _balancePool.Get(playerEntity);

                    if (balanceComp.Spend(costComp.Cost) || _initFirstBusinessPool.Has(entity)) 
                    {
                        incomeComp.LevelUp();
                        costComp.RecalculateCost(incomeComp.Level);
                        PlayerObserver.BuisnessLevelChange();
                        _initFirstBusinessPool.Del(entity); 
                    }
                }
                 _lockPool.Del(entity);
                _levelupAddPool.Del(entity);
            }
        }
    }
}