using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;




public class CitizenManager : MonoBehaviour
{
    public static CitizenManager instance;

    [SerializeField] Elevator _elevator;
    [SerializeField] Build _build;
    [SerializeField] Citizen citizen;
    
    public Citizen Citizen
    {
        get { return citizen; }
    }
    List<Citizen> _citizenList = new List<Citizen>();
    public List<Citizen> citizenList
    {
        get { return _citizenList; }
        set { _citizenList = value;  }
    }

    Citizen citizenUp = null;
    public Citizen CitizenUp
    {
        get { return citizenUp; }
        set {citizenUp = value; }
    }
    bool citizenSpawned = false;
    public bool CitizenSpawned { get { return citizenSpawned; } }
     
    List<Citizen> citizens = new List<Citizen>();
    Vector2 _elevatorPos;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Build.instance.towerResetEvent.AddListener(TowerCleared);
        _elevatorPos = _elevator.transform.position;
        _elevator.GetComponent<Elevator>();
        _build.GetComponent<Build>();
        
        StartCoroutine("NewCitizen");
    }
    
    // Update is called once per frame
    void Update()
    {
    
   
    }
    void CanSpawn(bool value)
    {
        StartCoroutine("NewCitizen");
    }
    public IEnumerator NewCitizen()
    {
        while (true) {
            if (citizenUp == null && Build.instance.NbFloorReady> 0) {
                yield return new WaitForSeconds(Random.Range(1, 10));
                //Instanci un personnage et 
                _citizenList.Add(citizenUp = Instantiate(citizen, _elevatorPos, Quaternion.identity, _elevator.transform));
                
                
                Debug.Log("le nouveau citizen" + CitizenUp);
            }
            yield return null;
        }
    }
    void TowerCleared()
    {
      StopCoroutine(NewCitizen());
    }
}
