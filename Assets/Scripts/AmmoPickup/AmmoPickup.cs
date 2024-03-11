using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AmmoPickup : NetworkBehaviour
{
    [SerializeField] int _ammoAmount;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsServer)
        {
            FiringAction firingAction = other.GetComponent<FiringAction>();
            if (!firingAction) return;
            firingAction.AddAmmo(_ammoAmount);

            int xPosition = Random.Range(-4, 4);
            int yPosition = Random.Range(-2, 2);

            gameObject.transform.position = new Vector3(xPosition, yPosition, 0);
        }
    }
}
