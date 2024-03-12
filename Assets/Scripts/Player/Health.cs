using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> _currentHealth = new NetworkVariable<int>();
    [SerializeField] PlayerController _playerController;
    [SerializeField] FiringAction _firingController;
    [SerializeField] GameObject _deathScreen;
    private NetworkVariable<bool> _dead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> _respawnTokens = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] Text _tokensLeftText;
    private NetworkVariable<bool> _respawnRequest = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        _dead.OnValueChanged += Death;

        if (IsOwner)
        {
            _respawnTokens.OnValueChanged += UpdateDeathText;
        }

        if (!IsServer) return;
        _currentHealth.Value = 100;
        _respawnRequest.OnValueChanged += RespawnHandler;
    }

    private void UpdateDeathText(int previousValue, int newValue)
    {
        if (!IsOwner) return;

        _tokensLeftText.text = "Tokens Left: " + _respawnTokens.Value;
    }

    private void Death(bool previousValue, bool newValue)
    {
        if (newValue == true)
        {
            if (IsOwner)
            {
                transform.position = new Vector3(1000, 1000, 0);

                _playerController.enabled = false;
                _deathScreen.SetActive(true);
            }
        }
        else
        {
            if (IsOwner)
            {
                int xPosition = Random.Range(-4, 4);
                int yPosition = Random.Range(-2, 2);
                transform.position = new Vector3(xPosition, yPosition, 0);

                _playerController.enabled = true;
                _deathScreen.SetActive(false);
                _respawnRequest.Value = false;
            }
        }


    }

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        damage = damage < 0 ? damage : -damage;
        _currentHealth.Value += damage;

        if (_currentHealth.Value <= 0)
        {
            _dead.Value = true;
        }
    }

    public void RespawnRequest()
    {
        if (!IsOwner) return;

        _respawnRequest.Value = true;
    }

    private void RespawnHandler(bool previousValue, bool newValue)
    {
        if (!IsServer || newValue == false) return;

        if (_respawnTokens.Value > 0)
        {
            _respawnTokens.Value--;
            Debug.Log(_respawnTokens.Value);
            _firingController.AddAmmo(100);
            _currentHealth.Value = 100;
            _dead.Value = false;
        }

    }

    public void Heal(int healing)
    {
        if (!IsServer) return;
        _currentHealth.Value += healing;
    }

}
