using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [SerializeField] private ItemCollection _collectionLink;

    #region Singleton
    public static ItemCollection Collection;

    private void Awake()
    {
        if (Collection != null)
        {
            if (_collectionLink != Collection) { Debug.LogError("More than one ItemCollection found!"); }
            return;
        }
        Collection = _collectionLink;
    }
    #endregion


    public static int GetItemId(Item item)
    {
        for (int i = 0; i < Collection.Items.Length; i++)
        {
            if (item == Collection.Items[i]) return i;
        }
        if (item != null) Debug.LogError("Items " + item.name + " not found in ItemBase!");
        return -1;
    }

    public static Item GetItem(int id)
    {
        return id == -1 ? null : Collection.Items[id];
    }
}
