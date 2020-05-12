using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraDrag : MonoBehaviour
{
    public static CameraDrag instance;
    public bool lockX = false;
    public bool lockY = false;
    public bool useLimits = false;
    public bool followTarget = false;
    
    [SerializeField] private float _minLocX = -3;
    [SerializeField] private float _maxLocX = 3;
    [SerializeField] private float _minLocY = -5;
    [SerializeField] private float _maxLocY = 5;

    [SerializeField] Text _zoomBtn;
    string _zoomInLabel = "Zoom In";
    string _zoomOutLabel = "Zoom Out";
    [SerializeField] private float _zoomOut = 5;
    private float _zoomOrigin;
    
    private Vector3 _resetCamera;
    private Camera _camera;
    private Vector3 _origin;
    private Vector3 _difference;
    private bool _drag = false;
    
    [SerializeField] Transform _target;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float _smoothTime = 2f;
    

    // GETTERS ET SETTERS
    // public float MinX { }
    // public float MaxX { }
    // public float MinY { }
     public float MaxY { 
         get {return _maxLocY;}
         set {_maxLocY = value;}
     }
    // public Transform Target { }
    void Awake()
    {
        instance = this;
    }
    void Start () {
        _camera = Camera.main;
        _zoomOrigin = _camera.orthographicSize;
        _resetCamera = _camera.transform.position;
        instance = this;
        _maxLocY += Build.instance.EtageList.Count;
    }
    void LateUpdate ()
    {
        Vector3 position = transform.position;
        
        if (followTarget && _target != null)
        {
            Vector3 targetPosition = new Vector3(0,_target.transform.position.y+4.5f, position.z);
            Vector3 newPos = Vector3.SmoothDamp(position, targetPosition, ref _velocity, _smoothTime);
            newPos.z = position.z;
            transform.position = ApplyLocConstraints(newPos);
        }
        else
        {
            if (Input.GetMouseButton(0)) {            
                _difference = _camera.ScreenToWorldPoint(Input.mousePosition) - _camera.transform.position;
                if (lockX)
                {
                    _difference.x = 0;
                }
                if (!_drag) {
                    _drag=true;
                    _origin = _camera.ScreenToWorldPoint(Input.mousePosition);
                }
            } else {
                _drag=false;
            }
            if (_drag)
            {
                Vector3 newLoc = _origin - _difference;

                newLoc = ApplyLocConstraints(newLoc);
            
                _camera.transform.position = newLoc;
            }
        
            if (Input.GetMouseButton (1)) {
                _camera.transform.position=_resetCamera;
            }
        }
    }

    public void ZoomOut()
    {
        _camera.orthographicSize = _zoomOut;
        _zoomBtn.text = _zoomInLabel;
        
    }
    public void ZoomIn()
    {
        _camera.orthographicSize = _zoomOrigin;
        _zoomBtn.text = _zoomOutLabel;
    }

    Vector3 ApplyLocConstraints(Vector3 loc)
    {
        if (lockX)
        {
            loc.x = _camera.transform.position.x;
        }
            
        if (lockY)
        {
            loc.y = _camera.transform.position.y;
        }

        if (useLimits)
        {
            float cameraHalfHeight = _camera.orthographicSize;
            float cameraHalfWitdh = _camera.aspect * cameraHalfHeight;

            if (!lockX)
            {
                loc.x = Mathf.Clamp(loc.x, _minLocX + cameraHalfWitdh, _maxLocX - cameraHalfWitdh);
            }
            
            if (!lockY)
            {
                loc.y = Mathf.Clamp(loc.y, _minLocY + cameraHalfHeight, _maxLocY - cameraHalfHeight);
            }
        }        

        return loc;
    }
}
