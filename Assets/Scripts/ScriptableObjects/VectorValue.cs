using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VectorValue : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector2 Initialvalue;
    public Vector2 defaultValue;
    public void OnAfterDeserialize()
    {
        Initialvalue = defaultValue;
    }
    public void OnBeforeSerialize()
    {

    }
}
