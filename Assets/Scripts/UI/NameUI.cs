using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NameUI : NetworkBehaviour
{

    [SerializeField] Text _nameText;
    NetworkVariable<FixedString32Bytes> _user = new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            var user = SavedClientInformationManager.GetUserData(OwnerClientId);
            _user.Value = user.userName;
            _nameText.text = _user.Value.ToString();
        }
    }

    public void Start()
    {
        if (!IsServer)
            _nameText.text = _user.Value.ToString();
    }

    public struct NetworkString : INetworkSerializable
    {
        private FixedString32Bytes name;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref name);
        }

        public override string ToString()
        {
            return name.ToString();
        }

        public static implicit operator string(NetworkString s) => s.ToString();
        public static implicit operator NetworkString(string s) => new NetworkString() { name = new FixedString32Bytes(s) };
    }
}
