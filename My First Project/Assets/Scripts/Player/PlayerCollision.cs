using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] bool Shielded = false;
    [SerializeField] float ShieldDuration = 1f;
    private Renderer cubeRenderer;
    
    void Awake()
    {
        cubeRenderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter(Collision collisionInfo) 
    {
        if (Shielded && collisionInfo.collider.tag == "Obstacle")
        {
            StartCoroutine(ChangeLayer());
        }

        if (collisionInfo.collider.tag == "Obstacle" && !Shielded)
        {
            GameManager.instance.EndGame();
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Shield")
        {
            StartCoroutine(ChangeLayer());
            Destroy(other.gameObject);
        }
    }

    IEnumerator ChangeLayer()
    {
        Shielded = true;
        gameObject.layer = LayerMask.NameToLayer("IgnoreCol");
        cubeRenderer.material.SetColor("_Color", Color.blue);
        yield return new WaitForSeconds(ShieldDuration);
        Shielded = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        cubeRenderer.material.SetColor("_Color", Color.red);
    }
}