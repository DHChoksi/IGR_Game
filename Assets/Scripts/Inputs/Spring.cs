using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Spring     
{
    private float m_Strength;
    private float m_Damper;
    private float m_Target;
    private float m_Velocity;
    private float m_Value;
    public void Update(float deltaTime)
    { 
        var direction = m_Target - m_Value >= 0 ? 1f : -1f;  
        var force = Mathf.Abs(m_Target - m_Value) * m_Strength;
        m_Velocity += (force * direction - m_Velocity * m_Damper) * deltaTime;
        m_Value += m_Velocity * deltaTime;
    }
    public void Reset()  
    {
        m_Velocity = 0f; 
        m_Value = 0f; 
    } 
    public void SetValue(float value)
    {
        this.m_Value = value;
    }
    public void SetTarget(float target) 
    {
        this.m_Target = target;
    }
    public void SetDamper(float damper)
    {
        this.m_Damper = damper;
    }
    public void SetStrength(float strength)
    {
        this.m_Strength = strength;
    } 
    public void SetVelocity(float velocity)
    {
        this.m_Velocity = velocity;
    } 
    public float Value => m_Value; 
}
