using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    #region VAR's
    #region GeneralVars
    CharacterController controller;
    Rigidbody rb;
    CapsuleCollider playerCollider;
    [SerializeField] GameObject Player;
    #region Animation
    [SerializeField] Animator anim;
    #endregion
    #endregion
    #region Flags
   bool isGrounded;
   bool isSprinting;
   bool isWalking;
   bool isCrouching;
    #endregion
    #region Camera
    public float MouseSensitivity = 2.0f;
    public float lookXLimit = 45.0f;
    float rotationX = 0;
    public Camera playerCamera;

    [Range(60, 140)]
    public float Fov = 95;
    #endregion
    #region Walking
    public float jumpForce = 5f;
    public float walkSpeed = 400;
    public float sprintSpeed = 600;
    public float playerSpeed = 10;
    float speedf, speedb, speedl, speedr;
    #region AccelerationAndStopping
    public float accelerationSpeed = 1;
    public float stoppingSpeed = 1.5f;
    #endregion
    #endregion
    #endregion

    void Start()
    {
        #region Initialization
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        playerCollider = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        Player = gameObject;
        #endregion
    }
    private void FixedUpdate()
    {
        #region Movement 
        #region Walk
        if (Input.GetKey(KeyCode.W))
        {
            if (speedf < playerSpeed)
            {
                speedf += playerSpeed * accelerationSpeed * Time.deltaTime;
            }
            else if (speedf > playerSpeed)
            {
                speedf = playerSpeed;
            }


            rb.velocity = transform.forward * speedf * Time.deltaTime;
            isWalking = true;
           
        }
        else if (speedf > 0)
        {

            speedf -= playerSpeed * stoppingSpeed * Time.deltaTime;

            if (speedf < 0)
            {
                speedf = 0;
            }
            rb.velocity = transform.forward * speedf * Time.deltaTime;
            isWalking = false;
           
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (speedb < playerSpeed)
            {
                speedb += playerSpeed * accelerationSpeed * Time.deltaTime;
            }


            rb.velocity = -transform.forward * speedb * Time.deltaTime;
            isWalking = true;
          
        }
        else if (speedb > 0)
        {

            speedb -= playerSpeed * stoppingSpeed * Time.deltaTime;

            if (speedb < 0)
            {
                speedb = 0;
            }
            rb.velocity = -transform.forward * speedb * Time.deltaTime;
            isWalking = false;
           
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (speedl < playerSpeed)
            {
                speedl += playerSpeed * accelerationSpeed * Time.deltaTime;
            }
            rb.velocity = -transform.right * speedl * Time.deltaTime;
            isWalking = true;
        
        }
        else if (speedl > 0)
        {

            speedl -= playerSpeed * stoppingSpeed * Time.deltaTime;

            if (speedl < 0)
            {
                speedl = 0;
            }
            rb.velocity = -transform.right * speedl * Time.deltaTime;
            isWalking = false;
           
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (speedr < playerSpeed)
            {
                speedr += playerSpeed * accelerationSpeed * Time.deltaTime;
            }
            rb.velocity = transform.right * speedr * Time.deltaTime;
            isWalking = true;
    
        }
        else if (speedr > 0)
        {

            speedr -= playerSpeed * stoppingSpeed * Time.deltaTime;

            if (speedr < 0)
            {
                speedr = 0;
            }
            rb.velocity = transform.right * speedr * Time.deltaTime;
            isWalking = false;
          
        }
        #endregion
        #region Crouch 
        if (Input.GetKey(KeyCode.LeftControl))
        {
            playerCollider.height = 1;
            playerCamera.transform.localPosition = new Vector3(0, 0.2f, 0);
            isCrouching = true;
        }
        else
        {
            playerCollider.height = 2;
            playerCamera.transform.localPosition = new Vector3(0, 0.566f, 0);
            isCrouching = false;
        }
        #endregion
        #endregion
    }
    void Update()
    {
        #region Camera Look 
        rotationX += -Input.GetAxis("Mouse Y") * MouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * MouseSensitivity, 0);
        #endregion
        #region Camera Stuff
        playerCamera.fieldOfView = Fov;
        #endregion
        #region GroundCheck 
        //Check is Player is on the gorund
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, 1.5f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow);

            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        #endregion
        #region Jump
        if (isGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector3(0, jumpForce, 0);
            }
        }

        #endregion
        #region Sprint
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            playerSpeed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            playerSpeed = walkSpeed;
            isSprinting = false;
        }
        #endregion
        #region Animations
        if (isWalking &&isSprinting ==false)
        {
            anim.SetTrigger("walk");
        }
       else if (isSprinting && isWalking)
        {
            anim.SetTrigger("sprint");
        }
        if (isWalking==false && isSprinting == false)
        {
            anim.SetTrigger("idle");
        }
        #endregion
    }
}
#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(FPSController)), InitializeOnLoadAttribute]
public class FPSControllerEditor : Editor
{
    FPSController FPSController;
    SerializedObject SerFPC;

    private void OnEnable()
    {
        FPSController = (FPSController)target;
        SerFPC = new SerializedObject(FPSController);
    }

    public override void OnInspectorGUI()
    {
        SerFPC.Update();

        EditorGUILayout.Space();
        GUILayout.Label("FPSController ", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        GUILayout.Label("Author: Piotr Tomaszewski", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
        EditorGUILayout.Space();

        #region MovementSettings
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Movement Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 15 }, GUILayout.ExpandWidth(true));
       
        //jumping 
        EditorGUILayout.Space();
        GUILayout.Label("Jump Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        FPSController.jumpForce = EditorGUILayout.FloatField(new GUIContent("Jump Force", "Determines how much force is applied when player jump"), FPSController.jumpForce);

        //walking
        EditorGUILayout.Space();
        GUILayout.Label("Walking/Sprinting Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        FPSController.walkSpeed = EditorGUILayout.FloatField(new GUIContent("Walking Speed", "Determines how fast player can walk"), FPSController.walkSpeed);
        FPSController.sprintSpeed = EditorGUILayout.FloatField(new GUIContent("Sprint Speed", "Determines how fast player can sprint"), FPSController.sprintSpeed);
      
        //acceleration etc.
        EditorGUILayout.Space();
        GUILayout.Label("Acceleration/Stopping Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        FPSController.accelerationSpeed = EditorGUILayout.FloatField(new GUIContent("Acceleration Speed","Determines how fast player is accelerating"),FPSController.accelerationSpeed);
        FPSController.stoppingSpeed = EditorGUILayout.FloatField(new GUIContent("Stopping Speed", "Determines how fast player is going to stop"), FPSController.stoppingSpeed);

        //Camera Settings 
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Camera Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 15 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        FPSController.Fov = EditorGUILayout.Slider(new GUIContent("FOV", "Field of view."), FPSController.Fov, 80, 140);
        FPSController.lookXLimit = EditorGUILayout.Slider(new GUIContent("Camera X rotation limit", "X axis rotation limit."), FPSController.lookXLimit, 30, 90);
        FPSController.MouseSensitivity = EditorGUILayout.FloatField(new GUIContent("Mouse Sensitivity", "Mouse Sensitivity"), FPSController.MouseSensitivity);
        FPSController.playerCamera= (Camera)EditorGUILayout.ObjectField(new GUIContent("Camera Object", "Camera attached to the controller."), FPSController.playerCamera, typeof(Camera), true);
        #endregion

    }
}
#endif
#endregion
