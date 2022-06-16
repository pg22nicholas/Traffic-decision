using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public int m_LaneID;

    private Vector3 m_CurrSpeed;
    private Vector3 m_BaseSpeed;

    private bool m_TurnCooldown;

    // Start is called before the first frame update
    void Start()
    {
        m_BaseSpeed = Vector3.forward * Random.Range(1, 10);
        m_CurrSpeed = m_BaseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(m_CurrSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Lane lane = ChangeLaneRaycast(Vector3.right);
            if (lane != null) Turn(lane);
        } else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Lane lane = ChangeLaneRaycast(Vector3.left);
            if (lane != null) Turn(lane);
        }

        MakeDecision();
    }

    Lane ChangeLaneRaycast(Vector3 direction) 
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, 5f);
        foreach (RaycastHit hit in hits)
        {
            Lane lane = hit.collider.gameObject.GetComponent<Lane>();
            if (lane != null && lane.m_ID != m_LaneID)
            {
                return lane;
            }
        }
        return null;
    }

    IEnumerator TurnCooldown()
    {
        m_TurnCooldown = true;
        yield return new WaitForSeconds(4);
        m_TurnCooldown = false;
    }

    void Turn(Lane lane) 
    {
        transform.position = new Vector3(lane.transform.position.x, transform.position.y, transform.position.z);
        m_LaneID = lane.m_ID;
        m_CurrSpeed = m_BaseSpeed;
        StartCoroutine(TurnCooldown());
    }

    void MakeDecision()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.forward, 5f);
        Car car = null;
        foreach (RaycastHit hit in hits)
        {
            car = hit.collider.gameObject.GetComponent<Car>();
            if (car != null) break;
        }
        if (car != null)
        {
            m_CurrSpeed = car.CurrSpeed;

            if (m_TurnCooldown) return;

            Lane laneRight = ChangeLaneRaycast(Vector3.right);
            Lane laneLeft = ChangeLaneRaycast(Vector3.left);
            if (laneRight != null)
            {
                Turn(laneRight);
            } else if (laneLeft != null)
            {
                Turn(laneLeft);
            }
        }
    }

    bool CheckCarInDirection(Vector3 direction)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 5, direction, 5f);
        foreach (RaycastHit hit in hits)
        {
            Car car = hit.collider.gameObject.GetComponent<Car>();
            if (car != null) return true;
        }
        return false;
    }

    public Vector3 CurrSpeed { get { return m_CurrSpeed; } }
}


