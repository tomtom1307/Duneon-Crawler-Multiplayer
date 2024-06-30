using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ChaosCore : MonoBehaviour
    {
        public float AttractionForce;
        PlayerStats PS;
        private void OnTriggerStay(Collider other)
        {
            
            if(other.gameObject.TryGetComponent<PlayerStats>(out PS))
            {
                transform.position = Vector3.Lerp(transform.position, PS.transform.position, AttractionForce);
                if(Vector3.Distance(transform.position, other.transform.position) < 0.6f)
                {
                    PS.ChaosCores += 1;
                    PS.ChaosCoreCountUI.text = $"{PS.ChaosCores}";
                    PlayerSoundSource.Instance.PlaySound(SourceSoundManager.SoundType.PickupSound, 0.7f);
                    Destroy(gameObject);
                }
            }
        }




    }
}
