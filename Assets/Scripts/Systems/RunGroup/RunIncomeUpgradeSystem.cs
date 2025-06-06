using Leopotam.EcsLite;

namespace Client 
{
    sealed class RunIncomeUpgradeSystem : IEcsInitSystem, IEcsRunSystem 
    {
        EcsWorld _world;
        EcsFilter _filter;
        EcsFilter _incomeFilter;
        EcsPool<AddUpgradeEvent> _addPool;
        EcsPool<IncomeComponent> _incomePool;
        EcsPool<UpgradeComponent> _upgradePool;
        EcsPool<LockComponent> _lockPool;
        EcsPool<AmplifierComponent> _amplifierPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<AddUpgradeEvent>().Inc<UpgradeComponent>().Inc<LockComponent>().Inc<AmplifierComponent>().End();
            _incomeFilter = _world.Filter<IncomeComponent>().End();

            _addPool = _world.GetPool<AddUpgradeEvent>();
            _incomePool = _world.GetPool<IncomeComponent>();
            _upgradePool = _world.GetPool<UpgradeComponent>();
            _lockPool = _world.GetPool<LockComponent>();
            _amplifierPool = _world.GetPool<AmplifierComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var upgradeComp = ref _upgradePool.Get(entity);

                ref var amplifierComp = ref _amplifierPool.Get(entity);

                ref var incomComp = ref _incomePool.Get(upgradeComp.OwnerEntity);
                incomComp.AddModifier(amplifierComp.PercentageValue);

                _lockPool.Del(entity);
                _addPool.Del(entity);
            }
        }
    }
}