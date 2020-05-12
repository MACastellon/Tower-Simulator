using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    static SaveManager instance;
     static GameObject _startCanvas = null;
    
    string _game = "Game";
    string _nomSave = "TowerSave";
    string _saveName = "towerContent";
    public string _gameSaved;
     string gameSaved {
        get { return _gameSaved; }
        set { _gameSaved = value; }
    }
    int _version = 1;
    private void Awake()
    {
        
        DontDestroyOnLoad(this);
        if (instance == null) {
            instance = this;
            _startCanvas = transform.GetChild(0).gameObject;
            Debug.Log("I am the save manager");
        } else {
            Destroy(gameObject);
            Debug.Log("new save manager");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       //LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Load toutes les informations sauvegarder
    /// </summary>
    void LoadGame()
    {
        gameSaved = PlayerPrefs.GetString(_nomSave + "_v5");

        Debug.Log(gameSaved);
        if (gameSaved != "") {
            Debug.Log("allo");
            BuildInfo info = JsonUtility.FromJson<BuildInfo>(gameSaved);
            Build.instance.LoadInfo(info);
        }
    }
    /// <summary>
    /// Sauvegarde la parti
    /// </summary>
    public void Save()
    {
        
        BuildInfo info = Build.instance.GetInfo();
        string save = JsonUtility.ToJson(info );
        PlayerPrefs.SetString(_nomSave + "_v5", save);
        Debug.Log(save);
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene(_game);
        Debug.Log(gameSaved);
        _startCanvas.SetActive(false);
        
    }
    public void ContinueGame()
    {
        SceneManager.LoadScene(_game);
       StartCoroutine(SceneLoad(_startCanvas, false, true));
       // _startCanvas.SetActive(false);


    }
    public void Menu () 
    {
        SceneManager.LoadScene("Start");
        //_startCanvas = transform.GetChild(0).gameObject;
       //StartCoroutine(SceneLoad(_startCanvas, true, false));
        _startCanvas.SetActive(true);
        
    }
    IEnumerator SceneLoad(GameObject obj, bool state, bool load)
    {
        yield return new WaitForSeconds(0.001f);
        obj.SetActive(state);
        if (load) {
            LoadGame();    
        }
        

    }
    
}
