using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CommercialFloor : Etage
{
    [SerializeField] private int pricePerDay = 2000;
    [SerializeField] private int salary = 50;


    // Start is called before the first frame update
    void Start()
    {
        Build.instance.towerResetEvent.AddListener(DestroyFloor);
        type = Floortype.Commercial;
         if (_info == null)
        {
            StartCoroutine(Activetime(buildTime * 2));
        }
        else
        {
            LoadFloorInfo(info.buildTime);
        }
        StartCoroutine(RantCoroutine());
        StartCoroutine(GainMoneyCouroutine());
        Debug.Log(type);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RantCoroutine()
    {
        while(true)
        {
            if (IsReady)
            {
                MoneyManager.instance.TotalMoney -= pricePerDay;
                yield return new WaitForSecondsRealtime(60);
            }
            yield return null;
            
        }
        
    }
    IEnumerator GainMoneyCouroutine()
    {
        if (IsReady)
        {
             while (true)
            {
            MoneyManager.instance.TotalMoney += salary * floorPopulation.Count;
            yield return new WaitForSeconds(30);
            }
        }
       yield return null;
    }
}
