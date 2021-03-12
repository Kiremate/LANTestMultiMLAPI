using UnityEngine;
using MLAPI;
using MLAPI.Serialization.Pooled;
using MLAPI.Messaging;
using System.IO;

public class MLAPIPerformanceTest : NetworkedBehaviour
{
    /// <summary>
    /// To use the performance mode, the RPC method require the following signature 
    /// void (ulong clientId, Stream readStream) 
    /// and the sender is required to use the non generic Stream overload.
    /// </summary>

    private void OnGUI()
    {
        if (GUILayout.Button("SendRandomInt"))
        {
            if (IsServer)
            {
                using (PooledBitStream stream = PooledBitStream.Get())
                {
                    using (PooledBitWriter writer = PooledBitWriter.Get(stream))
                    {
                        writer.WriteInt32Packed(Random.Range(-50, 50));

                        InvokeClientRpcOnEveryonePerformance(MyClientRPC, stream);
                    }
                }
            }
            else
            {
                using (PooledBitStream stream = PooledBitStream.Get())
                {
                    using (PooledBitWriter writer = PooledBitWriter.Get(stream))
                    {
                        writer.WriteInt32Packed(Random.Range(-50, 50));

                        InvokeServerRpcPerformance(MyServerRPC, stream);
                    }
                }
            }
        }
    }

    [ServerRPC]
    private void MyServerRPC(ulong clientId, Stream stream) //This signature is REQUIRED for the performance mode
    {
        using (PooledBitReader reader = PooledBitReader.Get(stream))
        {
            int number = reader.ReadInt32Packed();
            Debug.Log("The number received was: " + number);
            Debug.Log("This method ran on the server upon the request of a client");
        }
    }

    [ClientRPC]
    private void MyClientRPC(ulong clientId, Stream stream) //This signature is REQUIRED for the performance mode
    {
        using (PooledBitReader reader = PooledBitReader.Get(stream))
        {
            int number = reader.ReadInt32Packed();
            Debug.Log("The number received was: " + number);
            Debug.Log("This method ran on the client upon the request of the server");
        }
    }

}
