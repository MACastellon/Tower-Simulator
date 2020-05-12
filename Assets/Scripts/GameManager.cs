using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject build;
    [SerializeField] GameObject citizenManager;
    [SerializeField] GameObject elevator;
    // Start is called before the first frame update
    void Start()
    {
        build.GetComponent<Build>();
        citizenManager.GetComponent<CitizenManager>();
        elevator.GetComponent<Elevator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
