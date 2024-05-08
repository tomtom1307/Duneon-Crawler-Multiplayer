using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Project
{
    public class DissolveController : MonoBehaviour
    {
        public List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
        public List<Material> materials;
        public VisualEffect effect;
        public float DissolveSpeed = 0.0125f;
        public float refreshRate = 0.025f;
        // Start is called before the first frame update

        void Start()
        {

            effect = GetComponentInChildren<VisualEffect>();
            effect.Stop();
            if (GetComponent<SkinnedMeshRenderer>()  != null)
            {
                skinnedMeshRenderers.Add(GetComponent<SkinnedMeshRenderer>());
            }

            if(GetComponentsInChildren<SkinnedMeshRenderer>() != null)
            {
                foreach (var item in GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    skinnedMeshRenderers.Add(item);
                }
            }

            foreach (var item in skinnedMeshRenderers)
            {
                foreach (var mat in item.materials)
                {
                    materials.Add(mat);
                }
            }

            
        }

        public void StartDissolve()
        {
            StartCoroutine(Dissolve());
        }

        public IEnumerator Dissolve()
        {
            if(effect != null)
            {
                effect.Play();
            }

            if(materials.Count > 0)
            {
                float counter = 0;
                while (materials[0].GetFloat("_DissolveAmount") < 1)
                {
                    counter+= DissolveSpeed;
                    foreach (var item in materials)
                    {

                        item.SetFloat("_DissolveAmount", counter);

                    }
                    yield return new WaitForSeconds(refreshRate);
                }
                
            }


        }
    }
}
