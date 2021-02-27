using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Info : ScriptableObject
{
    public event Action IncreaseQuantity;   // Quantity of enemies killed

    [SerializeField] private string objectName;

    public string ObjectName { get => objectName; protected set => objectName = value; }

    public void RaiseEvent () => IncreaseQuantity?.Invoke();
    public Delegate[] GetInvocationList () => IncreaseQuantity?.GetInvocationList();
}
