using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForChance : BehaviourTreeNode
{
    public float Timeout = 1.0f;
    public float SuccessThreshold = 1.0f;

    private float _timeout = 0; 
    
    #region BehaviourTreeNode
    public override void Initiate()
    {
        _timeout = Timeout;
        status = Status.Running;
    }

    public override void Run()
    {
        if (status == Status.Running)
        {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0)
            {
                float dice = Random.Range(0.0f, 1.0f);
                if (dice >= SuccessThreshold)
                {
                    Debug.Log("[WaitForChance] \"" + name + "\" succeeded with dice " + dice.ToString("F5") + " and threshold " + SuccessThreshold.ToString("F5"));
                    status = Status.Success;
                }
                else
                {
                    Debug.Log("[WaitForChance] \"" + name + "\" failed with dice " + dice.ToString("F5") + " and threshold " + SuccessThreshold.ToString("F5"));
                    status = Status.Failure;
                }
            }
        }
    }
    #endregion
}