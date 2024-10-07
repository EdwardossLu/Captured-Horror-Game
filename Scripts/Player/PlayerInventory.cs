using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public void Up()
    {
        InventoryManager.Instance.Up();
    }

    public void Down()
    {
        InventoryManager.Instance.Down();
    }
    
    public void Select(int index)
    {
        InventoryManager.Instance.Select(index);
    }
}
