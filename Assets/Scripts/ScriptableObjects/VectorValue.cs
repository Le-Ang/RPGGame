using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VectorValue : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Value running in game")]
    public Vector2 Initialvalue;
    [Header("Value by default when starting")]
    public Vector2 defaultValue;
    public void OnAfterDeserialize()
    {
        Initialvalue = defaultValue;
    }
    public void OnBeforeSerialize()
    {

    }
}
