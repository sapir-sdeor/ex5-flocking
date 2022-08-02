using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiatePeep : MonoBehaviour
{

    [SerializeField] private float waitTime = 3f;
    [SerializeField] private int maxPeepNum = 10;
    [SerializeField] private int countPeep;
    [SerializeField] private GameObject peepPrefab;
    
    [SerializeField] private float stealWaitTime = 3f;
    [SerializeField] private int maxStealNum = 10;
    [SerializeField] private int countSteal;
    [SerializeField] private GameObject stealPrefab;
    
    [SerializeField] private float protectWaitTime = 3f;
    [SerializeField] private int maxProtectNum = 10;
    [SerializeField] private int countProtect;
    [SerializeField] private GameObject protectPrefab;


    const int HeightScreen = 130;
    const int MinZ = -14;
    const int MaxZ = 13;
    
  
    void Start()
    {
        StartCoroutine(NewPeep());
        StartCoroutine(NewProtection());
        StartCoroutine(NewSteal());
    }

    private IEnumerator NewPeep()
    {
        while (true)
        {
            if (countPeep <= maxPeepNum)
            {
                var posX = Random.Range(-HeightScreen, HeightScreen);
                var posZ = Random.Range(MinZ, MaxZ);
                var newPos = new Vector3(posX, 0, posZ);
                Instantiate(peepPrefab, newPos, Quaternion.identity);
                countPeep++;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
    
    private IEnumerator NewProtection()
    {
        while (true)
        {
            if (countProtect <= maxProtectNum)
            {
                var posX = Random.Range(-HeightScreen, HeightScreen);
                var posZ = Random.Range(MinZ, MaxZ);
                var newPos = new Vector3(posX, 1.2f, posZ);
                Instantiate(protectPrefab, newPos, Quaternion.identity);
                countProtect++;
            }
            yield return new WaitForSeconds(protectWaitTime);
        }
    }

    private IEnumerator NewSteal()
    {
        while (true)
        {
            if (countSteal <= maxStealNum)
            {
                var posX = Random.Range(-HeightScreen, HeightScreen);
                var posZ = Random.Range(MinZ, MaxZ);
                var newPos = new Vector3(posX, 1.2f, posZ);
                Instantiate(stealPrefab, newPos, Quaternion.identity);
                countSteal++;
            }

            yield return new WaitForSeconds(stealWaitTime);
        }
    }
}
