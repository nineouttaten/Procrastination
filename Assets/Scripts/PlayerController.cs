using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] private float gravity = -13.0f;
    
    [SerializeField] bool lockCursor = true;

    [SerializeField] private float walkSpeed = 6.0f;

    public Camera camera;
    
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    
    CharacterController controller = null;
    
    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        
        if (!lockCursor) return;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
        
        if(Input.GetKeyDown("e"))
        {
            //Raycast
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
     
            if (Physics.Raycast(ray, out hit)) 
            {
                GameObject objectHit = hit.transform.gameObject;
                InteractionScript actionObj = objectHit.GetComponent<InteractionScript>();
                if(actionObj != null)
                {
                    actionObj.DoAction();
                }
            }
        }
    }

    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * (currentMouseDelta.x * mouseSensitivity));
    }

    private void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;

        var transformVelocity = transform;
        Vector3 velocity = (transformVelocity.forward * currentDir.y + transformVelocity.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }
}
