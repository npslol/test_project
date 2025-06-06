using UnityEngine;

[CreateAssetMenu(fileName = "NewEntity", menuName = "Entities/NewEntity")]
public class ScriptableObjectEntity : ScriptableObject
{
    public ScriptableObjectComponent[] Components;

    public void InitEntity(Leopotam.EcsLite.EcsWorld world, int entity)
    {
        int length = Components.Length;

        for (int i = 0; i < length; i++)
        {
            Components[i].AddComponent(world, entity);
        }
    }
}
