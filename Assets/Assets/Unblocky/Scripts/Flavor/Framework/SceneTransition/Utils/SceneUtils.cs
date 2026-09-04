using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flavor
{
    public static class SceneUtils
    {
        #region 1. Load Đồng Bộ (Tức thì)

        /// <summary>
        /// Load Scene theo tên (Đồng bộ - đứng hình trong lúc load)
        /// </summary>
        public static void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(sceneName, mode);
        }

        /// <summary>
        /// Load Scene theo Index trong Build Settings
        /// </summary>
        public static void LoadScene(int sceneBuildIndex, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(sceneBuildIndex, mode);
        }

        /// <summary>
        /// Chơi lại màn hiện tại (Replay)
        /// </summary>
        public static void ReloadCurrentScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }

        #endregion

        #region 2. Load Bất Đồng Bộ (Async - Mượt mà, hỗ trợ Loading Bar)

        /// <summary>
        /// Trả về AsyncOperation để bạn tự theo dõi tiến trình (progress)
        /// </summary>
        public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(sceneName, mode);
        }

        /// <summary>
        /// Load Scene ngầm, theo dõi tiến độ %, và CHỜ ĐẾN KHI scene mới Initialize xong mới gọi onCompleted
        /// </summary>
        /// <param name="sceneName">Tên Scene cần nạp</param>
        /// <param name="onProgress">Bắn ra tiến độ % (0.0 -> 1.0)</param>
        /// <param name="onCompleted">Gọi khi TẤT CẢ đã sẵn sàng (để tắt UI Loading)</param>
        /// <param name="waitUntil">Điều kiện chờ (ví dụ: () => LevelManager.IsInitialized)</param>
        /// <param name="mode">Chế độ load</param>
        public static IEnumerator LoadSceneWithProgressRoutine(
         string sceneName,
         Action<float> onProgress = null,
         Action onCompleted = null,
         Func<bool> waitUntil = null,
         LoadSceneMode mode = LoadSceneMode.Single,
         float minLoadDuration = 1.0f) // Thời gian tối thiểu để slider chạy mượt (ví dụ 1 giây)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, mode);

            // 1. CHẶN Unity tự động kích hoạt scene khi nạp xong
            op.allowSceneActivation = false;

            float currentProgress = 0f;

            float speed = 1f / Mathf.Max(0.1f, minLoadDuration);
            // Giai đoạn 1: Chờ Unity nạp dữ liệu nền (0 -> 90%)
            while (op.progress < 0.9f || currentProgress < 0.9f)
            {
                currentProgress = Mathf.MoveTowards(currentProgress, 0.9f, Time.unscaledDeltaTime * speed);
                onProgress?.Invoke(currentProgress);
                yield return null;
            }

            // 5. CHO PHÉP Scene mới kích hoạt
            op.allowSceneActivation = true;

            // 3. Chờ thêm điều kiện khởi tạo dữ liệu riêng của bạn (nếu có)
            if (waitUntil != null)
            {
                yield return new WaitUntil(waitUntil);
            }

            while (currentProgress < 1.0f)
            {
                currentProgress = Mathf.MoveTowards(currentProgress, 1.0f, Time.unscaledDeltaTime * speed);
                onProgress?.Invoke(currentProgress);
                yield return null;
            }

            // 4. Chốt 100% cho Slider
            onProgress?.Invoke(1.0f);


            // Chờ Scene hoàn tất kích hoạt hoàn toàn
            while (!op.isDone)
            {
                yield return null;
            }

            // 6. Hoàn tất toàn bộ
            onCompleted?.Invoke();
        }


        #endregion

        #region 3. Unload Scene (Dùng cho Additive Scenes)

        /// <summary>
        /// Gỡ bỏ một Scene đang chạy ngầm
        /// </summary>
        public static AsyncOperation UnloadSceneAsync(string sceneName)
        {
            return SceneManager.UnloadSceneAsync(sceneName);
        }

        #endregion

        #region 4. Tiện ích kiểm tra thông tin Scene

        /// <summary>
        /// Lấy tên Scene đang hiển thị hiện tại
        /// </summary>
        public static string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        /// <summary>
        /// Lấy Index của Scene hiện tại
        /// </summary>
        public static int GetActiveSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        /// <summary>
        /// Kiểm tra xem một Scene đã được Load hay chưa
        /// </summary>
        public static bool IsSceneLoaded(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            return scene.isLoaded;
        }

        #endregion
    }
}
