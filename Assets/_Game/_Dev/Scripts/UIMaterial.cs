using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using com.Pizia.Tools;
using com.Pizia.Saver;

public class UIMaterial : CachedReferences
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text amountTxt;

    //======================================================================

    public void Initialize(Sprite _icon, int _amount)
    {
        icon.sprite = _icon;
        amountTxt.text = _amount.ToString();
    }

    public void UpdateData(int _amount)
    {
        amountTxt.text = _amount.ToString();
    }
}
