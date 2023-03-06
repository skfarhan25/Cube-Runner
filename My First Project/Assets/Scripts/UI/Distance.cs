using UnityEngine;
using UnityEngine.UI;

public class Distance : MonoBehaviour
{
    public Transform player;
    public Transform goal;
    public Text DistanceText;

    void Update()
    {
        DistanceText.text = (goal.position.z - player.position.z).ToString("0");
    }
}
