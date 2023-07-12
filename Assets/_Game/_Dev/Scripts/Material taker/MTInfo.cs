using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;

public class MTInfo : CachedReferences
{
    public SpriteRenderer panelRend;
    public NeedInfo[] needInfos;

    float panelSize = 2.08f;
    float panelHeightSum = 0.96f;

    public void SetStepInfo(Step _step)
    {
        int count = 0;
        for (int i = 0; i < _step.needs.Length; i++)
        {
            if (_step.needs[i].amount == 0)
            {
                count++;
            }
        }
        if (count == _step.needs.Length)
        {
            gameObject.SetActive(false);
        }

        int currentSteps = _step.needs.Length;
        panelRend.size = new Vector2(panelSize, panelSize + (panelHeightSum * (currentSteps - 1)));

        for (int i = 0; i < needInfos.Length; i++)
        {
            if (i < currentSteps)
            {
                needInfos[i].gameObject.SetActive(true);
                needInfos[i].SetIcon(_step.needs[i].relatedMaterial.icon);
                needInfos[i].SetText(_step.needs[i].amount.ToString());
            }
            else
            {
                needInfos[i].gameObject.SetActive(false);
            }
        }
    }

    public void AnimateIn()
    {

    }
    public void AnimateOut()
    {

    }
}
