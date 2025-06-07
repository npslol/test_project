using Leopotam.EcsLite;

namespace Client 
{
 /// <summary>
 /// upgrade purchase system
 /// </summary>
    sealed class RunIncomeUpgradeSystem : IEcsInitSystem, IEcsRunSystem 
    {
        EcsWorld _world;
        EcsFilter _filter;
        EcsFilter _playerFilter;
        EcsPool<AddUpgradeEvent> _addPool;
        EcsPool<IncomeComponent> _incomePool;
        EcsPool<UpgradeComponent> _upgradePool;
        EcsPool<LockComponent> _lockPool;
        EcsPool<BalanceComponent> _balancePool;
        EcsPool<AmplifierComponent> _amplifierPool;
        EcsPool<CostComponent> _costPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<AddUpgradeEvent>().Inc<UpgradeComponent>().Inc<AmplifierComponent>().End();
            _playerFilter = _world.Filter<PlayerComponent>().Inc<BalanceComponent>().End();

            _addPool = _world.GetPool<AddUpgradeEvent>();
            _incomePool = _world.GetPool<IncomeComponent>();
            _upgradePool = _world.GetPool<UpgradeComponent>();
            _lockPool = _world.GetPool<LockComponent>();
            _balancePool = _world.GetPool<BalanceComponent>();
            _costPool = _world.GetPool<CostComponent>();
            _amplifierPool = _world.GetPool<AmplifierComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var upgradeComp = ref _upgradePool.Get(entity);
                ref var costComp = ref _costPool.Get(entity);
                ref var amplifierComp = ref _amplifierPool.Get(entity);

                foreach (var playerEntity in _playerFilter)
                {
                    ref var balanceComp = ref _balancePool.Get(playerEntity);

                    if (balanceComp.Spend(costComp.Cost))
                    {
                        ref var incomeComp = ref _incomePool.Get(upgradeComp.OwnerEntity);
                        incomeComp.AddModifier(amplifierComp.PercentageValue);
                        _lockPool.Del(entity);

                        PlayerObserver.UpgradeLevelChange();
                    }
                }

                _addPool.Del(entity);
            }
        }
    }
}