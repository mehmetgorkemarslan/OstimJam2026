using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Dependencies")] [SerializeField]
    private GameObject playerObject;
    [SerializeField] private float horizontalMargin = 5f;
    
    private Transform playerTransform;
    
    private float bloodFlowSpeed = 2f;

    private Camera _cam;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        playerTransform = playerObject.transform;
        bloodFlowSpeed = playerObject.GetComponent<PlayerController>()._bloodflowSpeed;
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        float currentCamX = transform.position.x;
        float nextCamX = currentCamX + (bloodFlowSpeed * Time.deltaTime);


        float leftBoundary = _cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + horizontalMargin;
        float rightBoundary = _cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - horizontalMargin;

        float playerX = playerTransform.position.x;

        if (playerX > rightBoundary)
        {

            float diff = playerX - rightBoundary;
            nextCamX += diff;
        }

        else if (playerX < leftBoundary)
        {
            float diff = playerX - leftBoundary;
            nextCamX += diff;
        }

        transform.position = new Vector3(nextCamX, transform.position.y, transform.position.z);
    }
}