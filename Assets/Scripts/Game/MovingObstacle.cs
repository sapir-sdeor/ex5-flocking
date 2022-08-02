using System;
using System.Collections;
using System.Collections.Generic;
using Flocking;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private LayerMask leaderLayer;
    [SerializeField] private float coolDown;
    [SerializeField] private float speed;
    
    private int movingDirection = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var cur = transform.position;
        transform.position = new Vector3(cur.x, cur.y, cur.z + (movingDirection * Time.deltaTime * speed));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            movingDirection *= -1;
        }

        if (other.gameObject.CompareTag("Peep"))
        {
            if (leaderLayer.value / Mathf.Pow(2, other.gameObject.layer) != 1)
            {
                var otherController = other.gameObject.GetComponent<PeepController>();
                if (!otherController.canBeHarmed || otherController.Group == 2)
                {
                    return;
                }
                otherController.Group = 2;
                otherController.MaxSpeed = 0;
                var peepRenderer = other.gameObject.GetComponentInChildren<MeshRenderer>();
                peepRenderer.material = GameManager._shared.grayMaterial;
                StartCoroutine(otherController.CantJoin(coolDown));
            }
        }
    }
}
