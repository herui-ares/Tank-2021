using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveSpeed = 200;
    void Start()
    {
        transform.position = (new Vector3(300, -250, 0));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(300, 150, 0), Time.deltaTime * moveSpeed);
    }
}
// tsst master branch