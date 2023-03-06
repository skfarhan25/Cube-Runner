using UnityEngine;
using System.Collections;

public class JumpPowerUP : MonoBehaviour
{
    [SerializeField] private Rigidbody PlayerRB;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private int JumpHeight;
    [SerializeField] private int DownForce;
    [SerializeField] private float duration;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Jump());
            StartCoroutine(Rotate());
        }
    }

    public IEnumerator Jump()
    {
        PlayerRB.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse);
        yield return new WaitForSeconds(duration/2);
        PlayerRB.AddForce(Vector3.down * DownForce, ForceMode.Impulse);
    }

    public IEnumerator Rotate()
    {
        float time = 0;
        Vector3 endvalue = new Vector3(-180, 0, 0);
        while (time < duration)
        {
            PlayerTransform.rotation = Quaternion.Lerp(Quaternion.Euler(Vector3.zero), Quaternion.Euler(endvalue), time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }
}