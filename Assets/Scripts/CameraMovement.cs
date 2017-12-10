using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float lookSpeedH = 2f;
    public float lookSpeedV = 2f;
    public float zoomSpeed = 2f;
    public float dragSpeed = 6f;
    public float orbitSpeed = 10.0f;
    private float yaw = 0f;
    private float pitch = 0f;
    public float minZoom = 10f;
    public float maxZoom = 300f;
    private float curZoom = 0f;
    public GameObject orbitAround;

    void Update()
    {
        //Look around with Right Mouse
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //yaw += lookSpeedH * Input.GetAxis("Mouse X");
            //pitch -= lookSpeedV * Input.GetAxis("Mouse Y");
            transform.RotateAround(orbitAround.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
            //transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //yaw += lookSpeedH * Input.GetAxis("Mouse X");
            //pitch -= lookSpeedV * Input.GetAxis("Mouse Y");
            transform.RotateAround(orbitAround.transform.position, Vector3.up, -orbitSpeed * Time.deltaTime);
            //transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }

        //drag camera around with right Mouse
        if (Input.GetMouseButton(1))
        {
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
        }

        //Zoom in and out with Mouse Wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;

        /*pos.y -= scroll * 1000 * zoomSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);
        pos.x -= scroll * 1000 *zoomSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, 636, 700); */
		pos.y -= scroll * 1000 * zoomSpeed * Time.deltaTime;
		pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);

        transform.position = pos;
    }
}
