using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    public Transform target;
    public Transform farBackground, middleBackground, nearBackground, cloudBackground, grassBackground, miniGrassBackground; // Add a "miniGrassBackground"

    public float minHeight, maxHeight;
    private Vector2 lastPos;

    public bool stopFollow;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopFollow)
        {
            transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);

            Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

            // Move far background
            farBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f);

            // Move middle background
            middleBackground.position += new Vector3(amountToMove.x*0.1f, amountToMove.y, 0f) ;

            // Move near background
            nearBackground.position += new Vector3(amountToMove.x*0.2f, amountToMove.y, 0f) ;

            // Move cloud background
            cloudBackground.position += new Vector3(amountToMove.x*0.1f, amountToMove.y, 0f) ;

            // Move grass background
            grassBackground.position += new Vector3(amountToMove.x*0.3f, amountToMove.y, 0f) ;

            // Move mini grass background (same as grass background)
            miniGrassBackground.position += new Vector3(amountToMove.x*0.3f, amountToMove.y, 0f) ;

            // Update last position
            lastPos = transform.position;
        }
    }
}
