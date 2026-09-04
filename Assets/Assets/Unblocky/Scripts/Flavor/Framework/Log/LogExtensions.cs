
using UnityEngine;

namespace Flavor
{
    public static class LogExtensions
    {
        public static void LogWarning(this object sender, string message)
        {
            // Thẻ #if này giúp Log chỉ chạy trên máy bạn. Khi xuất file game APK/EXE,
            // nó sẽ tự xóa lệnh này đi để game chạy mượt, không tốn CPU rác.
#if UNITY_EDITOR || DEVELOPMENT_BUILD

            // Tự động lấy tên Script và tô đậm nó lên
            string className = sender.GetType().Name;
            Debug.LogWarning($"<b><color=cyan>[{className}]</color></b> {message}");

#endif
        }

        public static void Log(this object sender, string message)
        {
            // Thẻ #if này giúp Log chỉ chạy trên máy bạn. Khi xuất file game APK/EXE,
            // nó sẽ tự xóa lệnh này đi để game chạy mượt, không tốn CPU rác.
#if UNITY_EDITOR || DEVELOPMENT_BUILD

            // Tự động lấy tên Script và tô đậm nó lên
            string className = sender.GetType().Name;
            Debug.Log($"<b><color=cyan>[{className}]</color></b> {message}");   

#endif
        }
    }
}