using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveSpeed = 200;

    //引用
    public GameObject center;
    void Start()
    {
        transform.position = (new Vector3(300, -250, 0));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 centerPosition = center.GetComponent<Transform>().position;
        transform.position = Vector3.MoveTowards(transform.position, centerPosition, Time.deltaTime * moveSpeed);
    }
}
