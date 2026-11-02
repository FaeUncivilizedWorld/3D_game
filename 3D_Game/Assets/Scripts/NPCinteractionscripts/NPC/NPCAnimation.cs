
using UnityEngine;
using UnityEngine.AI;

public class NpcAnimation : MonoBehaviour
{
    [Header("Who to react to")]
    public Interactor player;
    public float talkDistance = 4f;

    [Header("Animator")]
    public Animator animator;

    [Header("Animation State Names")]
    public string idleState = "Idle";
    public string walkState = "";
    public string talkState = "";

    public float crossFadeSeconds = 0.2f;
    public float walkSpeedThreshold = 0.1f;

    private NavMeshAgent agent;
    private string currentState = "";

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        agent = GetComponentInParent<NavMeshAgent>();
    }

    private void Start()
    {
        Switch(idleState, true);
    }

    private void Update()
    {
        if (animator == null) return;

        bool talking = player != null
            && player.InputLocked
            && Vector3.Distance(transform.position, player.transform.position) < talkDistance;

        string wanted = idleState;

        if (talking && !string.IsNullOrEmpty(talkState))
        {
            wanted = talkState;
        }
        else if (!string.IsNullOrEmpty(walkState)
            && agent != null
            && !agent.isStopped
            && agent.velocity.magnitude > walkSpeedThreshold)
        {
            wanted = walkState;
        }

        Switch(wanted, false);
    }

    private void Switch(string state, bool instant)
    {
        if (string.IsNullOrEmpty(state) || state == currentState)
            return;

        currentState = state;

        if (instant)
            animator.Play(state);
        else
            animator.CrossFadeInFixedTime(state, crossFadeSeconds);
    }
}
