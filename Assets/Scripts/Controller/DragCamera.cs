using UnityEngine;

public class DragCamera : MonoBehaviour
{
    public float speed;
    void Update()
    {
        if (Input.touchCount > 1)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            float currDist = Vector2.Distance(touch1.position, touch2.position);
            float lastDist = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);
            Debug.Log("Touch");
            if (currDist > lastDist && transform.position.y > 300)
            {
                GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y - 30, transform.position.z + 20);
                if (transform.position.y < 500)
                {
                    transform.Rotate(-1, 0, 0);
                }
            }
            else if (currDist < lastDist && transform.position.y < 1500)
            {
                GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + 30, transform.position.z - 20);
                if (transform.position.y < 500)
                {
                    transform.Rotate(1, 0, 0);
                }
            }
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float x = touch.deltaPosition.x * speed * Time.deltaTime * 0.05f;
            float z = touch.deltaPosition.y * speed * Time.deltaTime * 0.05f;
            Debug.Log("Touch");
            transform.position -= new Vector3(x, 0, z);

        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.y > 20)
        {
            //GetComponent<Camera>().fieldOfView--;
            GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y - 30, transform.position.z + 20);
            if (transform.position.y < 200)
            {
                transform.Rotate(-1, 0, 0);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y < 600)
        {
            //GetComponent<Camera>().fieldOfView++;
            GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + 30, transform.position.z - 20);
            if (transform.position.y < 200)
            {
                transform.Rotate(1, 0, 0);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            float x = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
            float z = Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
            //Debug.Log(Input.GetAxis("Mouse X") + " " + Input.GetAxis("Mouse Y"));
            //Debug.Log(transform.rotation.y);
            transform.position -= new Vector3(x, 0, z);
        }
        else if (Input.GetMouseButton(1))
        {
            Camera.main.transform.position = new Vector3(-100,500, -200);
            Camera.main.transform.rotation = Quaternion.Euler(60, 0, 0);
            /*
            Vector3 center = new Vector3(-200,0,-500);
            if (Input.GetAxis("Mouse X") > 0)
            {
                //Vector3 center = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, camera.nearClipPlane));
                //Debug.Log(center);
                transform.RotateAround(center, new Vector3(0, .5f, 0),3);
                //transform.eulerAngles = transform.eulerAngles - new Vector3(0, .7f, 0);
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                //Debug.Log(center);
                transform.RotateAround(center, new Vector3(0, -.5f, 0), 3);
                //transform.eulerAngles = transform.eulerAngles - new Vector3(0, -.7f, 0);
            }
        */
        }
    }
    

}