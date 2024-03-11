using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class FiringAction : NetworkBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject clientSingleBulletPrefab;
    [SerializeField] GameObject serverSingleBulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    NetworkVariable<int> _currentAmmo = new(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] Text _ammoText;

    void Start()
    {
        _currentAmmo.OnValueChanged += UpdateText;
    }

    private void UpdateText(int previousValue, int newValue)
    {
        _ammoText.text = _currentAmmo.Value.ToString() + " / 10";
    }

    public override void OnNetworkSpawn()
    {
        playerController.onFireEvent += Fire;
        _ammoText.text = _currentAmmo.Value.ToString() + " / 10";
    }

    private void Fire(bool isShooting)
    {

        if (isShooting && _currentAmmo.Value > 0)
        {
            ShootLocalBullet();
        }
    }

    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        if (IsServer)
        {
            _currentAmmo.Value--;
        }
        GameObject bullet = Instantiate(serverSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        ShootBulletClientRpc();
    }

    [ClientRpc]
    private void ShootBulletClientRpc()
    {
        if (IsOwner) return;
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

    }

    private void ShootLocalBullet()
    {
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

        ShootBulletServerRpc();
    }
}
