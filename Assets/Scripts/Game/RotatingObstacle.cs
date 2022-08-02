using System.Collections;
using System.Collections.Generic;
using Flocking;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject pivotObject;
    [SerializeField] private LayerMask leaderLayer;
    [SerializeField] private float coolDown;
    void Update()
    {
        transform.RotateAround(pivotObject.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
    
    private void OnCollisionEnter(Collision other)
    {
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
