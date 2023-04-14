using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettaFish : Agent
{
    public enum State
    {
        Healthy,
        Hungry
    }

    private State currentState = State.Healthy;
    public State CurrentState => currentState;
    public SpriteRenderer spriteRenderer;
    public Sprite healthySprite;
    public Sprite hungrySprite;
    
    protected override void CalculateSteeringForces()
    {
        Goldfish targetFish = AgentManager.Instance.GetClosestGoldfish(this);
        
        switch (currentState)
        {
            case State.Healthy:
            {
                float distToFish = Vector3.SqrMagnitude(physicsObject.Position - targetFish.physicsObject.Position);
                
                if (IsTouchingGoldfish(targetFish))
                {
                    StateTransition(State.Hungry);
                }
                else
                {
                    if (distToFish < Mathf.Pow(3f, 2))
                    {
                        Seek(targetFish.physicsObject.Position);
                    }
                    else
                    {
                        Wander();
                    }
                }
                
                Seperate(AgentManager.Instance.bettaFishes);
                break;
            }
            case State.Hungry:
            {
                Wander();
                Seperate(AgentManager.Instance.bettaFishes);
                //seek fish food here
                //switch to healthy after touching food
                break;
            }
        }
        
        StayInBounds(4);
    }

    private void StateTransition(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Healthy:
            {
                spriteRenderer.sprite = healthySprite;
                break;
            }
            case State.Hungry:
            {
                spriteRenderer.sprite = hungrySprite;
                break;
            }
        }
    }

    private bool IsTouchingFood()
    {
        //check for touching food here
        return true;
    }
    
    private bool IsTouchingGoldfish(Goldfish fish)
    {
        float sqrDistance = Vector3.SqrMagnitude(physicsObject.Position - fish.physicsObject.Position);
        float sqrRadii = Mathf.Pow(physicsObject.radius, 2) + Mathf.Pow(fish.physicsObject.radius, 2);

        return sqrDistance < sqrRadii;
    }
}
