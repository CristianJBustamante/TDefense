using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using com.Pizia.Saver;
using com.Pizia.Tools;

public class MaterialTaker : MonoBehaviour
{
    public UniqueID uid;
    public Step[] steps;
    public bool initializeAutomatic = true;

    [Header("Debug Info")]
    public bool completed;
    public bool ready = true;
    public bool playerInside;
    public int currentStep;

    [Header("Components")]
    public Transform targetPoint;
    public InteractionArea interactionArea;
    public MTInfo mtInfo;

    public UnityEvent NeedsCompleted, AllNeedsCompleted;
    public delegate void OnInitialized();
    public OnInitialized onInitialized;

    float startTime;
    float delayBetweenTicks = 0.06f;

    //----------------------------------------------------

    private void Awake()
    {
        if (initializeAutomatic) LoadData();
    }

    private void OnEnable()
    {
        interactionArea.onEnter += ResetMTInfo;
    }
    private void OnDisable()
    {
        interactionArea.onEnter -= ResetMTInfo;
    }

    void Update()
    {
        if (!playerInside || !ready)
        {
            return;
        }
        if (Time.time - startTime > delayBetweenTicks)
        {
            startTime = Time.time;
            for (int i = 0; i < GetTotalMaterialsNeeded(); i++)
            {
                var amountToTake = 1;
                if (steps[currentStep].needs[i].amount < 50) amountToTake = 1;
                else if (steps[currentStep].needs[i].amount < 100) amountToTake = 2;
                else if (steps[currentStep].needs[i].amount < 140) amountToTake = 3;
                else if (steps[currentStep].needs[i].amount < 170) amountToTake = 4;
                else if (steps[currentStep].needs[i].amount < 200) amountToTake = 5;
                else amountToTake = steps[currentStep].needs[i].amount / 40;

                var material = steps[currentStep].needs[i].relatedMaterial;
                var invMat = MaterialManager.instance.GetInventory()[material.name];

                if (amountToTake > invMat.amount)
                {
                    amountToTake = Mathf.Max(invMat.amount, 1);
                }

                if (steps[currentStep].needs[i].amount > 0 && MaterialManager.instance.Remove(material, amountToTake))
                {
                    ParticleFeelManager.instance.FromPlayer(targetPoint.position, steps[currentStep].needs[i].relatedMaterial);

                    steps[currentStep].needs[i].amount -= amountToTake;

                    if (steps[currentStep].needs[i].amount == 0)
                    {
                        if (CheckForCompletion()) break;
                    }
                }
            }

            mtInfo.SetStepInfo(steps[currentStep]);
            if (!completed)
            {
                CheckForCompletion();
            }
        }
    }

    //----------------------------------------------------

    public void LoadData()
    {
        completed = SaveManager.LoadBool(uid + "completed");
        currentStep = SaveManager.LoadInt(uid + "step");
        for (int i = 0; i < steps.Length; i++)
        {
            for (int j = 0; j < steps[i].needs.Length; j++)
            {
                if (SaveManager.HasKey(uid + "step " + i + "need " + j))
                    steps[i].needs[j].amount = SaveManager.LoadInt(uid + "step " + i + "need " + j);
            }
        }
        interactionArea.gameObject.SetActive(!completed);
        mtInfo.gameObject.SetActive(!completed);
        mtInfo.SetStepInfo(steps[currentStep]);

        onInitialized?.Invoke();
    }

    bool CheckForCompletion()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            for (int j = 0; j < steps[i].needs.Length; j++)
            {
                SaveManager.SaveInt(uid + "step " + i + "need " + j, steps[i].needs[j].amount);
            }
        }
        for (int i = 0; i < GetTotalMaterialsNeeded(); i++)
        {
            if (steps[currentStep].needs[i].amount > 0) return false;
        }

        StepCompleted();
        return true;
    }

    void StepCompleted()
    {
        playerInside = false;
        SetReady(false, false);
        NeedsCompleted?.Invoke();
        mtInfo.gameObject.SetActive(false);
        if (currentStep == steps.Length - 1)
        { // ALL STEPS COMPLETED
            interactionArea.gameObject.SetActive(false);
            AllNeedsCompleted?.Invoke();
            completed = true;
            SaveManager.SaveBool(uid + "completed", completed);
        }
        else
        { // NEXT STEP
            currentStep++;
            SaveManager.SaveInt(uid + "step", currentStep);
            LeanTween.value(0, 1, 2f).setOnComplete(() =>
            {
                mtInfo.gameObject.SetActive(true);
                SetReady(true, false);
            });
        }
    }

    //----------------------------------------------------

    public void SetReady(bool _on, bool _immediate, bool _setInfo = false)
    {
        ready = _on;
        interactionArea.SetReady(_on, _immediate);
        if (_setInfo)
        {
            mtInfo.SetStepInfo(steps[currentStep]);
        }
    }

    public void ResetMTInfo()
    {
        mtInfo.SetStepInfo(steps[currentStep]);
    }

    //----------------------------------------------------

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (completed) return;
            playerInside = true;
            startTime = Time.time;

            delayBetweenTicks = 0.05f;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (completed) return;
            playerInside = false;

            delayBetweenTicks = 0.05f;
        }
    }

    //----------------------------------------------------

    public int GetTotalMaterialsNeeded() => steps[currentStep].needs.Length;
    public int GetCurrentAmountNeeded(int _index) => steps[currentStep].needs[_index].amount;
    public GameMaterial[] GetCurrentStepMaterials()
    {
        List<GameMaterial> aux = new List<GameMaterial>();
        for (int i = 0; i < steps[currentStep].needs.Length; i++)
        {
            aux.Add(steps[currentStep].needs[i].relatedMaterial);
        }
        return aux.ToArray();
    }
    public GameMaterial[] GetStepMaterials(int _step)
    {
        List<GameMaterial> aux = new List<GameMaterial>();
        for (int i = 0; i < steps[_step].needs.Length; i++)
        {
            aux.Add(steps[_step].needs[i].relatedMaterial);
        }
        return aux.ToArray();
    }
}

[System.Serializable]
public class Step
{
    public int levelNeeded;
    public InventoryItem[] needs;

}