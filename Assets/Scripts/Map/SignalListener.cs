using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    public SignalSender signal;
    public UnityEvent signalEvent;
    public void OnSignalRasie()
    {
        signalEvent.Invoke();
    }

    private void OnEnable()
    {
        signal.RegisterListener(this);
    }

    private void OnDisable()
    {
        signal.DeRegisterListener(this);
    }
}
