using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;

[System.Serializable]
public class BuildInfo
{
   
    public FloorInfo[] floors;
    public int priceFloorAppartment;
    public int priceFloorCommercial;
    public string saveTime;
    public MoneyManagerInfo moneyManagerInfo;
    public BuildInfo(List<Etage> floorsList , MoneyManager moneyManager, int priceAppartment,int priceCommercial)
    {
        floors = new FloorInfo[floorsList.Count];
       
        for (int i = 0; i < floorsList.Count; i++) {
            floors[i] = floorsList[i].GetInfo();
        }
        saveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        moneyManagerInfo = moneyManager.GetInfo();
        priceFloorAppartment = priceAppartment;
        priceFloorCommercial = priceCommercial;
        
        
    }
}
public class Build : MonoBehaviour
{
    public static Build instance;
    public UnityEvent towerResetEvent = new UnityEvent();
    [SerializeField] MoneyManager _moneyManager;
    [SerializeField] GameObject _towerSpawn;
    
    [SerializeField] Etage _floor;
    [SerializeField] Etage _commercialFloor;
    SpriteRenderer _floorSpriteRen;
    [SerializeField] Text nextFloorPriceLabel;
    [SerializeField] Text infoMessage;
    [SerializeField] int priceFloorAppartment = 1000;
    [SerializeField] int priceFloorCommercial = 1750;
   
   int floorHeigh;
   public int FloorPrice
    {
        get { return FloorPrice; }
    }
    Vector2 floorPos;
    
    int moneyMissing;

    List<Etage> _etageList = new List<Etage>();
    public List<Etage> EtageList {
        get { return _etageList; }
    }
 
    int _nbEtage;
    public int nbEtage { get { return _nbEtage; } //Getter du nombre d'étages actif
    }

    int nbFloorReady; // Nombre de floor pret
    public int NbFloorReady // getter setter du nombre de floor pret
    {
        get { return nbFloorReady; }
        set { nbFloorReady = value; }
    }
    private BuildInfo _buildinfo;
    private FloorInfo _info;

    public FloorInfo info
    {
        get { return _info; }
        set { _info = value; }
    }

     float _timeSaved = 0;
     public float timeSaved {
         get {return _timeSaved;}
     }

    void Awake()
{
        instance = this;
    
}
    void Start()
    {
        _floorSpriteRen = _floor.GetComponent<SpriteRenderer>();
        _moneyManager.GetComponent<MoneyManager>();
        floorPos = _towerSpawn.transform.position;
       
    }
    // Load toutes les infos sauver
    public void LoadInfo(BuildInfo info)
    {
        DateTime saveTime = DateTime.Parse(info.saveTime);
        DateTime currentTime = DateTime.Now;
        TimeSpan timeDifference = currentTime - saveTime;
        _timeSaved = (float)(currentTime - saveTime).TotalSeconds;
        foreach (FloorInfo floorinfo in info.floors)
        {
            Floortype type;
            type = floorinfo.type;
            Etage theFloor;
            switch(type)
            {
                case Floortype.Appartment:
                    theFloor = Instantiate(_floor, new Vector3(floorinfo.x, floorinfo.y, floorinfo.z), Quaternion.identity);
                    theFloor.info = floorinfo;
                    EtageList.Add(theFloor);
                    theFloor.LoadInfo(floorinfo);
               
                    NbFloorReady++;
                    Debug.Log(theFloor.type);
                break;
                case Floortype.Commercial:
                    theFloor = Instantiate(_commercialFloor, new Vector3(floorinfo.x, floorinfo.y, floorinfo.z), Quaternion.identity);
                    theFloor.info = floorinfo;
                    EtageList.Add(theFloor);
                    theFloor.LoadInfo(floorinfo);
                    NbFloorReady++;
                    break;
            }
        }
       
        Debug.Log(("Temps écouler depuis la dernière connexion " + (currentTime - saveTime).TotalSeconds));
        MoneyManager.instance.TotalMoney = info.moneyManagerInfo.totalMoney;
        MoneyManager.instance.TotalMoney +=  _etageList.Count * (int)timeDifference.TotalSeconds;
        Debug.Log(("Argent généré : " + (_etageList.Count * (int)timeDifference.TotalSeconds) + " $"));
        CameraDrag.instance.MaxY += info.floors.Length;
        priceFloorAppartment = info.priceFloorAppartment;
        priceFloorCommercial = info.priceFloorCommercial;
        nextFloorPriceLabel.text = "Next : "+ priceFloorAppartment +"$";
    }
    public BuildInfo GetInfo()
    {
        BuildInfo info = new BuildInfo(EtageList , MoneyManager.instance, priceFloorAppartment, priceFloorCommercial);
        Debug.Log(priceFloorAppartment);
        return info;
    }
    public void ResetTower()
    {
        towerResetEvent.Invoke();
        _etageList = new List<Etage>();
        NbFloorReady = 0;
        priceFloorAppartment = 0;
        priceFloorCommercial = 0;
    }
   
