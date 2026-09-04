using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Flavor
{
    public static class AddressablesExtensions
    {
        public static async UniTask<GameObject> InstantiateAsync(string addressableName)
        {
            var handle = Addressables.InstantiateAsync(addressableName);

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Lỗi: Không tìm thấy {addressableName}");
                return null;
            }

        }
    } 
}