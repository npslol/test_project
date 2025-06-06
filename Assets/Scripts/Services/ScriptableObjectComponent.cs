using UnityEngine;

public abstract class ScriptableObjectComponent : ScriptableObject
{
    public abstract void AddComponent(Leopotam.EcsLite.EcsWorld world, int entity);
}
