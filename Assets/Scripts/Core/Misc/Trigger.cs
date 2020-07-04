using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public Action<Collider> OnTriggerEnterEvent;
    private void OnTriggerEnter(Collider other) => OnTriggerEnterEvent?.Invoke(other);
}
