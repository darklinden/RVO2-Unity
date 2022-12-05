using System.Collections.Generic;
using RVO;
using UnityEngine;
using UnityEngine.Assertions;

public class GameMainManager : SingletonBehaviour<GameMainManager>
{
    public GameObject agentPrefab;
    public Vec2 mousePosition;
    private Plane m_hPlane = new Plane(Vector3.up, Vector3.zero);
    private Dictionary<int, GameAgent> m_agentMap = new Dictionary<int, GameAgent>();

    private RVOSimulator m_simulator = null;
    public RVOSimulator GetSimulator()
    {
        if (m_simulator == null)
        {
            m_simulator = new RVOSimulator();
        }
        return m_simulator;
    }

    // Use this for initialization
    void Start()
    {
    }

    private void UpdateMousePosition()
    {
        Vector3 position = Vector3.zero;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (m_hPlane.Raycast(mouseRay, out rayDistance))
            position = mouseRay.GetPoint(rayDistance);

        mousePosition.x_ = position.x;
        mousePosition.y_ = position.z;
    }

    void DeleteAgent()
    {
        int agentNo = GetSimulator().queryNearAgent(mousePosition, 1.5f);
        if (agentNo == -1 || !m_agentMap.ContainsKey(agentNo))
            return;

        GetSimulator().delAgent(agentNo);
        Destroy(m_agentMap[agentNo].gameObject);
        m_agentMap.Remove(agentNo);
    }

    void CreatAgent()
    {
        int sid = GetSimulator().addAgent(
            mousePosition,
            2f,
            10,
            1f,
            1f,
            0.6f,
            10,
            new Vec2(0.0f, 0.0f));
        if (sid >= 0)
        {
            GameObject go = Instantiate(agentPrefab, new Vector3(mousePosition.x, 0, mousePosition.y), Quaternion.identity);
            GameAgent ga = go.GetComponent<GameAgent>();
            Assert.IsNotNull(ga);
            ga.sid = sid;
            m_agentMap.Add(sid, ga);
        }
    }

    // void foo()
    // {

    //     var agent = GetSimulator().getAgent(sid);
    //     Vec2 pos = agent.position;
    //     Vec2 vel = agent.prefVelocity;
    //     transform.position = new Vector3(pos.x, transform.position.y, pos.y);
    //     if (Math.Abs(vel.x) > 0.01f && Math.Abs(vel.y) > 0.01f)
    //         transform.forward = new Vector3(vel.x, 0, vel.y).normalized;

    //     if (Input.GetMouseButton(0))
    //     {
    //         agent.prefVelocity = Vec2.zero;
    //         return;
    //     }
    //     else if (Input.GetMouseButton(1))
    //     {
    //         Vec2 goalVector = GameMainManager.Instance.mousePosition - pos;
    //         goalVector = RVOMath.normalize(goalVector) * normalSpeed;

    //         /* Perturb a little to avoid deadlocks due to perfect symmetry. */
    //         float angle = (float)m_random.NextDouble() * 2.0f * (float)Math.PI;
    //         float dist = (float)m_random.NextDouble() * 0.0001f;

    //         agent.prefVelocity = goalVector + dist * new Vec2((float)Math.Cos(angle), (float)Math.Sin(angle));
    //     }
    // }

    // Update is called once per frame
    private void Update()
    {
        UpdateMousePosition();

        var gaRun = Input.GetMouseButton(1);

        if (Input.GetMouseButtonUp(0))
        {
            if (Input.GetKey(KeyCode.Delete))
            {
                DeleteAgent();
            }
            else
            {
                CreatAgent();
            }
        }
        else if (Input.GetMouseButton(2))
        {
            var agentNo = GetSimulator().queryNearAgent(mousePosition, 1.5f);
            if (agentNo != -1 && m_agentMap.TryGetValue(agentNo, out var agent))
            {
                agent.Flash();
            }
        }

        GetSimulator().StepUpdate(Time.deltaTime);

        foreach (var ga in m_agentMap.Values)
        {
            var agent = GetSimulator().getAgent(ga.sid);
            Vec2 pos = agent.position;
            Vec2 vel = agent.prefVelocity;
            ga.transform.position = new Vector3(pos.x, ga.transform.position.y, pos.y);
            if (Mathf.Abs(vel.x) > 0.01f && Mathf.Abs(vel.y) > 0.01f)
                ga.transform.forward = new Vector3(vel.x, 0, vel.y).normalized;

            if (gaRun)
            {
                var goalVector = RVOMath.normalize(mousePosition - pos) * ga.normalSpeed;
                agent.prefVelocity = goalVector;
            }
            else
            {
                agent.prefVelocity = Vec2.zero;
            }
        }
    }
}