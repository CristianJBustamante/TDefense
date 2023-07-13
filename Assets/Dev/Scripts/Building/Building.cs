using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class Building : MonoBehaviour
{
    public string buldingName;
    [SerializeField] protected float initHP;
    public float currentHP;
    public GameObject buildingModel;
    public int level;
    public List<BuildingUpgrade> buildingUpgrades;
    public Module myModule;

    public int _Level
    {
        get => level; set
        {
            switch(value){
                case 1:
                    construction.SetActive(false);
                    buildingModel.SetActive(true);
                    myModule._ModuleState = ModuleState.Unlocked;
                break;
            }
            level = value;
        }
    }

    public GameObject construction;

    // UI Buy/Upgrade

    public GameObject canvas;
    public List<GameObject> horizontalPanels;
    public List<GameObject> coinsPanels;
    public List<Image> coinsImages;

    private void OnTriggerEnter(Collider other){
        if (other.TryGetComponent<HPlayer>(out HPlayer hPlayer))
        {
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<HPlayer>(out HPlayer hPlayer))
        {
            if(AvailableToUnlock()) TimeWaitToDoAction(UpgradeBuilding);
        }
    }

    // Buy Upgrade

    private void SetCanvas()
    {
        int coinsRequired = buildingUpgrades[level].cost;
        int activePanels = Mathf.CeilToInt(coinsRequired / 5.0f);

        for (int i = 0; i < activePanels; i++) horizontalPanels[i].SetActive(true);
        for (int i = 0; i < coinsRequired; i++) coinsPanels[i].SetActive(true);
        canvas.gameObject.SetActive(true);
    }

    private void HideCanvas(){
        foreach(GameObject go in horizontalPanels) go.SetActive(false);
        foreach(GameObject go in coinsPanels) go.SetActive(false);
        foreach(Image i in coinsImages) i.fillAmount = 0;
    }

    private int coinIteration = 0;
    private bool isTweening;

    protected void TimeWaitToDoAction(Action _action)
    {
        Debug.Log("!LeanTween.isTweening(gameObject): " + !LeanTween.isTweening(gameObject));
        Debug.Log(HPlayer.instance.simpleMovement.rb.velocity == Vector3.zero);
        if (!isTweening && HPlayer.instance.simpleMovement.rb.velocity == Vector3.zero)
        {
            isTweening = true;
            FillCoin(coinsImages[coinIteration], _action);
            SetCanvas();
        }
        if (HPlayer.instance.simpleMovement.rb.velocity != Vector3.zero){
            CancelTimeToDoAction();
            HideCanvas();
            coinIteration = 0;
        } 
    }

    void FillCoin(Image coinImage, Action _action)
    {
        LeanTween.value(gameObject, 0, 1, 0.5f).setOnUpdate((float value) => { coinImage.fillAmount = value; }).setOnComplete(() =>
        {
            coinIteration++;
            if (coinIteration < buildingUpgrades[level].cost)
            {
                FillCoin(coinsImages[coinIteration], _action);

            }
            else
            {
                _action();
                coinIteration = 0;
                HideCanvas();
                _Level++;
            }
        });
    }

    protected void CancelTimeToDoAction()
    {
        // timeImg.fillAmount = 0;
        canvas.gameObject.SetActive(false);
        LeanTween.cancel(gameObject);
        isTweening = false;
    }

    void UpgradeBuilding(){
        Debug.Log("Building Upgraded");
    }

    public void UnlockNextUpgrade(){
        buildingUpgrades[level].unlocked = true;
    }

    public bool AvailableToUnlock(){
        return level >= buildingUpgrades.Count? false : buildingUpgrades[level].unlocked;
    }
}


[System.Serializable]
public class BuildingUpgrade
{
    public int cost;
    public bool unlocked;
}
