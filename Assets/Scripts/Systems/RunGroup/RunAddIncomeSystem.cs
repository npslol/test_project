using Leopotam.EcsLite;

namespace Client
{
    /// <summary>
    /// income generation system
    /// </summary>
    sealed class RunAddIncomeSystem : IEcsRunSystem, IEcsInitSystem
    {
        EcsWorld _world;
        EcsFilter _filter;
        EcsFilter _playerFilter;
        EcsPool<AddIncomeEvent> _incomeAddPool;
        EcsPool<IncomeComponent> _incomePool;
        EcsPool<BalanceComponent> _balancePool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<AddIncomeEvent>().Inc<IncomeComponent>().End(); 
            _playerFilter = _world.Filter<PlayerComponent>().Inc<BalanceComponent>().End();
            _incomeAddPool = _world.GetPool<AddIncomeEvent>();
            _incomePool = _world.GetPool<IncomeComponent>();
            _balancePool = _world.GetPool<BalanceComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var incomeComp = ref _incomePool.Get(entity);

                foreach (var playerEntity in _playerFilter)
                {
                    _balancePool.Get(playerEntity).Add(incomeComp.Income);
                }

                _incomeAddPool.Del(entity);
            }
        }
    }
}