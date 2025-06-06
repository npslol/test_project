using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncomeComponent", menuName = "Components/IncomeComponent")]
public class IncomeComponent : ScriptableObjectComponent
{
    public int Value;

    public override void AddComponent(EcsWorld world, int entity)
    {
        if (!world.GetPool<Client.IncomeComponent>().Has(entity))
        {
            world.GetPool<Client.IncomeComponent>().Add(entity).BaseIncome = Value;
        }
    }
}

namespace Client
{
    public struct IncomeComponent
    {
        public int Level;                  // Уровень бизнеса
        public int BaseIncome;             // Базовый доход бизнеса
        public List<float> Modifiers;    

        public int Income; // Рассчитанный доход

        // Функция для пересчета дохода по формуле

        public void AddModifier(float value)
        {
            if (Modifiers == null ) Modifiers = new List<float>();

            Modifiers.Add(value);

            RecalculateIncome();
        }

        public void RecalculateIncome()
        {
            // Формула расчета дохода
            float totalMultiplier = 1;

            Modifiers.ForEach(x => totalMultiplier += x);

            Income = Mathf.FloorToInt(Level * BaseIncome * totalMultiplier);

            // Выводим результат для отладки (не обязательно)
            Debug.Log("Recalculated Income: " + Income);
        }

        // Функция для повышения уровня
        public void LevelUp()
        {
            Level++;
            RecalculateIncome(); // Пересчитываем доход после повышения уровня
        }
    }
}