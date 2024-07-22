using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public event Action<int, int> EnergyChanged;
    [SerializeField] private int maxEnergy = 50;

    [SerializeField] private float maxTimeToRefill = 3f;
    [SerializeField] private int increaseEnergyAmount = 1;

    private Coroutine refillCoroutine;

    private int currentEnergy = 0;
    private void Start()
    {
        currentEnergy = maxEnergy;
        EnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }
    public bool UseEnergy(int amount)
    {
        if (currentEnergy == 0) return false;
        if (currentEnergy - amount < 0)
        {
            return false;
        }
        currentEnergy = Mathf.Max(currentEnergy - amount, 0);
        EnergyChanged?.Invoke(currentEnergy, maxEnergy);
        if (refillCoroutine != null)
        {
            StopCoroutine(refillCoroutine);
            refillCoroutine = null;
        }
        refillCoroutine = StartCoroutine(RefillEnergy());
        return true;
    }
    private IEnumerator RefillEnergy()
    {
        yield return new WaitForSeconds(maxTimeToRefill);
        while (currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Min(currentEnergy + increaseEnergyAmount, maxEnergy);
            EnergyChanged?.Invoke(currentEnergy, maxEnergy);
            yield return null;
        }

    }


}
