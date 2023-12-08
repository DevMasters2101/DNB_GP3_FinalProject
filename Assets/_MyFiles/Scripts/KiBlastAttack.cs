using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KiBlastAttack : MonoBehaviour
{
    public GameObject kiBlastPreFab;
    public Transform kiBlastSpawn;
    public float kiForce = 20f;

    public static KiBlastAttack instance;
    NewPlayerInput playerInput;
    PlayerInput pi;
    Coroutine blastChargingCoroutine;
    Coroutine blastCoroutine;

    private Animator playerAnim;

    private void Awake()
    {
        instance = this;
        playerInput = new NewPlayerInput();
        pi = GetComponent<PlayerInput>();
        playerInput.Enable();
        playerInput.gameplay.KiAttack.performed += StartBlastCharging;
        playerInput.gameplay.KiAttack.canceled += StopBlastCharging;
        playerAnim = GetComponent<Animator>();
    }

    private void StopBlastCharging(InputAction.CallbackContext context)
    {
        if(blastChargingCoroutine != null)
        {
            StopCoroutine(blastChargingCoroutine);
            blastCoroutine = StartCoroutine(BlastHold());
            playerAnim.SetBool("LoopEndBlast", true);
            KiAttack();

            if (kiBlastPreFab != null)
            {
                pi.actions.FindAction("move").Disable();
            }
            else
            {
                StopCoroutine(blastCoroutine);
                playerAnim.SetBool("LoopEndBlast", false);
                pi.actions.FindAction("move").Enable();
            }

            blastChargingCoroutine = null;
        }
    }

    private void StartBlastCharging(InputAction.CallbackContext context)
    {
        if (KiCharge.instance.UseKi(KiCharge.instance.GetMaxKi()))
        {
            blastChargingCoroutine = StartCoroutine(BlastGather());
            pi.actions.FindAction("move").Disable();
            playerAnim.SetBool("ChargeBlast", true);
        }
    }

    private void KiAttack()
    {
        GameObject blastClone = Instantiate(kiBlastPreFab, kiBlastSpawn.transform.position, kiBlastSpawn.transform.rotation);
        Rigidbody rBody = blastClone.GetComponent<Rigidbody>();
        rBody.AddForce(-kiBlastSpawn.transform.forward * kiForce, ForceMode.Impulse);
    }

    private IEnumerator BlastGather()
    {
        playerAnim.SetBool("StartBlast", true);
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator BlastHold()
    {
        playerAnim.SetBool("EndBlast", true);
        yield return new WaitForEndOfFrame();
    }
}
