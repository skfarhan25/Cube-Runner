using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public EnemyMovement enemyMovement;
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public float TimeTaken = 3f;
    private float ElapsedTime;
    public bool Frozen = false;
    public bool Custom = false;
    public bool Tracking = false;
    public bool Looping = true;
    private Vector3 temp;
    [SerializeField] private AnimationCurve curve;
    private Transform _transform;

    void Awake()
    {   
        _transform = transform;
        StartPosition = _transform.position;
        if (!Custom)
        {
            EndPosition = StartPosition;
            EndPosition.x *= -1;
        }
        else if (Tracking)
        {
            EndPosition = StartPosition;
        }
    }

    void Update()
    {
        if (!Frozen)
        {
            if (Tracking)
            {
                EndPosition.x = player.position.x;
            }

            ElapsedTime += Time.deltaTime;
            float PercentageComplete = ElapsedTime / TimeTaken;

            _transform.position = Vector3.Lerp(StartPosition, EndPosition, curve.Evaluate(PercentageComplete));

            if (_transform.position.x == EndPosition.x && Looping)
            {
                ElapsedTime = 0f;
                temp = StartPosition;
                StartPosition = EndPosition;
                EndPosition = temp;
            }
        }
    }

    void OnCollisionEnter(Collision collisionInfo) 
    {
        if (collisionInfo.collider.tag == "Player")
        {
            enemyMovement.enabled = false;
        }
    }
}