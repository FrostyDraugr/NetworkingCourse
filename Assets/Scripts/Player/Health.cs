using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> _currentHealth = new NetworkVariable<int>();
    [SerializeField] PlayerController _playerController;
    [SerializeField] FiringAction _firingController;
    [SerializeField] GameObject _deathScreen;
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        _currentHealth.Value = 100;
    }

    void Start()
    {
        _currentHealth.OnValueChanged += CheckDeath;
    }

    private void CheckDeath(int previousValue, int newValue)
    {
        if (!IsOwner) return;
        if (_currentHealth.Value <= 0)
        {
            transform.position = new Vector3(100, 100, 0);
            _playerController.enabled = false;
            _deathScreen.SetActive(true);
        }
    }

    public void TakeDamage(int damage)
    {
        damage = damage < 0 ? damage : -damage;
        _currentHealth.Value += damage;
    }

    public void Respawn()
    {
        if (!IsOwner) return;
        int xPosition = Random.Range(-4, 4);
        int yPosition = Random.Range(-2, 2);
        transform.position = new Vector3(xPosition, yPosition, 0);

        _playerController.enabled = true;
        _deathScreen.SetActive(false);


        //NEEDS TO BE MOVED TO SERVER AUTHORITY
        _firingController.AddAmmo(100);
        _currentHealth.Value = 100;
    }

    public void Heal(int healing)
    {
        if (!IsServer) return;
        _currentHealth.Value += healing;
    }

}
