using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> _currentHealth = new NetworkVariable<int>();
    [SerializeField] PlayerController _playerController;
    [SerializeField] FiringAction _firingController;
    [SerializeField] GameObject _deathScreen;
    private UnityAction _onDeathEvent;
    private UnityAction _respawnEvent;
    private bool _isDead = false;
    private NetworkVariable<int> _respawnTokens = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public override void OnNetworkSpawn()
    {
        _onDeathEvent += Death;

        if (!IsServer) return;
        _currentHealth.Value = 100;
    }

    void Start()
    {
        _currentHealth.OnValueChanged += CheckDeath;
    }

    private void CheckDeath(int previousValue, int newValue)
    {
        if (_currentHealth.Value <= 0)
            _onDeathEvent.Invoke();
    }

    private void Death()
    {
        if (IsOwner)
        {
            transform.position = new Vector3(100, 100, 0);
            _playerController.enabled = false;
        }

        if (IsServer)
        {
            _respawnTokens.Value--;

            if (_respawnTokens.Value > 0)
                StartCoroutine(RespawnEvent());
        }
    }

    public void TakeDamage(int damage)
    {
        damage = damage < 0 ? damage : -damage;
        _currentHealth.Value += damage;
    }

    IEnumerator RespawnEvent()
    {
        yield return new WaitForSeconds(2);

        int xPosition = Random.Range(-4, 4);
        int yPosition = Random.Range(-2, 2);
        transform.position = new Vector3(xPosition, yPosition, 0);

        _playerController.enabled = true;
        _deathScreen.SetActive(false);

        _firingController.AddAmmo(100);
        _currentHealth.Value = 100;

        yield break;
    }

    public void Heal(int healing)
    {
        if (!IsServer) return;
        _currentHealth.Value += healing;
    }

}
