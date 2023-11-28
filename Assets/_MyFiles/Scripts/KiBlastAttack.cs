using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiBlastAttack : MonoBehaviour
{
    public GameObject kiBlastPreFab;
    public Transform kiBlastSpawn;
    public float kiForce = 20f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (KiCharge.instance.UseKi(KiCharge.instance.GetMaxKi()))
            { 
                KiAttack();
            }
        }
    }

    private void KiAttack()
    {
        GameObject rocketClone = Instantiate(kiBlastPreFab, kiBlastSpawn.transform.position, kiBlastSpawn.transform.rotation);
        Rigidbody rBody = rocketClone.GetComponent<Rigidbody>();
        rBody.AddForce(-kiBlastSpawn.transform.forward * kiForce, ForceMode.Impulse);
    }
}
