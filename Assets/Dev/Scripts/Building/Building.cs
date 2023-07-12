using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class Building: MonoBehaviour
{
    public string buldingName;
    [SerializeField] protected float initHP;
    public float currentHP;
    public GameObject buildingModel;
    public int level;
    public List<BuildingUpgrade> buildingUpgrades;

    public int _Level{ get=> level; set{
        level = value;
    }}

    // UI Buy/Upgrade

    public GameObject canvas;
    public List<GameObject> horizontalPanels;
    public List<GameObject> coinsPanels;
    public List<Image> coinsImage;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<HPlayer>(out HPlayer hPlayer))
        {
            SetCanvas();
        }
    }

    // Buy Upgrade

    private void SetCanvas(){
        int coinsRequired = buildingUpgrades[level].cost;
        int activePanels = Mathf.CeilToInt(coinsRequired / 5.0f);

        for(int i=0; i < activePanels; i++) horizontalPanels[i].SetActive(true);
        for (int i = 0; i < coinsRequired; i++) coinsPanels[i].SetActive(true);
    }

    private int coinIteration = 0;

    protected void TimeWaitToDoAction(Action _action)
    {
        if (!LeanTween.isTweening(gameObject) && Mathf.Abs(HPlayer.instance.simpleMovement.rb.velocity.x) < 1)
        {
            canvas.gameObject.SetActive(true);
            // LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float value) => { timeImg.fillAmount = value; }).setOnComplete(() =>
            // {
            //     // timeImg.fillAmount = 0;
            //     canvas.gameObject.SetActive(false);
            // });
        }
        if (HPlayer.instance.simpleMovement.rb.velocity != Vector3.zero) CancelTimeToDoAction();
    }

    void FillCoin(Image coinImage)
    {
        LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float value) => { coinImage.fillAmount = value; }).setOnComplete(() =>
        {
            coinIteration++;
            if(coinIteration < buildingUpgrades[level].cost){
                // FillCoin();

            } else{

            }
            canvas.gameObject.SetActive(false);
        });
    }

    protected void CancelTimeToDoAction()
    {
        // timeImg.fillAmount = 0;
        canvas.gameObject.SetActive(false);
        LeanTween.cancel(gameObject);
    }
}

[System.Serializable]
public class BuildingUpgrade{
    public int level;
    public int cost;

}
