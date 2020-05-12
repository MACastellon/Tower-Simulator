using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class MoneyManagerInfo
{
    public int totalMoney;
    public MoneyManagerInfo(int totalMoneyInfo)
    {
        totalMoney = totalMoneyInfo;
    }
}

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    [SerializeField] Text moneyLabel;
    
    string totalMoneyShown;

    int floorPrice;
    [SerializeField] int totalMoney;
    public int TotalMoney
    {
        get { return totalMoney; }
        set { totalMoney = value; }
    }
    float moneyUpdateTime = 1;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("MoneyUpdateCouroutine");
        Build.instance.towerResetEvent.AddListener(TowerCleared);
 
        totalMoney += 0;
        totalMoneyShown = totalMoney.ToString();
        moneyLabel.text = totalMoneyShown + " $";

    }
    public MoneyManagerInfo  GetInfo() {
        MoneyManagerInfo info = new MoneyManagerInfo(totalMoney); 
        return info;
    }

    // Update is called once per frame


    IEnumerator MoneyUpdateCouroutine() {

        while (true)
        {
             totalMoney += 50 * Build.instance.NbFloorReady;
          
            totalMoneyShown = totalMoney.ToString();
            moneyLabel.text = totalMoneyShown + " $";
           
            yield return new WaitForSeconds(1f);
        }
    }

    void TowerCleared () {
        totalMoney = 0 ;
    } 
}
