using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    MainPanel mainPanel;

    public void Init(Leopotam.EcsLite.EcsWorld world)
    {
        mainPanel = GetComponentInChildren<MainPanel>();    

        if (mainPanel != null)
        {
            mainPanel.Init(world);
        }
    }
}
