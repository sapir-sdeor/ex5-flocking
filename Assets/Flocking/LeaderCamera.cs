using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderCamera : MonoBehaviour
{

    [SerializeField] private Transform leaderTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(leaderTransform.position.x - 16f, transform.position.y, transform.position.z);
    }
}
