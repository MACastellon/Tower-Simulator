using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }
public class Elevator : MonoBehaviour
{
    public static Elevator instance; // Instance de Elevator
    Vector3 _originPos;
    Vector3 elevatorPos;
    Vector3 floorWantedPos;
    public Vector3 FloorWantedPos
    {
        get {return floorWantedPos;}
        set {floorWantedPos = value;}
    }

 

   
    
    private void Awake()
    {
        
        instance = this;
    }
    void Start()
    {
        Build.instance.towerResetEvent.AddListener(ClearElevator);
        elevatorPos = this.transform.position;
        _originPos = transform.position;
        Debug.Log(_originPos);
       
    }

    // Update is called once per frame
    void  Update()
    {
        

    }
    public void MoveElevator()
    {



        StartCoroutine("CitizenToNewFloor");
        Debug.Log("Elevator has someone");
       
        
        
        

        //CitizenManager.instance.CitizenUp = null;


        //StartCoroutine("CitizenToNewFloor");
    }
    IEnumerator CitizenToNewFloor ()
    {
       
        while (elevatorPos.y < floorWantedPos.y) {
            if (CitizenManager.instance.CitizenUp != null)
            {
                elevatorPos = transform.position;
                transform.Translate(0, 1 * Time.deltaTime, 0);
                //elevatorPos += floorWanted * Time.deltaTime;
                
            }

            yield return null; 
        }
        yield return new WaitForSeconds(2f);
        CitizenManager.instance.CitizenUp.MoveIn();
        yield return new WaitForSeconds(2f);
        if (elevatorPos.y >= floorWantedPos.y)
        {
            StartCoroutine("CitizenArrived");
        }


    }
    IEnumerator CitizenArrived()
    {
        while (elevatorPos.y > _originPos.y) {
            transform.Translate(0, -1 * Time.deltaTime, 0);
            elevatorPos = transform.position;
            Debug.Log("Arriver");
            yield return null;
        }
        CitizenManager.instance.CitizenUp = null;
        yield return new WaitForSeconds(1f);
        CameraDrag.instance.followTarget = false;

    }
    void ClearElevator()
    {
        if (this.GetComponentInChildren<Citizen>())
        {
            Destroy(CitizenManager.instance.CitizenUp.gameObject);
        }
        else{
            
        }
       
        
    }



}
