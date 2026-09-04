using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Flavor
{
    public class UISystem : BaseSystem
    {
        [Header("UI System Settings")]
        [Tooltip("Nhãn Addressable của các màn hình UI")]
        [SerializeField] private string _uiLabel = "UI";

        [Tooltip("Canvas cha để nhét các UI đẻ ra vào (Tùy chọn)")]
        [SerializeField] private Transform _uiRootCanvas;
        // Tự động phân loại UI vào Từ Điển (Khóa là Type của class con)
        private Dictionary<Type, BaseUI> _uiDictionary = new Dictionary<Type, BaseUI>();
        public override async UniTask LoadDataAsync()
        {
            // Chờ thằng cha chạy xong nếu có
            await base.LoadDataAsync();

            // Chạy hàm vét UI theo Label
            await LoadAllUIByLabelAsync();

            IsInitialized = true;
        }
        private async UniTask LoadAllUIByLabelAsync()
        {
            // 1. Xin danh sách địa chỉ của tất cả Asset đang dán nhãn "UI"
            var handle = Addressables.LoadResourceLocationsAsync(_uiLabel);
            var locations = await handle.Task;
            if (locations.Count == 0)
            {
                Debug.LogWarning($"[UISystem] Không tìm thấy cái Asset nào có nhãn '{_uiLabel}'!");
                return;
            }
            // 2. Duyệt qua từng địa chỉ, Instantiate (Đẻ ra) và Nhét vào Dictionary
            foreach (var location in locations)
            {
                // Đẻ Prefab ra và làm con của _uiRootCanvas
                var go = await Addressables.InstantiateAsync(location, _uiRootCanvas).Task;
                // Cố gắng tìm component kế thừa từ BaseUI (vd: WinUI, LoseUI, SettingsUI...)
                if (go.TryGetComponent<BaseUI>(out var uiComponent))
                {
                    // Lấy chính xác cái Type "Con" của nó. Ví dụ typeof(WinUI)
                    Type uiType = uiComponent.GetType();
                    if (!_uiDictionary.ContainsKey(uiType))
                    {
                        // Nhét vào từ điển
                        _uiDictionary.Add(uiType, uiComponent);

                        // Khởi tạo trạng thái ban đầu
                        uiComponent.Initialize();
                    }
                    else
                    {
                        Debug.LogError($"[UISystem] Bị trùng lặp Type UI: {uiType.Name}. Kiểm tra lại Prefab!");
                    }
                }
                else
                {
                    Debug.LogWarning($"[UISystem] Prefab {go.name} dán nhãn UI nhưng không có script BaseUI!");
                }
            }
            Debug.Log($"[UISystem] Tải xong {_uiDictionary.Count} màn hình UI vào Bộ nhớ!");
        }
        // ========================================================
        // HÀM TIỆN ÍCH CHO CÁC CLASS KHÁC GỌI LÊN LẤY UI
        // ========================================================

        public T GetUI<T>() where T : BaseUI
        {
            Type type = typeof(T);

            if (_uiDictionary.TryGetValue(type, out var ui))
            {
                // Ép kiểu ngược lại về T (vd: WinUI)
                return ui as T;
            }

            Debug.LogError($"[UISystem] Không tìm thấy UI nào thuộc loại {type.Name} đang chạy!");
            return null;
        }
    }
}
