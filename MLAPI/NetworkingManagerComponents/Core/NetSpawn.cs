
using MLAPI.Configuration;
using MLAPI.Data;
using UnityEngine;

namespace MLAPI
{
    /// <summary>
    /// An easier method of instantiating 'NetworkedObjects'
    /// </summary>
    public static class NetSpawn
    {
        #region Private Properties

        static NetworkConfig NetworkConfiguration => NetworkingManager.singleton.NetworkConfig;

        #endregion

        #region Public Interface

        /// <summary>
        ///Instantiates a NetworkedPrefab as a NetworkedObject given it's name as a string
        /// </summary>
        /// <param name="netPrefabName"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <returns>The spawned NetworkedObject</returns>
        public static NetworkedObject Instantiate(string netPrefabName, Vector3? pos = null, Quaternion? rot = null)
        {
            if (!NetworkConfiguration.NetworkPrefabNames.ContainsValue(netPrefabName))
                return null;

            return Instantiate(
                NetworkConfiguration.NetworkedPrefabs[NetworkConfiguration.NetworkPrefabIds[netPrefabName]],
                pos,
                rot
                );
        }

        /// <summary>
        /// Instantiate's a NetworkedObject given it's NetworkedPrefab
        /// </summary>
        /// <param name="netPrefab"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <returns></returns>
        public static NetworkedObject Instantiate(NetworkedPrefab netPrefab, Vector3? pos = null, Quaternion? rot = null)
        {
            //Only do this check on the lowest level method overload
            if (!NetworkingManager.singleton.isServer)
                return null;


            GameObject obj = netPrefab.prefab;

            obj = GameObject.Instantiate(
                obj,
                pos ?? obj.transform.position,
                rot ?? obj.transform.rotation
                );
            NetworkedObject netObj = obj.GetComponent<NetworkedObject>();
            if (netObj == null) throw new System.Exception($"Network prefab does have a 'NetworkedObject' component on root");

            netObj.Spawn();
            return netObj;
        }

        #endregion
    }
}
