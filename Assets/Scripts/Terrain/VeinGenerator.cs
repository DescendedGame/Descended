using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeinGenerator : MonoBehaviour
{
    public int m_vein_count = 100;
    public GameObject m_vein_prefab;

    private void Awake()
    {
        for (int i = 0; i < m_vein_count; i++)
        {
            Instantiate(m_vein_prefab);
        }
    }
}
