# 📚 TÀI LIỆU KIẾN TRÚC & TỔNG HỢP CUỘC TRÒ CHUYỆN (FLAVOR FRAMEWORK)
*Thời gian sao lưu: 03/08/2026*

---

## 📌 MỤC LỤC
1. [Hệ Thống Vòng Lời & Kiến Trúc Cốt Lõi (Flavor Architecture)](#1-hệ-thống-vòng-đời--kiến-trúc-cốt-lõi)
2. [Cơ Chế ServiceLocator & Static Memory Cleanup](#2-cơ-chế-servicelocator--static-memory-cleanup)
3. [Tư Duy Hệ Thống: BaseSystem & Event-Driven vs Update Loop](#3-tư-duy-hệ-thống-basesystem)
4. [Quy Tắc Phân Biệt: Utils vs Extension vs Helper](#4-quy-tắc-phân-biệt-utils-vs-extension-vs-helper)
5. [Hệ Thống Chuyển Cảnh (Scene Transition Pipeline & Lego Pattern)](#5-hệ-thống-chuyển-cảnh-scene-transition)
6. [Kỹ Thuật Odin Inspector: TableList vs Dictionary](#6-kỹ-thuật-odin-inspector)

---

## 1. HỆ THỐNG VÒNG ĐỜI & KIẾN TRÚC CỐT LÕI

### A. Triết lý phân tách:
- **`TickService` (Pure C#):** Quản lý các danh sách `List<ITickable>` theo từng `TickMode` (EarlyTick, Tick, FixedTick, LateTick). Tránh dùng `Action/Delegate` cho update liên tục để không tạo rác GC.
- **`MainApplication` (MonoBehaviour - Composition Root):** Đóng vai trò là cầu nối duy nhất nhận các hàm gốc của Unity (`Update`, `FixedUpdate`, `LateUpdate`) rồi chuyển tiếp vào `TickService`.
- **`ILifeCycle`:**
  - `IEnumerator Initialize()`: Cho phép khởi tạo bất đồng bộ tuần tự (load file save, tải asset).
  - `void Dispose()`: **Bắt buộc là `void` (đồng bộ)** để dọn dẹp tức thì trước khi GameObject bị hủy.

### B. Mã nguồn chuẩn `BaseMono.cs`:
```csharp
using UnityEngine;

namespace Flavor
{
    public class BaseMono : MonoBehaviour, ITickable
    {
        [SerializeField] private TickMode mode = TickMode.None;

        protected virtual void OnEnable()
        {
            ListeningEvents();
            if (mode != TickMode.None)
            {
                this.GetApplication()?.RegisterTickable(this, mode);
            }
            DoEnable();
        }

        protected virtual void OnDisable()
        {
            DoDisable();
            UnlisteningEvents();
            if (mode != TickMode.None)
            {
                this.GetApplication()?.UnregisterTickable(this);
            }
            Dispose();
        }

        public virtual void DoEnable() { }
        public virtual void DoDisable() { }
        public virtual void Dispose() { }
        public virtual void ListeningEvents() { }
        public virtual void UnlisteningEvents() { }
        
        public virtual void EarlyTick() { }
        public virtual void FixedTick() { }
        public virtual void LateTick() { }
        public virtual void Tick() { }
    }
}
```

---

## 2. CƠ CHẾ SERVICELOCATOR & STATIC MEMORY CLEANUP

### A. Vai trò của `ServiceLocator`:
- Là kho lưu trữ reference trung tâm (thay thế cho việc lạm dụng Singleton hoặc FindObjectOfType).
- Hỗ trợ cả Generic `Register<T>` lẫn Runtime Type `Register(Type, object)`.
- **Tự động dọn dẹp Static:** Tích hợp `[RuntimeInitializeOnLoadMethod]` để xóa sạch dữ liệu static khi bấm Play trong Unity Editor (hỗ trợ tắt Domain Reload).

### B. Mã nguồn `ServiceLocator.cs`:
```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service) where T : class
        {
            Type serviceType = typeof(T);
            if (service == null) return;
            _services[serviceType] = service;
        }

        public static void Register(Type type, object service)
        {
            if (service == null) return;
            _services[type] = service;
        }

        public static void Unregister<T>() where T : class
        {
            Type serviceType = typeof(T);
            _services.Remove(serviceType);
        }

        public static T Get<T>() where T : class
        {
            Type serviceType = typeof(T);
            if (_services.TryGetValue(serviceType, out object rawService))
                return (T)rawService;

            Debug.LogError($"[ServiceLocator] Service {serviceType.Name} chưa được đăng ký!");
            return null;
        }

        public static bool TryGet<T>(out T service) where T : class
        {
            Type serviceType = typeof(T);
            if (_services.TryGetValue(serviceType, out object rawService))
            {
                service = (T)rawService;
                return true;
            }
            service = null;
            return false;
        }

        public static void ClearAll() => _services.Clear();

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticData() => ClearAll();
#endif
    }
}
```

---

## 3. TƯ DUY HỆ THỐNG: BASESYSTEM

### A. Nhạc trưởng `MainApplication` làm chủ đăng ký:
- `BaseSystem` không tự gọi `ServiceLocator` (để tránh phân tán).
- `MainApplication` quản lý danh sách `_systems`, đăng ký từng System theo `Priority` và chạy tuần tự `yield return system.Initialize()`.

### B. 80-90% System là Event-Driven (Không cần Update):
- Đa phần các System (`AudioSystem`, `SaveSystem`, `ShopSystem`, `ItemSystem`, `TraitSystem`...) chỉ chạy khi có sự kiện gọi đến.
- Chỉ các System đặc thù (Input, Physics, Cooldown) mới cần bật `TickMode.Tick`.

---

## 4. QUY TẮC PHÂN BIỆT: UTILS VS EXTENSION VS HELPER

| Tiêu chí | Extension | Utils | Helper |
| :--- | :--- | :--- | :--- |
| **Bản chất** | Mở rộng method cho Type có sẵn (dùng `this`) | Hộp đồ nghề kỹ thuật độc lập (Pure Tech / Math / File) | Trợ lý hỗ trợ 1 tính năng / domain game cụ thể |
| **Phụ thuộc Game** | Không phụ thuộc | **Độc lập 100%** (Dự án nào cũng dùng được) | **Dính chặt** với class của Game (Player, Block...) |
| **Cách gọi** | `transform.SetPosX(5f)` | `FileUtils.WriteText(path, text)` | `LevelSpawnHelper.SpawnBlock(blockData)` |
| **Ví dụ** | `TransformExtensions`, `VectorExtensions` | `FileUtils`, `MathUtils`, `ColorUtils` | `LevelHelper`, `AudioHelper`, `UIHelper` |

---

## 5. HỆ THỐNG CHUYỂN CẢNH (SCENE TRANSITION)

### A. Triết lý thiết kế:
1. **Chuyển Scene là một State Machine:**
   `Idle -> Transition Out (Che màn hình) -> Loading Async -> Waiting (Chờ Scene mới Ready) -> Transition In (Mở màn hình) -> Completed`.
2. **Kế thừa tối đa 1 tầng:** `BaseTransitionEffect : MonoBehaviour, ITransitionEffect`.
3. **Mô hình Lego (Lifecycle Hooks / Observer Pattern):**
   Mọi tính năng phụ (Âm thanh, Mẹo chơi, Rung màn hình, Hiệu ứng particle) đều là các mảnh Lego kế thừa `ITransitionHook`.

### B. Khung sườn Interface `ITransitionHook.cs`:
```csharp
namespace Flavor
{
    public interface ITransitionHook
    {
        void OnTransitionStart(); // Gọi lúc bắt đầu che màn hình
        void OnSceneLoaded();     // Gọi lúc Scene mới nạp xong (màn hình đang che kín)
        void OnTransitionEnd();   // Gọi lúc màn hình đã mở ra hoàn toàn
    }
}
```

---

## 6. KỸ THUẬT ODIN INSPECTOR

- **`[TableList]`**: Chỉ dùng cho `List<Class>` hoặc mảng `T[]` đại diện cho các dòng / cột. Không gắn trực tiếp lên `Dictionary`.
- **Hiển thị Dictionary dạng Bảng:**
  - *Cách 1:* Bọc thành `List<GroupDataRow>` + `[TableList]`.
  - *Cách 2:* Kế thừa `SerializedMonoBehaviour` và dùng `[DictionaryDrawerSettings]`.
