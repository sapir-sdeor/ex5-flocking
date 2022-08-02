using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _shared;
    [SerializeField] public Material greenMaterial;
    [SerializeField] public Material grayMaterial;
    [SerializeField] public List<Material> regularMaterials;
    [SerializeField] public List<Material> ghostMaterials;
    
    // Start is called before the first frame update
    void Start()
    {
        _shared = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
