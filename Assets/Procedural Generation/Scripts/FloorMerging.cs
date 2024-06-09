using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project
{
    public class FloorMerging : MonoBehaviour
    {

        void Start()
        {
            
        }

        public void CombineFloorColliders(List<GameObject> corridorObjects)
        {


            List<MeshFilter> floorMeshFilters = new List<MeshFilter>();

            // Gather all floor mesh filters
            foreach (var corridor in corridorObjects)
            {
                MeshFilter[] meshFilters = corridor.GetComponentsInChildren<MeshFilter>();
                foreach (var Filter in meshFilters)
                {
                    if (Filter.gameObject.name.Contains("Floor")) // Assuming floor objects have "Floor" in their name
                    {
                        floorMeshFilters.Add(Filter);
                    }
                }
            }

            // Combine the meshes
            CombineInstance[] combine = new CombineInstance[floorMeshFilters.Count];
            for (int i = 0; i < floorMeshFilters.Count; i++)
            {
                combine[i].mesh = floorMeshFilters[i].sharedMesh;
                combine[i].transform = floorMeshFilters[i].transform.localToWorldMatrix;
                floorMeshFilters[i].gameObject.SetActive(false); // Optionally disable original mesh renderers
            }

            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combine);

            // Create a new GameObject for the combined floor collider
            GameObject combinedFloor = new GameObject("CombinedFloorCollider");
            combinedFloor.transform.SetParent(transform);
            combinedFloor.layer = gameObject.layer;

            MeshFilter meshFilter = combinedFloor.AddComponent<MeshFilter>();
            meshFilter.mesh = combinedMesh;

            MeshCollider meshCollider = combinedFloor.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = combinedMesh;

            // Optional: Optimize the combined mesh
            combinedMesh.Optimize();
            combinedMesh.RecalculateNormals();
            combinedMesh.RecalculateBounds();
        }
    }
}
