using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntitiesConfig", menuName = "Config/EntitiesConfig")]
public class EntitiesConfig : Config
{
    public List<ScriptableObjectEntity> Entities;

    public override IEnumerator Init()
    {
        yield return null;
    }
}
