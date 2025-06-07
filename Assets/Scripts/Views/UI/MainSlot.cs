using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.UI;
using Client;
using System;
using System.Reflection;

public class MainSlot : MonoBehaviour
{
    [SerializeField] private Button _btnLevelUp;
    [SerializeField] private Button[] _btnUpgrades;

    [SerializeField] private Image _sliderFill;

    [SerializeField] private Text _header;
    [SerializeField] private Text _currentLevel;
    [SerializeField] private Text _currentIncome;
    [SerializeField] private Text _currentCost;

    [SerializeField] private Text[] _headersUpd;
    [SerializeField] private Text[] _titleUpd;
    [SerializeField] private Text[] _costUpd;

    private int _entity;
    private EcsWorld _world;

    public void Init(EcsWorld world, int entity)
    {
        _world = world;
        _entity = entity;

        // Подписка на событие изменения баланса
        PlayerObserver.OnBalanceChange += onBalanceChange;
        PlayerObserver.OnDelayUpdate += onSliderValueUpdate;
        PlayerObserver.OnLevelUpdate += onBuisnessChange;
        PlayerObserver.OnUpgradeUpdate += onBuisnessChange;

        if (world.GetPool<Client.DescriptionComponent>().Has(entity))
        {
            ref var description = ref world.GetPool<Client.DescriptionComponent>().Get(entity);
            _header.text = description.Header;
        }
        // Обработчики кликов для кнопок
        _btnLevelUp.onClick.AddListener(onLevelUp);
        for (int i = 0; i < _btnUpgrades.Length; i++)
        {
            int index = i;  // Замыкание для использования внутри лямбда-функции
            _btnUpgrades[i].onClick.AddListener(() => onUpgradeUp(index));
        }

        // Инициализация интерфейса
        onBuisnessChange();
    }

    // Обновление значения слайдера
    void onSliderValueUpdate(int entity, float value)
    {
        if (_entity == entity)
        {
            _sliderFill.fillAmount = value;
        }
    }

    // Обработчик для улучшений
    void onUpgradeUp(int index)
    {
        // Получаем бизнес компонент
        ref var buisnessComp = ref _world.GetPool<Client.BuisnessComponent>().Get(_entity);
        var upgradeEntity = buisnessComp.UpgradesEntity[index];

        // Обновляем компоненты улучшения
        _world.GetPool<AddUpgradeEvent>().Add(upgradeEntity);

        ref var costComp = ref _world.GetPool<Client.CostComponent>().Get(upgradeEntity);
        ref var amplifier = ref _world.GetPool<Client.AmplifierComponent>().Get(upgradeEntity);

        // Обновляем описание и стоимость улучшения
        if (_world.GetPool<Client.DescriptionComponent>().Has(upgradeEntity))
        {
            ref var description = ref _world.GetPool<Client.DescriptionComponent>().Get(upgradeEntity);
            _headersUpd[index].text = description.Header;
            _titleUpd[index].text = $"{description.Title}: {(int)(amplifier.PercentageValue * 100)}%";
        }

        // Обновляем текст и кнопку улучшения
        if (_world.GetPool<Client.LockComponent>().Has(upgradeEntity))
            _costUpd[index].text = $"Стоимость: {costComp.Cost}";
        else
        {
            _costUpd[index].text = "Куплено";
            _btnUpgrades[index].interactable = false;
        }

    }

    // Обновление информации о бизнесе (уровень, доход, стоимость)
    void onBuisnessChange()
    {
        ref var costComp = ref _world.GetPool<Client.CostComponent>().Get(_entity);
        ref var incomComp = ref _world.GetPool<Client.IncomeComponent>().Get(_entity);

        // Получаем описание бизнеса
        if (_world.GetPool<Client.DescriptionComponent>().Has(_entity))
        {
            ref var descriptionComp = ref _world.GetPool<Client.DescriptionComponent>().Get(_entity);
            _header.text = descriptionComp.Header;
        }

        _currentLevel.text = $"LVL {incomComp.Level}";
        _currentIncome.text = $"Доход {incomComp.Income}$";
        _currentCost.text = $"Цена: {costComp.Cost}";

        // Обновление улучшений
        ref var buisnessComp = ref _world.GetPool<Client.BuisnessComponent>().Get(_entity);
        for (int i = 0; i < buisnessComp.UpgradesEntity.Count; i++)
        {
            onChange(i, buisnessComp.UpgradesEntity[i]);
        }
    }

    // Обновление информации о стоимости и состоянии улучшений
    void onChange(int index, int upgradeEntity)
    {
        ref var costComp = ref _world.GetPool<Client.CostComponent>().Get(upgradeEntity);
        ref var amplifier = ref _world.GetPool<Client.AmplifierComponent>().Get(upgradeEntity);

        if (_world.GetPool<Client.DescriptionComponent>().Has(upgradeEntity))
        {
            ref var description = ref _world.GetPool<Client.DescriptionComponent>().Get(upgradeEntity);
            _headersUpd[index].text = description.Header;
            _titleUpd[index].text = $"{description.Title}: {(int)(amplifier.PercentageValue * 100)}%";
        }

        if (_world.GetPool<Client.LockComponent>().Has(upgradeEntity))
            _costUpd[index].text = $"Стоимость: {costComp.Cost}";
        else
        {
            _costUpd[index].text = "Куплено";
            _btnUpgrades[index].interactable = false;
        }
    }

    // Повышение уровня бизнеса
    void onLevelUp()
    {
        _world.GetPool<AddLevelUpEvent>().Add(_entity);
    }

    // Обработка изменения баланса игрока
    void onBalanceChange(int value)
    {
        ref var costComp = ref _world.GetPool<Client.CostComponent>().Get(_entity);

        // Проверяем, можно ли повысить уровень
        if (value >= costComp.Cost)
        {
            _btnLevelUp.interactable = true;
        }
        else
        {
            _btnLevelUp.interactable = false;
        }

        // Обновляем все данные о бизнесе
        onBuisnessChange();
    }

    private void OnDestroy()
    {
        _btnLevelUp.onClick.RemoveAllListeners();
        for (int i = 0; i < _btnUpgrades.Length; i++)
        {
            _btnUpgrades[i].onClick.RemoveAllListeners();
        }

        PlayerObserver.OnBalanceChange -= onBalanceChange;
        PlayerObserver.OnDelayUpdate -= onSliderValueUpdate;
        PlayerObserver.OnLevelUpdate -= onBuisnessChange;
        PlayerObserver.OnUpgradeUpdate -= onBuisnessChange;
    }
}
