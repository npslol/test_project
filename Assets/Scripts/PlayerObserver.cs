using System;
using UnityEngine;

public static class PlayerObserver
{
    public static Action<int> OnBalanceChange;
    public static Action<int, float> OnDelayUpdate;
    public static Action OnLevelUpdate;
    public static Action OnUpgradeUpdate;

    public static void BalanceChange(int value)
    {
        OnBalanceChange?.Invoke(value);
    }
    public static void BuisnessLevelChange()
    {
        OnLevelUpdate?.Invoke();
    }
    public static void UpgradeLevelChange()
    {
        OnUpgradeUpdate?.Invoke();
    }

    public static void DelayUpdate(int entity, float value)
    {
        OnDelayUpdate?.Invoke(entity, value);  
    }
}
