using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public float TimeTaken = 3f;
    private float ElapsedTime;
    public bool Frozen = false;
    public bool Active = false;
    public bool Looping = true;
    private Vector3 temp;
    private Transform _transform;

    void Start()
    {   
        _transform = transform;
        StartPosition = _transform.position;
        EndPosition = StartPosition;
        EndPosition.x *= -1;
    }

    public void Activate()
    {
        Active = true;
    }

    void Update()
    {
        if (!Frozen && Active)
        {
            ElapsedTime += Time.deltaTime;
            float PercentageComplete = ElapsedTime / TimeTaken;

            _transform.position = Vector3.Lerp(StartPosition, EndPosition, PercentageComplete);

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
            this.enabled = false;
        }
    }
}