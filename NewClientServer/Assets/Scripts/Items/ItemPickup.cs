using UnityEngine;

public class ItemPickup : Interactable 
{

    public Item Item;
    public float LifeTime;

    private void Update()
    {
        if (isServer)
        {
            LifeTime -= Time.deltaTime;
            if (LifeTime <= 0) { Destroy(gameObject); }
        }
    }

    public override bool Interact(GameObject user) 
    {
        return PickUp(user);
    }

    public bool PickUp(GameObject user) 
    {
        Character character = user.GetComponent<Character>();
        if (character != null && character.Player.Inventory.AddItem(Item))
        {
            Destroy(gameObject);
            return true;
        }
        else { return false; }
    }
}
