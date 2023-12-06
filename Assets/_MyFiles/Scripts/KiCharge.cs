using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class KiCharge : MonoBehaviour
{
    public Slider kiBar;


    private float maxKi = 10;
    private float currentKi;
    [SerializeField] private float chargeRate = 1;
    [SerializeField] private ParticleSystem chargeParticle;

    public static KiCharge instance;
    NewPlayerInput playerInput;
    PlayerInput pi;
    Coroutine chargingCoroutine;

    private Animator player_Animator;

    private void Awake()
    {
        instance = this;
        playerInput = new NewPlayerInput();
        pi = GetComponent<PlayerInput>();
        playerInput.Enable();
        playerInput.gameplay.recharge.performed += StartCharging;
        playerInput.gameplay.recharge.canceled += StopCharging;
    }

    private void StopCharging(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        chargeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        
        if (chargingCoroutine != null)
        {
            StopCoroutine(chargingCoroutine);
            pi.actions.FindAction("move").Enable();
            player_Animator.SetBool("ChargeUpLoop", false);
            player_Animator.SetBool("ChargeUp", false);
            chargingCoroutine = null;
        }
    }

    private void StartCharging(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        chargingCoroutine = StartCoroutine(Gather());
        pi.actions.FindAction("move").Disable();
        chargeParticle.Play();
        player_Animator.SetBool("ChargeUpLoop", true);

        if (currentKi >= maxKi)
        {
            player_Animator.SetBool("ChargeUpLoop", false);
            chargeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void Start()
    {
        currentKi = 0;
        kiBar.maxValue = maxKi;
        kiBar.value = currentKi;
        player_Animator = GetComponent<Animator>();
    }

    public float GetMaxKi()
    {
        return maxKi;
    }

    public bool UseKi(float amount)
    {
        Debug.Log($"current ki: {currentKi}, max Ki: {maxKi}");
        if (currentKi - amount >= 0)
        {
            currentKi -= amount;
            kiBar.value = currentKi;
            return true;
        }
        else
        {
            Debug.Log("Not enough ki");
            return false;
        }
    }

    private IEnumerator Gather()
    {
        while (currentKi < maxKi)
        {
            currentKi += Time.deltaTime * chargeRate;
            kiBar.value = currentKi;
            player_Animator.SetBool("ChargeUp", true);
            yield return new WaitForEndOfFrame();
        }
    }
}
