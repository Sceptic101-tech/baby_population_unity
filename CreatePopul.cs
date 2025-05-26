using JetBrains.Annotations;
using UnityEngine;

public class CreatePopul : MonoBehaviour
{
    public GameObject predator;
    public GameObject prey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject AgentObject;
        for (int i = 0; i < 25; i++)
        {
            AgentObject = Instantiate(predator, transform.position - new Vector3(-1, 0, 30 - i), Quaternion.identity);
        }
        for (int i = 0; i < 60; i++)
        {
            AgentObject = Instantiate(prey, transform.position + new Vector3(15, 0, -30 + i), Quaternion.identity);
        }
    }
}
