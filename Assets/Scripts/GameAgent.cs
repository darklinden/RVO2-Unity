using UnityEngine;
using Random = System.Random;

public class GameAgent : MonoBehaviour
{
    public int sid = -1;

    public float normalSpeed = 5.0f;

    public MeshRenderer meshRenderer;

    private Random m_random = new Random();

    private float m_timeflash = 0.0f;
    internal void Flash()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponentInChildren<MeshRenderer>();

        meshRenderer.material.color = Color.red;
        m_timeflash = 0.2f;
    }

    private void Update()
    {
        if (m_timeflash > 0.0f)
        {
            m_timeflash -= Time.deltaTime;
            if (m_timeflash <= 0.0f)
            {
                meshRenderer.material.color = Color.white;
            }
        }
    }
}