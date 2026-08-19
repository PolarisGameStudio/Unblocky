using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flavor
{
    public static class ServiceLocator
    {
        // Lưu trữ các Service theo cặp [Type, Object Instance]
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        /// <summary>
        /// Đăng ký một Service vào hệ thống.
        /// Khuyên dùng: Đăng ký theo Interface (vd: Register<ITicker>(tickService))
        /// </summary>
        public static void Register(Type type, object service)
        {
            if (service == null) return;
            if (_services.ContainsKey(type))
                _services[type] = service;
            else
                _services.Add(type, service);
        }

        /// <summary>
        /// Gỡ bỏ một Service khỏi hệ thống.
        /// </summary>
        public static void Unregister<T>() where T : class
        {
            Type serviceType = typeof(T);

            if (_services.ContainsKey(serviceType))
            {
                _services.Remove(serviceType);
            }
            else
            {
                Debug.LogWarning($"[ServiceLocator] Không tìm thấy Service kiểu {serviceType.Name} để gỡ bỏ!");
            }
        }

        /// <summary>
        /// Lấy Service ra để dùng. 
        /// Nếu không tìm thấy sẽ bắn LogError và trả về null.
        /// </summary>
        public static T Get<T>() where T : class
        {
            Type serviceType = typeof(T);

            if (_services.TryGetValue(serviceType, out object rawService))
            {
                return (T)rawService;
            }

            Debug.LogError($"[ServiceLocator] Không tìm thấy Service kiểu {serviceType.Name}! Hãy đảm bảo đã gọi Register<{serviceType.Name}>() trước đó.");
            return null;
        }

        /// <summary>
        /// Lấy Service một cách an toàn (dùng khi bạn không chắc chắn Service đó đã được đăng ký hay chưa).
        /// </summary>
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

        /// <summary>
        /// Kiểm tra xem một Service đã được đăng ký hay chưa.
        /// </summary>
        public static bool IsRegistered<T>() where T : class
        {
            return _services.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Xóa sạch toàn bộ các Service đã đăng ký (Dùng khi Reset toàn bộ Game hoặc đổi Scene).
        /// </summary>
        public static void ClearAll()
        {
            _services.Clear();
        }
    }
}
