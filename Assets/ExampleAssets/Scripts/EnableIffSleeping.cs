using UnityEngine;

/// <summary>
/// 当刚体固定移动时启用一个行为
/// 否则禁用该行为
/// </summary>
// Enables a behaviour when a rigidbody settles movement
// otherwise disables the behaviour
public class EnableIffSleeping : MonoBehaviour
{
    public Behaviour m_Behaviour;
    Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (m_Rigidbody == null || m_Behaviour == null)
            return;

        if (m_Rigidbody.IsSleeping() && !m_Behaviour.enabled)
            m_Behaviour.enabled = true;

        if (!m_Rigidbody.IsSleeping() && m_Behaviour.enabled)
            m_Behaviour.enabled = false;
    }
}
