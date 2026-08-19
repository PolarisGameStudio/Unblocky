using System;
using System.Collections.Generic;

namespace Flavor
{
    public static class EventDispatcher
    {
        private static readonly Dictionary<Type, Delegate> _subscribers = new Dictionary<Type, Delegate>();
        /// <summary>
        /// Đăng ký lắng nghe một sự kiện cụ thể.
        /// </summary>
        /// <param name=""></param>
        public static void Subscribe<T>(Action<T> callback) where T : struct
        {
            Type eventType = typeof(T);

            // nếu như chưa có kênh cho event này thì tạo mới
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = null;
            }
            _subscribers[eventType] = Delegate.Combine(_subscribers[eventType], callback);
        }

        /// <summary>
        /// Hủy đăng ký lắng nghe (Bắt buộc gọi ở Ondestroy hoặc OnDisable)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        public static void Unsubcribe<T>(Action<T> callback) where T : struct
        {
            Type eventType = typeof(T);

            // Tìm xem có kênh event này đang hoạt động không
            if (_subscribers.TryGetValue(eventType, out Delegate currentDelegate))
            {
                // Gỡ người nghe ra khỏi kênh
                Delegate newDelegate = Delegate.Remove(currentDelegate, callback);

                if (newDelegate == null)
                {
                    // Nếu kênh không còn ai nghe nữa thì xóa luôn khỏi Dictionary cho nhẹ RAM
                    _subscribers.Remove(eventType);
                }
                else
                {
                    // Cập nhật lại danh sách người nghe
                    _subscribers[eventType] = newDelegate;
                }
            }
        }

        /// <summary>
        /// Phát đi một event kèm theo Data. Tất cả những ai subscribe sẽ nhận được thông tin này.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventData"></param>
        public static void Publish<T>(T eventData) where T : struct
        {
            Type eventType = typeof(T);

            if(_subscribers.TryGetValue(eventType, out Delegate currentDelegate))
            {
                if (currentDelegate is Action<T> callback)
                {
                    callback.Invoke(eventData);
                }
            }
        }

        /// <summary>
        /// Dọn dẹp toàn bộ hệ thống (Thường dùng khi đổi Scene mới hoặc Reset toàn bộ Game).
        /// </summary>
        public static void ClearAllSubscribers()
        {
            _subscribers.Clear();
        }

    }
}