    //Build apartment floor
    public void BuildFloor () 
    {
       // int floorStartPrice = 0;
        
        float _towerSpawnPosX = _towerSpawn.transform.position.x; // Position x du spawn de la tour
        float _towerSpawnPosY = _towerSpawn.transform.position.y + (_etageList.Count * (_floorSpriteRen.size.y * 98 / 100) + (_floorSpriteRen.size.y * 98 / 100)); // La position y du spawn de la tour
        if (_nbEtage == 0)
        {
            floorPos = new Vector2(_towerSpawnPosX, _towerSpawnPosY);// Position d'instantiation des étages de la tour
            _etageList.Add( Instantiate(_floor, floorPos, Quaternion.identity)); //Instantiation des étages de la tour.
            _nbEtage++;
            priceFloorAppartment = 1000;
            nextFloorPriceLabel.text = "Next : "+ priceFloorAppartment + "$";
        }
        else if(_moneyManager.TotalMoney >= priceFloorAppartment)
        {
           floorPos = new Vector2(_towerSpawnPosX, _towerSpawnPosY);// Position d'instantiation des étages de la tour
            _etageList.Add(Instantiate(_floor, floorPos, Quaternion.identity)); //Instantiation des étages de la tour.
            _nbEtage++;
            _moneyManager.TotalMoney -= priceFloorAppartment;
            priceFloorAppartment += priceFloorAppartment * 10/100;
           
            nextFloorPriceLabel.text = "Next : " + priceFloorAppartment + "$";
            CameraDrag.instance.MaxY = _towerSpawnPosY;
        }
        else
        {
            StartCoroutine(CouroutineMessage((priceFloorAppartment - MoneyManager.instance.TotalMoney).ToString()+" $ to build this apartment floor!"));
        }
       

    }
    //Build commercial floor
    public void BuildCommercial() {
        
        
        float _towerSpawnPosX = _towerSpawn.transform.position.x; // Position x du spawn de la tour
        float _towerSpawnPosY = _towerSpawn.transform.position.y + (_etageList.Count * (_floorSpriteRen.size.y * 98 / 100) + (_floorSpriteRen.size.y * 98 / 100)); // La position y du spawn de la tour
       
        if(_moneyManager.TotalMoney >= priceFloorCommercial)
        {
           floorPos = new Vector2(_towerSpawnPosX, _towerSpawnPosY);// Position d'instantiation des étages de la tour
            _etageList.Add(Instantiate(_commercialFloor, floorPos, Quaternion.identity)); //Instantiation des étages de la tour.
            _nbEtage++;
            _moneyManager.TotalMoney -= priceFloorCommercial;
            priceFloorCommercial += priceFloorCommercial * 10/100;
            //nextFloorPriceLabel.text = "Next : " + floorPrice + "$";
            CameraDrag.instance.MaxY += 1f;
        }
        else
        {
            StartCoroutine(CouroutineMessage((priceFloorCommercial - MoneyManager.instance.TotalMoney).ToString()+" $ to build this commercial floor!"));
        }
       
   
        
    }
    //Couroutine qui affiche un message voulu;
    private IEnumerator CouroutineMessage( string message)
    {
        //int total = _moneyManager.TotalMoney -= FloorPrice;
        infoMessage.text = "You're missing "+message;
        yield return new WaitForSeconds(3f);
        infoMessage.text = "";
    }
}
