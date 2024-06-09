using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class MeshCombiner : MonoBehaviour
    {
        [SerializeField] private List<MeshFilter> sourceMeshFilters;
        [SerializeField] private MeshFilter targetMeshFilter;




        [ContextMenu("Combine Meshes")]
        private void CombineMeshes()
        {
            if (sourceMeshFilters == null || sourceMeshFilters.Count == 0)
            {
                Debug.LogWarning("No source meshes to combine.");
                return;
            }

            CombineInstance[] combine = new CombineInstance[sourceMeshFilters.Count];

            for (int i = 0; i < sourceMeshFilters.Count; i++)
            {
                combine[i].mesh = sourceMeshFilters[i].sharedMesh;
                // Adjust the transform to account for the parent object's transform
                combine[i].transform = transform.parent.worldToLocalMatrix * sourceMeshFilters[i].transform.localToWorldMatrix;
                //sourceMeshFilters[i].gameObject.SetActive(false); // Disable original mesh renderers
            }

            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combine, true, true);

            targetMeshFilter.mesh = combinedMesh;
            targetMeshFilter.gameObject.SetActive(true); // Ensure the target mesh renderer is active

            // Optional: Optimize the combined mesh
            combinedMesh.Optimize();
            combinedMesh.RecalculateNormals();
            combinedMesh.RecalculateBounds();
        }


        private void Start()
        {
            Invoke("CombineMeshes", 1.4f);
        }



    }
}
