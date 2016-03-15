using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	CharacterController cc;
	Vector3 movement;
	float upwardVelocity = 0;
	public float movementSpeed = 10;
	float cameraRotation = 0;
	public float horizontalSensitivity = 1.5f;
	public float verticalSensitivity = 1.5f;
	public float jumpVel = 20;
	public float airMovementMult = .5f;
	public float useRange = 2;
	public bool holdToCrouch = true;
	bool crouching = false;
	float amountToLean = 10;
    public static bool cursorLocked = true;

	// Use this for initialization
	void Start () {
        ChoiceManager.instance.init();
		cc = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
		upwardVelocity += Physics.gravity.y*Time.deltaTime;
		if (cc.isGrounded && upwardVelocity < Physics.gravity.y) {
			upwardVelocity = Physics.gravity.y;
		}
		float mods = 0;
		if (cc.isGrounded) {
			mods = 1;
		} else {
			mods = airMovementMult;
		}

		movement.z += Input.GetAxis ("Vertical") * movementSpeed * Time.deltaTime*mods;
		movement.x += Input.GetAxis ("Horizontal") * movementSpeed * Time.deltaTime*mods;

		float rot = Input.GetAxis("Mouse X")+transform.rotation.eulerAngles.y*horizontalSensitivity;

		transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x, rot, transform.eulerAngles.z);
		Quaternion camRot = Camera.main.transform.rotation;
		cameraRotation = -Input.GetAxis("Mouse Y")+cameraRotation*verticalSensitivity;
		cameraRotation = Mathf.Clamp (cameraRotation, -60f, 60f);
		Camera.main.transform.rotation = Quaternion.Euler (cameraRotation, camRot.eulerAngles.y, camRot.eulerAngles.z);

		if (Input.GetButtonDown ("Jump") && cc.isGrounded) {
			upwardVelocity += jumpVel;
		}
		movement.y += upwardVelocity * Time.deltaTime;

		if (Input.GetButtonDown ("Use")) {
			RaycastHit info;
			if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out info, useRange)){
				Transform trans = info.collider.transform;
				while(trans != null){
					if(trans.GetComponent<Interactable>() != null){
						trans.GetComponent<Interactable>().use(gameObject);
						trans = null;
					}else{
						trans = trans.parent;
					}
				}
			}
		}
		if (holdToCrouch) {
			if (Input.GetButton ("Crouch")) {
				crouching = true;
				this.transform.localScale = Vector3.Lerp (transform.localScale, new Vector3 (transform.localScale.x, .5f, transform.localScale.z), .1f);
			} else {
				crouching = false;
				this.transform.localScale = Vector3.Lerp (transform.localScale, new Vector3 (transform.localScale.x, 1f, transform.localScale.z), .1f);
			}
		} else {
			if(Input.GetButtonDown("Crouch"))
				crouching = !crouching;
			if (crouching) {
				this.transform.localScale = Vector3.Lerp (transform.localScale, new Vector3 (transform.localScale.x, .5f, transform.localScale.z), .1f);
			} else {
				this.transform.localScale = Vector3.Lerp (transform.localScale, new Vector3 (transform.localScale.x, 1f, transform.localScale.z), .1f);
			}
		}
		if (!crouching) {
			float newLean = -Input.GetAxis ("lean") * amountToLean;
			Quaternion TargetRot = Quaternion.Euler (transform.FindChild ("CameraHinge").localRotation.x, transform.FindChild ("CameraHinge").localRotation.y, newLean);
			transform.FindChild ("CameraHinge").localRotation = Quaternion.Lerp (transform.FindChild ("CameraHinge").localRotation, TargetRot, .1f);
		}
	}

	void FixedUpdate(){
		cc.Move (transform.rotation*movement);
		movement = Vector3.zero;
	}
}
