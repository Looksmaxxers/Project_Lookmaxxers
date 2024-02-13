using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            BallCollector bc = c.attachedRigidbody.gameObject.GetComponent<BallCollector>();
            if (bc != null)
            {
                bc.RecieveBall();
                EventManager.TriggerEvent<BombBounceEvent, Vector3>(c.transform.position);
                Destroy(gameObject);
            }
        }
    }
}
