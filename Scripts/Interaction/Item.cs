using System;
using UnityEngine;

public class Item : Interactor
{
    [SerializeField] protected ItemData data = null;
    
    public ItemData GetData => data;
    
    public override void Use()
    {
        base.Use();
        InventoryManager.Instance.Add(this);
    }

    public override void OnObtain()
    {
        base.OnObtain();
        this.gameObject.SetActive(false);
    }
    
    public override void OnRemove(Vector3 position)
    {
        this.gameObject.SetActive(true);
        this.transform.position = position;
        
        base.OnRemove(position);
    }
}
