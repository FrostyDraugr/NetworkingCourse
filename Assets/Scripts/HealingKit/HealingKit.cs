using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;

public class HealingKit : NetworkBehaviour
{
    [SerializeField] int _healAmount;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.GetComponent<Health>();
        if (!health) return;
        health.Heal(_healAmount);

        NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
        networkObject.Despawn();
    }
}
