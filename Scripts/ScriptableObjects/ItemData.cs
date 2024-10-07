using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/Items")]
public class ItemData : ScriptableObject
{
    public string key = string.Empty;
    [TextArea(4, 10)] public string description = "";
    public Sprite icon = null;
    [AssetsOnly] public GameObject go = null;
}
