using System.Collections;

using UnityEngine;

public abstract class Config : ScriptableObject
{
    public virtual string Name { get => name; }
    public abstract IEnumerator Init();
}
