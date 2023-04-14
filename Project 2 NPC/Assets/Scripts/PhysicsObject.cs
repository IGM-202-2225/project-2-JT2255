using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PhysicsObject : MonoBehaviour
{
    Vector3 position = Vector3.zero;
    Vector3 direction = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;


    public Vector3 Velocity => velocity;
    public Vector3 Direction => direction;
    public Vector3 Position => transform.position;


    [SerializeField]
    float mass = 1f;

    public bool useGravity, useFriction, useBounce;
    public float frictionCoeff;

    public Vector3 mousePos;
    public float radius = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        direction = Random.insideUnitCircle.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (useGravity)
        {
            ApplyGravity(Vector3.down);
        }

        if (useFriction)
        {
            ApplyFriction(frictionCoeff);
        }

        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;

        if (velocity.sqrMagnitude > Mathf.Epsilon)
        {
            direction = velocity.normalized;
        }

        //Do checks on new position here

        if (useBounce)
        {
            CheckForBounce();
        }
        
        transform.position = position;

        acceleration = Vector3.zero;

        transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;

        ApplyForce(friction);
    }

    void ApplyGravity(Vector3 force)
    {
        acceleration += force;
    }

    void CheckForBounce()
    {
          if (position.x > AgentManager.Instance.maxPosition.x)
          {
              velocity.x *= -1f;
          }
          else if (position.x < AgentManager.Instance.minPosition.x)
          {
              velocity.x *= -1f;
          }

          if (position.y > AgentManager.Instance.maxPosition.y)
          {
              velocity.y *= -1f;
          }
          else if (position.y < AgentManager.Instance.minPosition.y)
          {
              velocity.y *= -1f;
          }
    }
}
