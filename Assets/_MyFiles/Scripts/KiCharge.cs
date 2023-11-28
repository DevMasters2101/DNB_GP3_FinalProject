using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KiCharge : MonoBehaviour
{
    public Slider kiBar;

    private int maxKi = 100;
    private int currentKi;

    public static KiCharge instance;

    private void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        currentKi = 0;
        kiBar.maxValue = maxKi;
        kiBar.value = currentKi;
    }

    public void UseKi(int amount)
    {
         if (currentKi - amount >= maxKi)
        {
            currentKi -= amount;
            kiBar.value = currentKi;
        }
        else
        {
            Debug.Log("Not enough ki");
        }  
    }

    private void KiGather()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            while(currentKi  < maxKi)
            {
                currentKi += maxKi / 10;
                kiBar.value = currentKi;
            }
        }
    }
}
