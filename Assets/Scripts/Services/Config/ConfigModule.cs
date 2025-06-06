using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public static class ConfigModule
{
    public static Action<string, float> OnLoaded;
    public static bool AllConfigInited;
    public static bool AllConfigLoaded;
    private static Dictionary<Type, Config> _configData = new Dictionary<Type, Config>();
    /// <summary>
    /// �������������� ��� Config �� ����� "Configs" � ������� ���������� �� �������������.
    /// </summary>
    public static void InitConfigs(MonoBehaviour caller, Action onComplete)
    {
        if (caller == null)
        {
            Debug.LogError("Caller is null. Config initialization requires a valid MonoBehaviour instance.");
            return;
        }

        caller.StartCoroutine(InitializeConfigsCoroutine(caller, onComplete));
    }

    /// <summary>
    /// ���������� ������ Config �� ��� ����.
    /// </summary>
    public static T GetConfig<T>() where T : Config
    {
        if (_configData.TryGetValue(typeof(T), out var config))
        {
            return config as T;
        }

        Debug.LogError($"Config of type {typeof(T).Name} not found.");
        return null;
    }

    /// <summary>
    /// ��������� ��� Config �� ����� "Configs" � �������������� ��.
    /// </summary>
    private static IEnumerator InitializeConfigsCoroutine(MonoBehaviour caller, Action onComplete)
    {
        Debug.Log("Starting config initialization...");
        // ��������� ��� Config �� ����� "Configs"
        var configs = Resources.LoadAll<Config>("Configs");

        string loadedInfo = $"�������� {configs.Length} ��������";

        float loadedValue = 0f;

        float maxValue = configs.Length;

        OnLoaded?.Invoke(loadedInfo, loadedValue);

        int currentIndex = 0;

        foreach (var config in configs)
        {
            currentIndex++;

            var type = config.GetType();

            if (!_configData.ContainsKey(type))
            {
                _configData.Add(type, config);

                loadedValue = currentIndex / maxValue;

                loadedInfo = $"�������� {config.Name}";

                OnLoaded?.Invoke(loadedInfo, loadedValue);

                yield return new WaitForSeconds(0.5f);
            }
        }

        AllConfigLoaded = true;
        currentIndex = 0;

        // �������������� ������ Config
        foreach (var config in _configData.Values)
        {
            currentIndex++;

            loadedValue = currentIndex / maxValue;

            Debug.Log($"Initializing {config.name}...");

            loadedInfo = $"������������� {config.Name}";

            OnLoaded?.Invoke(loadedInfo, loadedValue);

            yield return caller.StartCoroutine(config.Init());
            Debug.Log($"Initialized {config.name}");

        }

        Debug.Log("All configs initialized successfully.");

        loadedInfo = $"�������� ���������";

        AllConfigInited = true;

        onComplete?.Invoke();
    }
}
