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
            ref var incomeComp = ref world.GetPool<Client.IncomeComponent>().Add(entity);
            incomeComp.BaseIncome = Value;
            incomeComp.Modifiers = new();
        }
    }
}

namespace Client
{
    public struct IncomeComponent
    {
        public int Level;                  // ������� �������
        public int BaseIncome;             // ������� ����� �������
        public List<float> Modifiers;    

        public int Income; // ������������ �����

        public void AddModifier(float value)
        {
            if (Modifiers == null ) Modifiers = new List<float>();

            Modifiers.Add(value);

            RecalculateIncome();
        }

        public void RecalculateIncome()
        {
            // ������� ������� ������
            float totalMultiplier = 1;

            Modifiers.ForEach(x => totalMultiplier += x);

            Income = Mathf.FloorToInt(Level * BaseIncome * totalMultiplier);

            Debug.Log("Recalculated Income: " + Income);
        }

        // ������� ��� ��������� ������
        public void LevelUp()
        {
            Level++;
            RecalculateIncome(); // ������������� ����� ����� ��������� ������
        }
    }
}