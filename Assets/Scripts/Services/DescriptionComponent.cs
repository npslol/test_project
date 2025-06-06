using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "DescriptionComponent", menuName = "Components/DescriptionComponent")]
public class DescriptionComponent : ScriptableObjectComponent
{
    public string Header;
    public string Title;

    public override void AddComponent(EcsWorld world, int entity)
    {
        if (!world.GetPool<Client.DescriptionComponent>().Has(entity))
        {
            ref var descriptionComp = ref world.GetPool<Client.DescriptionComponent>().Add(entity);
            descriptionComp.Title = Title;
            descriptionComp.Header = Header;
        }
    }
}

namespace Client
{
    public struct DescriptionComponent
    {
        public string Header;
        public string Title;
    }
}