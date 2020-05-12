using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class CitizenInfo
{
    
    public string citizenFirstName;
    public string citizenLastName;
    public  int floorNb;
    public string nameLabel;
    public Sprite citizenSprite;

    public CitizenInfo(string firstName, string lastName ,int floor, string citizenNameLabel, Sprite theSprite)
    {
        citizenFirstName = firstName;
        citizenLastName = lastName;
        floorNb = floor;
        nameLabel = citizenNameLabel;
        citizenSprite = theSprite;
    }
}
public class Citizen : MonoBehaviour
{
    [SerializeField] List<GameObject> _listgm = new List<GameObject>();
    
    Etage floor;
    Vector3 home;
    [SerializeField] Text _nameLabel;
    public Text nameLabel{
        get {return _nameLabel;}
        set {_nameLabel = value;}
    }
    [SerializeField] List<string> lastNameList = new List<string>();
    [SerializeField] List<string> firstNameList = new List<string>();
    [SerializeField] List<Sprite> citizenSriteList;
    string lastName;
    string firstName;
    public int floorWanted;

    private CitizenInfo _info;
    
    public CitizenInfo info {
        get {return _info;}
        set {_info = value;}
    }
    SpriteRenderer _sren;

    
    // Start is called before the first frame update
    void Start()
    {
        _sren = GetComponent<SpriteRenderer>();
        if (_info == null) {
            NewCitizenInfo();
        } else {
            LoadCitizenInfo();
        }
    }
    public void NewCitizenInfo()
    {
        lastName = lastNameList[Random.Range(0, lastNameList.Count)];
        firstName = firstNameList[Random.Range(0, firstNameList.Count)];
        floorWanted = Random.Range(0, Build.instance.EtageList.Count);
        Elevator.instance.FloorWantedPos = Build.instance.EtageList[floorWanted].transform.position;
        home = Build.instance.EtageList[floorWanted].transform.position;
        Debug.Log("I want to go " + floorWanted + " a pos est " + Elevator.instance.FloorWantedPos);
        this.name = firstName + " " + lastName;
        _nameLabel.text = this.name;
        _sren.sprite = citizenSriteList[Random.Range(0,citizenSriteList.Count)];
        
    }
    public void LoadCitizenInfo()
    {
        lastName = _info.citizenLastName;
        firstName = _info.citizenFirstName ;
        floorWanted = _info.floorNb;
        this.name = firstName + " " + lastName;
        _nameLabel.text = this.name;
        _sren.sprite = info.citizenSprite;
        MoveIn();
    }

    public CitizenInfo GetInfo()
    {
        CitizenInfo info = new CitizenInfo(firstName, lastName, floorWanted ,this.name , _sren.sprite);
        return info;
    }
    // Update is called once per frame
    void Update()
    {
      
    }
    private void OnMouseUp()
    {
        if (transform.parent == Elevator.instance.transform)
        {
            Elevator.instance.MoveElevator();
            CameraDrag.instance.followTarget = true;
        }
        else
        {
            Debug.Log(this.name);

        }

    }
    public void MoveIn()
    {
        StartCoroutine("LiveYourLife");
    }
    IEnumerator LiveYourLife ()
    {
        transform.parent = Build.instance.EtageList[floorWanted].transform;
        Build.instance.EtageList[floorWanted ].floorPopulation.Add(this);
        floor = Build.instance.EtageList[floorWanted].GetComponent<Etage>();
        transform.position = floor.waypoints[0].transform.position;
        yield return new WaitForSeconds(0.5f);
        Vector3 randomPosToGo = floor.waypoints[Random.Range(0, floor.waypoints.Count - 1)].transform.position;
         
        while (true)
        {
            if (transform.position != randomPosToGo)
            {
                transform.position = Vector3.MoveTowards(transform.position, randomPosToGo, 1 * Time.deltaTime);
                  
                if (transform.position.x <  randomPosToGo.x) {
                    _sren.flipX = false;
                } else {
                    _sren.flipX = true;
                }
                
            }
            else {
                yield return new WaitForSeconds(0.5f);
                randomPosToGo = floor.waypoints[Random.Range(0, floor.waypoints.Count - 1)].transform.position;
            }
            yield return null;
        }

        
    }

}
