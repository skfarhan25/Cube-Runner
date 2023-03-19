using System.Collections.Generic;
using UnityEngine;

public class SOManager : MonoBehaviour
{
    public List<Transform> _obstacles;
    public List<TestMovement> _obstacleScript;
    public float ActivationPoint = 4f;
    [SerializeField] private bool temp = false;
    int index = 0;

    void Start()
    {
        _obstacleScript[index].Activate();
    }

    void Update()
    {
        if (temp)
        {
            TestMethod();
        }
        else
        {
            if (_obstacles[index].position.x < 0)
            {
                if (_obstacles[index].position.x >= -ActivationPoint)
                {
                    _obstacleScript[index+1].Activate();
                    index++;

                    CheckIndex();
                }
            }
            else if (_obstacles[index].position.x <= ActivationPoint)
            {
                _obstacleScript[index+1].Activate();
                index++;

                CheckIndex();
            }
        }
    }

    void CheckIndex()
    {
        if (_obstacleScript[_obstacleScript.Count - 1].Active == true)
        {
            this.enabled = false;
        }
    }

    void TestMethod()
    {
        if (_obstacles[index].position.x < 0)
        {
            if (_obstacleScript[index].StartPosition.x >= -ActivationPoint)
            {
                _obstacleScript[index+1].Activate();
                index++;

                CheckIndex();
            }
        } 
        else if (_obstacleScript[index].StartPosition.x <= ActivationPoint)
        {
            _obstacleScript[index+1].Activate();
            index++;

            CheckIndex();
        }
    }
}
