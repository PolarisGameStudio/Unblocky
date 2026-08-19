using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Flavor
{
    [CreateAssetMenu(fileName = "SystemSO", menuName = "Flavor/SystemSO", order = 1)]
    public class SystemConfigSO : ScriptableObject
    {
        public List<AssetReference> Systems;

#if UNITY_EDITOR
        [Button("Tự động Load Systems (Label)")]
        public void LoadSystems()
        {
            Systems = new List<AssetReference>();
            // Lấy thông tin cấu hình Addressables hiện tại
            var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("Chưa khởi tạo Addressables Settings!");
                return;
            }
            // Quét toàn bộ các asset đang có trong Addressables
            foreach (var group in settings.groups)
            {
                if (group == null) continue;
                foreach (var entry in group.entries)
                {
                    // Nếu Asset đó có dán nhãn "Systems"
                    if (entry.labels.Contains("Systems"))
                    {
                        Systems.Add(new AssetReference(entry.guid));
                    }
                }
            }
            // Đánh dấu ScriptableObject thay đổi để Unity lưu lại
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"Đã tự động điền {Systems.Count} Systems vào mảng!");
        }
#endif
    }
}

