using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    MainLayout mainLayout;
    [SerializeField] Text _balanceTitle;

    public void Init(Leopotam.EcsLite.EcsWorld world)
    {
        mainLayout = GetComponentInChildren<MainLayout>();

        if (mainLayout != null)
        {
            mainLayout.Init(world);
        }

        PlayerObserver.OnBalanceChange += onBalanceChange;
    }

    void onBalanceChange(int value)
    {
        _balanceTitle.text = $"Баланс: {value}$";
    }

    private void OnDestroy()
    {
        PlayerObserver.OnBalanceChange -= onBalanceChange;
    }
}
