using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpriteHandler : NetworkBehaviour
{

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private NetworkVariable<float> _speed = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] Color _slow;
    [SerializeField] Color _fast;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsOwner)
        {
            _speed.Value = _rigidBody.velocity.magnitude;
        }

        _spriteRenderer.color = ChangeColor();
    }

    Color ChangeColor()
    {
        float invLerp = Mathf.InverseLerp(0, 5, _speed.Value);

        return Color.Lerp(_slow, _fast, invLerp);
    }

}
