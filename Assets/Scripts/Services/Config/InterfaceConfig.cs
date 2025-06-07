using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InterfaceConfig", menuName = "Config/Interface")]
public class InterfaceConfig : Config
{
    public MainSlot SlotRef;

    public override IEnumerator Init()
    {
        yield return null;
    }
}
