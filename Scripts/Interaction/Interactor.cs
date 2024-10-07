using System;
using UnityEngine;

public abstract class Interactor : MonoBehaviour, IInteract
{
    // When interacted with
    public Action onUse = null;
    
    // For validation when the Player uses the object
    public Action onObtained = null;
    public Action onRemoved = null;
    
    public virtual void Use()
    {
        onUse?.Invoke();
    }

    protected void ChangeDebugText(string changeText)
    {
        if (!this.TryGetComponent(out DebugTextDisplayer textDisplayer)) return;
        textDisplayer.Set(changeText);
    }

    public virtual void OnObtain()
    {
        onObtained?.Invoke();
    }

    // TODO: Make the position generic
    public virtual void OnRemove(Vector3 position)
    {
        onRemoved?.Invoke();
    }
}
