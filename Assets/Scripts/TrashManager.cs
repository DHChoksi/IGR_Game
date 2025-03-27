using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static Constants.Constants;

public class TrashManager : MonoBehaviour
{
    [SerializeField]
    private TrashData _TrashData;

    [SerializeField]
    private List<Trash> m_TrashList = new List<Trash>();

    [SerializeField]
    private List<GameObject> m_Platforms= new List<GameObject>();

    [SerializeField]
    private List<Vector3> m_SpawnCoordinates = new List<Vector3>();

    [SerializeField]
    private float m_SphereRadius = 120f;

    private int m_CoordinateIndex = 0;


    void Start()
    {
        GenerateCoordinates();
    }

    void GenerateCoordinates()
    {
        m_CoordinateIndex = 0;

        while (m_CoordinateIndex < m_TrashList.Count)       
        {
            float x_Coordinate = RandomChoice();
            float y_Coordinate = RandomChoice();
            float z_Coordinate = RandomChoice();
            float sphereMinRadius = 0.25f;
            float sphereMaxRadius = 1f;

            float distance = Mathf.Sqrt(Mathf.Pow(x_Coordinate, 2f) + Mathf.Pow(y_Coordinate, 2f) + Mathf.Pow(z_Coordinate, 2f));

            if (distance > sphereMaxRadius || distance < sphereMinRadius)
            {
                continue;
            }

            x_Coordinate *= m_SphereRadius;
            y_Coordinate *= m_SphereRadius;
            z_Coordinate *= m_SphereRadius;

            Vector3 newCoordinates = new Vector3(x_Coordinate, y_Coordinate, z_Coordinate);

            int flag = CheckPlatformDistance(newCoordinates);

            if (flag == 1)
            {
                continue;
            }
            
            float trashDistance = 5f;
            if (m_SpawnCoordinates.Count > 0)
            {
                for (int index = 0; index < m_SpawnCoordinates.Count; index++)
                {
                    if (Vector3.Distance(newCoordinates, m_SpawnCoordinates[index]) < trashDistance ||
                            m_SpawnCoordinates[index] == newCoordinates)
                    {
                        flag = 1;
                        break;
                    }

                }
            }
            
            if (flag == 1)
            {
                continue;
            }

            m_SpawnCoordinates.Add(newCoordinates);
            m_CoordinateIndex++;
        }

        SpawnTrash();
    }

    int CheckPlatformDistance(Vector3 trashPosition) 
    {
        for (int i = 0; i < m_Platforms.Count; i++)
        {
            Vector3 platformPosition = m_Platforms[i].transform.position;
            if (Vector3.Distance(platformPosition, trashPosition) < 1f)
            {
                return 1;
            }
        }
        return 0;
    }
        
    float  RandomChoice()
    {
        return Random.Range(-1f, 1f) * 2f - 1f;
    }

    void SpawnTrash()
    {
        for (int index = 0; index < m_TrashList.Count; index++)
        {
            m_TrashList[index].transform.position = m_SpawnCoordinates[index];
        }
    }

}
