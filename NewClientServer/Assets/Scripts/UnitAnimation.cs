using UnityEngine;
using UnityEngine.AI;

public class UnitAnimation : MonoBehaviour {

    private static readonly int Moving = Animator.StringToHash("Moving");
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
	
	void FixedUpdate () 
    {
        if (agent.velocity.magnitude == 0)
        {
            animator.SetBool(Moving, false);
        }
        else { animator.SetBool(Moving, true); }
    }

    void Hit() {
    }

    void FootR() {
    }

    void FootL() {
    }
}
