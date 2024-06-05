using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

namespace Project
{
    public class Zipline : MonoBehaviour
    {
        public LineRenderer LR;
        public Transform PointA;
        public Transform PointB;
        public float LineSize;
        public float ZipSpeed;
        public float ColliderSize;
        public LayerMask canUseZipline;
        public Vector3 DirectionVector;

        BoxCollider collider;
        // Start is called before the first frame update
        void Start()
        {
            LR = GetComponent<LineRenderer>();
            LR.SetPosition(0, PointA.localPosition);
            LR.SetPosition(1, PointB.localPosition);
            DirectionVector = (PointB.position - PointA.position).normalized;


            GameObject ColliderObj = GenerateBoxCollider(PointA, PointB);
            ColliderObj.transform.parent = transform;



        }




        GameObject GenerateBoxCollider(Transform pointA, Transform pointB)
        {
            // Calculate the position and size of the box collider
            Vector3 positionA = pointA.position;
            Vector3 positionB = pointB.position;

            Vector3 center = (positionA + positionB) / 2f;
            float length = Vector3.Distance(positionA, positionB);

            // Create an empty GameObject to hold the BoxCollider
            GameObject boxObject = new GameObject("GeneratedBoxCollider");

            // Position the GameObject at the center
            boxObject.transform.position = center;

            // Orient the GameObject to align with the direction from A to B
            boxObject.transform.rotation = Quaternion.LookRotation(positionB - positionA);

            // Add a BoxCollider component
            BoxCollider boxCollider = boxObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            // Set the size of the BoxCollider
            boxCollider.size = new Vector3(ColliderSize, ColliderSize, length);

            //Add TriggerHandling
            ZiplineTriggerHandler ZTH = boxObject.AddComponent<ZiplineTriggerHandler>();
            ZTH.zipline = this;


            return boxObject;


        }
    }

    public class ZiplineTriggerHandler : MonoBehaviour
    {
        public Zipline zipline;
        bool entered = false;
        Rigidbody rb;
        Vector3 Velocity;

        private void OnTriggerStay(Collider other)
        {
            

            if (Input.GetKey(KeyCode.Space))
            {
                if (!entered)
                {
                    EnterZipline(other.gameObject);
                    if (Vector3.Dot(Camera.main.transform.forward, zipline.DirectionVector) > 0)
                    {
                        Velocity = zipline.ZipSpeed * zipline.DirectionVector;
                    }
                    else Velocity = -zipline.ZipSpeed * zipline.DirectionVector;
                }
                //Check Camera direction for direction of travel
                if (other.TryGetComponent<Rigidbody>(out rb))
                {
                    rb.useGravity = false;

                    rb.velocity = Velocity;

                    
                    
                }
            }

            else
            {
                ExitZipline();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            print("Exited Zipline");
            ExitZipline();
        }


        private void EnterZipline(GameObject GO)
        {
            entered = true;
            Vector3 projectedPoint = Vector3.Project(GO.transform.position - zipline.PointA.position, zipline.DirectionVector) + zipline.PointA.position;

            // Set the player's position to directly under the zipline
            GO.transform.position = new Vector3(projectedPoint.x, projectedPoint.y - 0.5f, projectedPoint.z);
        }

        private void ExitZipline()
        {
            if (rb == null) return;
            entered = false;
            rb.useGravity = true;
            rb = null;
        }

    }



}
