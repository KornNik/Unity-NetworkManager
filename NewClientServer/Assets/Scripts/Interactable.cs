using UnityEngine;
using UnityEngine.Networking;

public class Interactable : NetworkBehaviour
{
    public Transform InterectionTransform;
    public float Radius = 2f;

    private bool _hasInteract = true;

    public bool HasInteracte
    { get { return _hasInteract; } protected set { _hasInteract = value; } }

    public virtual bool Interact(GameObject user)
    {
        return false;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(InterectionTransform.position, Radius);
    }

}

