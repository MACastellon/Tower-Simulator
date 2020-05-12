using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorInfo
{
    public float x;
    public float y;
    public float z;
    public float r;
    public float v;
    public float b;
    public Floortype type;
    public float buildTime;
    public float savedTime;

    public CitizenInfo[] citizenInFloor;
    public FloorInfo(Vector3 position, Color color, List<Citizen> citizenList , Floortype theType , float theBuildingTime)
    {
        x = position.x;
        y = position.y;
        z = position.z;

        r = color.r;
        v = color.g;
        b = color.b;
        buildTime = theBuildingTime;

        type = theType;

        citizenInFloor = new CitizenInfo[citizenList.Count];
        for (int i = 0; i < citizenList.Count; i++)
        {
            citizenInFloor[i] = citizenList[i].GetInfo();
        }
    }
}
public class Etage : MonoBehaviour
{

    [SerializeField] List<GameObject> _waypoints = new List<GameObject>();
    public List<GameObject> waypoints { get { return _waypoints; } }

    SpriteRenderer _spriteRen;
    public SpriteRenderer spriteRen
    {
        get { return _spriteRen; }
        set { _spriteRen = value; }
    }
    Vector3 floorPos;
    public Vector3 FloorPos
    {
        get {return floorPos;}
    }
  
    
   [SerializeField] List<Color> c = new List<Color>();
   protected List<Citizen> _floorPopulation = new List<Citizen>();
   public List<Citizen> floorPopulation
    {
        get { return _floorPopulation; }
        set { _floorPopulation = value; }
    }


    Color randomColor;
    public Color _randomColor 
    {
        get {return randomColor;}
        set {randomColor = value;}
    }

    private  bool isReady =false;
    public bool IsReady 
    {
        get {return isReady;}
        set {isReady = value;}
    }
    protected FloorInfo _info;
    public FloorInfo info
    {
        get { return _info; }
        set { _info = value; }
    }
    private Floortype _type = Floortype.Appartment;
    public Floortype type
    {
        get{return _type;}
        set{_type = value;}
    }
    protected float buildTime = 10;


    void Awake() {
        AddColor();
    }


    // Start is called before the first frame update
    void Start()
    {
        if (_info == null)
        {
            Debug.Log(_info);
            StartCoroutine(Activetime(buildTime));
        }
        else
        {
            buildTime = info.buildTime;
            LoadFloorInfo(buildTime);
        }
    
        Build.instance.towerResetEvent.AddListener(DestroyFloor);
        _spriteRen = GetComponent<SpriteRenderer>();
 
       
    }
    /// <summary>
    /// Load floor info when the game start
    /// </summary>
    /// <param name="info">Floor informations</param>
    public void LoadInfo(FloorInfo info)
    {
        //Instantiate every citizens the floor had when saved
        foreach (CitizenInfo citizenInfo in info.citizenInFloor)
        {
            //citizenInfo.
            Citizen theCitizen = Instantiate(CitizenManager.instance.Citizen, new Vector3(waypoints[0].transform.position.x, waypoints[0].transform.position.y, waypoints[0].transform.position.z), Quaternion.identity);
            theCitizen.info = citizenInfo;
        }
    }
    public FloorInfo GetInfo()
    {
        FloorInfo info = new FloorInfo(transform.position, randomColor , floorPopulation , _type, buildTime);
        return info;
    }
    protected void LoadFloorInfo(float buildingTime)
    {
        if (buildingTime <= 0 ){
            isReady=true;
            SetColor(new Color(_info.r, _info.v, _info.b));
            Debug.Log("Jetais pret");
        } else {
           Debug.Log("pas fini load info");
            StartCoroutine(Activetime(buildingTime));
        }
       
    }
    void RandomColor()
    {
        SetColor(c[Random.Range(0,c.Count)]);
 
    }

    void SetColor(Color rgb) {
        randomColor = rgb;
        GetComponent<SpriteRenderer>().color = rgb;
    }

    protected IEnumerator Activetime(float time) {
        Debug.Log("Couroutine Active");
        Debug.Log(isReady);
        while (isReady != true)
        {
            time -= Time.deltaTime;
            buildTime = time; 
            if(time <= 0  || Build.instance.timeSaved > time){
                RandomColor();
                isReady = true;
                Build.instance.NbFloorReady++;
                buildTime = 0;
                Debug.Log(isReady);
            }
            yield return null;
        }
    }
    protected void DestroyFloor()
    {
        Destroy(gameObject);
    }
    void AddColor(){
        c.Add(new Color(0, 0, 0, 1));
        c.Add(new Color(0, 0, 1, 1));
        c.Add(new Color(0, 1, 1, 1));
        c.Add(new Color(0, 1, 0, 1));
        c.Add(new Color(1, 0, 0, 1));
        c.Add(new Color(1, 0.92f, 0.016f, 1));

    }
}
//enum that determine the type of floor
public enum Floortype {Appartment , Commercial};
