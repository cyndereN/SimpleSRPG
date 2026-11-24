using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    private float moveHorizontal;
    private float moveVertical;
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer transposer;
    private float zoomAmount = 1f;
    private Vector3 followOffset;

    // Start is called before the first frame update
    void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        followOffset = transposer.m_FollowOffset;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotato();
        Zoom();

    }

    private void Zoom()
    {
        if (Input.mouseScrollDelta.y > 0) { followOffset.y -= zoomAmount; }
        else if (Input.mouseScrollDelta.y < 0) { followOffset.y += zoomAmount; }
        followOffset.y = Mathf.Clamp(followOffset.y, 1f, 10f);
        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, followOffset, 0.01f);
    }

    private void Move()
    {
        moveHorizontal = -Input.GetAxis("Horizontal");
        moveVertical = -Input.GetAxis("Vertical");
        Vector3 inputMoveDir = new Vector3(moveHorizontal, 0, moveVertical).normalized;
        Vector3 movevector3 = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += movevector3 * speed * Time.deltaTime;
    }
    private void Rotato()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q)) { rotationVector.y = 1f; }
        else if (Input.GetKey(KeyCode.E)) { rotationVector.y = -1f; }
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

}
