using UnityEngine;

public class Spook : MonoBehaviour
{
    [SerializeField] private GameObject SpookEnemy;

    void OnTriggerEnter()
    {
        SpookEnemy.SetActive(!SpookEnemy.activeSelf);
    }
}