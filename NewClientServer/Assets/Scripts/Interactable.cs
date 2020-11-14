using UnityEngine;
using UnityEngine.Networking;

public class Interactable : NetworkBehaviour
{
    [SerializeField] private float _radius = 2f;

    public Transform InterectionTransform;

    private bool _hasInteract = true;

    public bool HasInteract
    { get { return _hasInteract; } set { _hasInteract = value; } }


    public virtual float GetInteractDistance(GameObject user)
    {
        return _radius;
    }

    public virtual bool Interact(GameObject user)
    {
        return false;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(InterectionTransform.position, _radius);
    }

}

