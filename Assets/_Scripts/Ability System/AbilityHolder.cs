
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHolder : NetworkBehaviour
{
    public Ability ability;
    float cooldownTime;
    float activeTime;
    public Image UIFill;

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }
    AbilityState state = AbilityState.ready;

    public KeyCode key;
    bool Fail;
    float maxCooldownVal;
    private void Start()
    {

        maxCooldownVal = ability.coolDownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLocalPlayer)
        {
            return;
        }
        switch (state)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    ability.Activate(gameObject, out Fail);
                    if (Fail) return;
                    
                    state = AbilityState.active;


                    activeTime = ability.activeTime;
                    
                }
                break;
            case AbilityState.active:
                if(activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.cooldown;
                    UIFill.fillAmount = 
                    cooldownTime = ability.coolDownTime;
                }
            break;
            case AbilityState.cooldown:
                if(cooldownTime > 0) {  cooldownTime -= Time.deltaTime;
                    UIFill.fillAmount = 1-(cooldownTime / maxCooldownVal);
                }
                else { state = AbilityState.ready; }
            break;
        }


        
    }
}
