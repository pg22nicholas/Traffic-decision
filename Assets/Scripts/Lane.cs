using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{

    [SerializeField] private Car m_Car;
    [SerializeField] private Transform m_SpawnPoint;
    [SerializeField] public int m_ID;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCoroutine()
    {
        do
        {
            if (m_ID == 0)
            {
                Car car = Instantiate(m_Car, m_SpawnPoint.position, Quaternion.identity);
                car.m_LaneID = m_ID;
            }
            
            yield return new WaitForSeconds(5);
        } while (true);
    }
}
