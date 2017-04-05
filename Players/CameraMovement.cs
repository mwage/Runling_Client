using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float cameraSpeed;


	void Start ()
    {
        //set camera sensitivity
        cameraSpeed = 20f;
	}
	
    //move camera on WASD keypress/hold
	void LateUpdate ()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        float moveX = inputX * cameraSpeed * Time.deltaTime;
        float moveY = inputY * cameraSpeed * Time.deltaTime;

        transform.Translate(moveX, moveY, 0f);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -15, 15), Mathf.Clamp(transform.position.y, 0, 40), Mathf.Clamp(transform.position.z, -15, 15));
    }
}
