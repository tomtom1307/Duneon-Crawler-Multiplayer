using Project;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class PlayerCam : MonoBehaviour
{

    public float sensitivity;
    public Transform orientation;
    CamAnimations camAnim;
    public Transform camHolder;

    float xRot;
    float yRot;

    // Start is called before the first frame update
    void Start()
    {
        camAnim = GetComponent<CamAnimations>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    // Update is called once per frame
   

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity;

        yRot += mouseX;
        xRot -= mouseY;

        xRot = Mathf.Clamp(xRot, -90f, 90f);

        camHolder.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
    }

    private void OnDisable()
    {
        camAnim.enabled = false;
    }

    private void OnEnable()
    {
        camAnim.enabled = true;
    }

}
