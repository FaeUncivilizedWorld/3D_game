
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NpcWaypointWalker : MonoBehaviour
{
    public enum PathMode { Loop, PingPong, Once }

    [Header("Path")]
    public Transform[] waypoints;
    public PathMode pathMode = PathMode.Loop;
    public float waitAtWaypoint = 2f;
    public float walkSpeed = 1.4f;

    [Header("Talking")]
    public Interactor player;
    public float stopDistance = 4f;
    public float turnSpeed = 5f;

    [Header("Animation")]
    public Animator animator;
    public string speedParameter = "";
    public float speedDamping = 0.1f;

    private NavMeshAgent agent;
    private int index;
    private int direction = 1;
    private float waitTimer;
    private bool waiting;
    private bool finished;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        GoToCurrent();
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0 || finished)
        {
            UpdateAnimator();
            return;
        }

        bool talking = player != null
            && player.InputLocked
            && Vector3.Distance(transform.position, player.transform.position) < stopDistance;

        if (talking)
        {
            agent.isStopped = true;

            Vector3 look = player.transform.position - transform.position;
            look.y = 0f;

            if (look.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(look),
                    turnSpeed * Time.deltaTime
                );
            }

            UpdateAnimator();
            return;
        }

        agent.isStopped = false;

        if (waiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                waiting = false;
                Advance();
            }

            UpdateAnimator();
            return;
        }

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            waiting = true;
            waitTimer = waitAtWaypoint;
        }

        UpdateAnimator();
    }

    private void Advance()
    {
        if (waypoints.Length <= 1) return;

        switch (pathMode)
        {
            case PathMode.Loop:
                index = (index + 1) % waypoints.Length;
                break;

            case PathMode.PingPong:
                if (index + direction >= waypoints.Length ||
                    index + direction < 0)
                    direction = -direction;

                index += direction;
                break;

            case PathMode.Once:
                if (index >= waypoints.Length - 1)
                {
                    finished = true;
                    return;
                }

                index++;
                break;
        }

        GoToCurrent();
    }

    private void GoToCurrent()
    {
        if (waypoints == null || waypoints.Length == 0 ||
            waypoints[index] == null)
            return;

        agent.SetDestination(waypoints[index].position);
    }

    private void UpdateAnimator()
    {
        if (animator == null || string.IsNullOrEmpty(speedParameter))
            return;

        animator.SetFloat(
            speedParameter,
            agent.velocity.magnitude,
            speedDamping,
            Time.deltaTime
        );
    }
}
