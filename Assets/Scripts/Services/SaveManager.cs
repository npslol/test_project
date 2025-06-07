using UnityEngine;
using Leopotam.EcsLite;
using System.IO;
using Client;
using System.Collections.Generic;
using System;

public static class SaveManager
{
    public static string SaveFilePath = Application.persistentDataPath + "/game_save.json";
    public static int PlayerEntity;
    // Сохранение игры в файл
    public static void SaveGame(EcsWorld world)
    {
        GameData gameData = new GameData();

        // Сохраняем баланс игрока
        var playerFilter = world.Filter<PlayerComponent>().Inc<Client.BalanceComponent>().End();
        foreach (var playerEntity in playerFilter)
        {
            PlayerEntity = playerEntity;
            ref var balanceComp = ref world.GetPool<Client.BalanceComponent>().Get(playerEntity);
            gameData.playerBalance = balanceComp.Value;
        }


        // Сохраняем данные бизнесов
        var businessEntities = world.Filter<Client.BuisnessComponent>().End();

        foreach (var businessEntity in businessEntities)
        {
            ref var businessComp = ref world.GetPool<Client.BuisnessComponent>().Get(businessEntity);
            ref var incomeComp = ref world.GetPool<Client.IncomeComponent>().Get(businessEntity);
            businessComp.Level = incomeComp.Level;
            BusinessData businessData = new BusinessData
            {
                businessLevel = businessComp.Level,
                upgradesBought = new bool[businessComp.UpgradesEntity.Count],
                currentDelay = 0f // Инициализируем значение задержки
            };

            // Сохраняем состояние апгрейдов
            for (int i = 0; i < businessComp.UpgradesEntity.Count; i++)
            {
                var upgradeEntity = businessComp.UpgradesEntity[i];
                businessData.upgradesBought[i] = !world.GetPool<Client.LockComponent>().Has(upgradeEntity); // Если LockComponent нет, значит апгрейд куплен
            }

            // Проверяем, есть ли компонент DelayIncomeComponent, и сохраняем текущее время задержки
            if (world.GetPool<Client.DelayIncomeComponent>().Has(businessEntity))
            {
                ref var delayIncomeComp = ref world.GetPool<Client.DelayIncomeComponent>().Get(businessEntity);
                businessData.currentDelay = delayIncomeComp.CurrentDelay;
            }

            gameData.businessesData.Add(businessData);
        }

        // Записываем данные в файл
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log("Game saved!");
    }
    public static void LoadGame(EcsWorld world)
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);

            // Загружаем баланс игрока
            var playerFilter = world.Filter<PlayerComponent>().Inc<Client.BalanceComponent>().End();
            foreach (var playerEntity in playerFilter)
            {
                ref var balanceComp = ref world.GetPool<Client.BalanceComponent>().Get(playerEntity);
                balanceComp.Value = gameData.playerBalance;
            }

            // Загружаем данные бизнесов
            var businessEntities = world.Filter<Client.BuisnessComponent>().End();
            int index = 0; // Индекс для прохождения по данным игры
            foreach (var businessEntity in businessEntities)
            {
                // Проверяем, что в списке бизнесов есть данные для текущей сущности
                if (index < gameData.businessesData.Count)
                {
                    ref var businessComp = ref world.GetPool<Client.BuisnessComponent>().Get(businessEntity);
                    ref var incomeComp = ref world.GetPool<Client.IncomeComponent>().Get(businessEntity);
                    ref var costComp = ref world.GetPool<Client.CostComponent>().Get(businessEntity);
                    var businessData = gameData.businessesData[index];

                    // Восстанавливаем уровень бизнеса
                    businessComp.Level = businessData.businessLevel;
                    incomeComp.Level = businessData.businessLevel;
                    

                    if (incomeComp.Level > 0) world.GetPool<Client.LockComponent>().Del(businessEntity);
                    // Восстанавливаем купленные апгрейды
                    for (int j = 0; j < businessData.upgradesBought.Length; j++)
                    {
                        var upgradeEntity = businessComp.UpgradesEntity[j];

                        if (businessData.upgradesBought[j])
                        {
                            // Если апгрейд куплен, удаляем блокировку, преобразуем
                            if (world.GetPool<Client.LockComponent>().Has(upgradeEntity))
                            {
                                world.GetPool<Client.LockComponent>().Del(upgradeEntity);
                                ref var upgradeComp = ref world.GetPool<Client.UpgradeComponent>().Get(upgradeEntity);
                                ref var amplifierComp = ref world.GetPool<Client.AmplifierComponent>().Get(upgradeEntity);


                                ref var incomeUpgradeComp = ref world.GetPool<Client.IncomeComponent>().Get(upgradeComp.OwnerEntity);
                                incomeComp.AddModifier(amplifierComp.PercentageValue);
                            }
                        }
                        else
                        {
                            // Если апгрейд не куплен, добавляем блокировку
                            if (!world.GetPool<Client.LockComponent>().Has(upgradeEntity))
                            {
                                world.GetPool<Client.LockComponent>().Add(upgradeEntity);
                            }
                        }
                    }

                    
                    if (world.GetPool<Client.DelayIncomeComponent>().Has(businessEntity))
                    {
                        ref var delayIncomeComp = ref world.GetPool<Client.DelayIncomeComponent>().Get(businessEntity);
                        delayIncomeComp.CurrentDelay = businessData.currentDelay;
                    }

                    incomeComp.RecalculateIncome();
                    costComp.RecalculateCost(incomeComp.Level);

                }

                index++;
            }

            Debug.Log("Game loaded!");
        }
        else
        {
            Debug.Log("No save file found.");
        }
    }


}
[Serializable]
public class GameData
{
    public int playerBalance;
    public List<BusinessData> businessesData = new List<BusinessData>();
}

[Serializable]
public class BusinessData
{
    public int businessLevel;
    public bool[] upgradesBought;
    public float currentDelay;
}
