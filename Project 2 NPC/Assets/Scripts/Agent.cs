using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PhysicsObject))]
public abstract class Agent : MonoBehaviour
{
    public PhysicsObject physicsObject;

    public float maxSpeed = 5f;
    public float maxForce = 5f;

    protected Vector3 totalForce = Vector3.zero;
    
    private float wanderAngle = 0f;
    public float maxWanderAngle = 45f;
    public float maxWanderChangePerSecond = 10f;

    public float personalSpace = 1f;
    
    private void Awake () 
    {
        if (physicsObject == null)
        {
            physicsObject = GetComponent<PhysicsObject>();
        }
    }

    protected virtual void Update()
    {
        CalculateSteeringForces();

        totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
        physicsObject.ApplyForce(totalForce);

        totalForce = Vector3.zero;
    }

    protected abstract void CalculateSteeringForces();

    protected void Seek(Vector3 targetPos, float weight = 1f)
    {
        // calculate desired velocity
        Vector3 desiredVelocity = targetPos - physicsObject.Position;

        // set desired velocity magnitude to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // calculate seek steering force
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        //return seekingForce * weight;
        // apply seek steering force
        totalForce += seekingForce * weight;
    }

    protected void Flee(Vector3 targetPos, float weight = 1f)
    {
        // calculate desired velocity
        Vector3 desiredVelocity = physicsObject.Position - targetPos;
    
        // set desired velocity magnitude to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // calculate the flee steering force
        Vector3 fleeingForce = desiredVelocity - physicsObject.Velocity;
        
        //return fleeingForce * weight;
        // apply the flee steering force
        totalForce += fleeingForce * weight;
    }

    protected void Pursue(Agent target)
    {
        Seek(target.CalculateFuturePosition(2f));
    }

    protected void Wander(float weight = 1f)
    {
        // Update the angle of our current wander
        float maxWanderChange = maxWanderChangePerSecond * Time.deltaTime;
        
        wanderAngle += Random.Range(-maxWanderChange, maxWanderChange);
        wanderAngle = Mathf.Clamp(wanderAngle, -maxWanderAngle, maxWanderAngle);

        // Get a position that is defined by that wander angle
        Vector3 wanderTarget = Quaternion.Euler(0, 0, wanderAngle) * physicsObject.Direction.normalized + 
                               physicsObject.Position;

        // Seek towards our wander position
        Seek(wanderTarget, weight);
    }

    protected void StayInBounds(float weight = 1f)
    {
        Vector3 futurePosition = CalculateFuturePosition();

        if (futurePosition.x > AgentManager.Instance.maxPosition.x || futurePosition.x < AgentManager.Instance.minPosition.x ||
            futurePosition.y > AgentManager.Instance.maxPosition.y || futurePosition.y < AgentManager.Instance.minPosition.y)
        {
            Seek(Vector3.zero, weight);
        } 
    }
    
    protected void Seperate<T>(List<T> agents) where T : Agent
    {
        float sqrPersonalSpace = Mathf.Pow(personalSpace, 2);
        
        // loop through agents
        foreach (T other in agents)
        {
            // find sqr distance between agents
            float sqrDist = Vector3.SqrMagnitude(other.physicsObject.Position - physicsObject.Position);

            if (sqrDist < float.Epsilon)
            {
                continue;
            }

            if (sqrDist < sqrPersonalSpace)
            {
                float weight = sqrPersonalSpace / (sqrDist + 0.1f);
                Flee(other.physicsObject.Position, weight);
            }
        }
    }

    public Vector3 CalculateFuturePosition(float time = 1f)
    {
        return physicsObject.Position + (physicsObject.Velocity * time);
    }
}
