using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
namespace Flavor
{
    public class SaveSystem : BaseSystem, ISaver
    {
        [SerializeField] private List<ISaveable> _saves = new List<ISaveable>();

        public override void Initialize()
        {
            base.Initialize();
            Load();
        }

        public override void Dispose()
        {
            base.Dispose();
            Save();
        }

        public void RegisterSaveable(ISaveable saveable)
        {
            if (!_saves.Contains(saveable))
                _saves.Add(saveable);
        }
        // Hàm cho Lính hủy báo danh khi bị chết/xóa đi

        public void Save()
        {
            foreach (var saveable in _saves)
            {
                var saveID = saveable.SaveID;
                var captureObj = saveable.CaptureSaving();
                if (captureObj != null)
                {
                    WriteData(saveID, captureObj);
                }
            }
        }

        public void Load()
        {
            foreach (var saveable in _saves)
            {
                var saveID = saveable.SaveID;
                var typeToLoad = saveable.SaveDataType;

                var obj = ReadDataFromFile(saveID, typeToLoad);
                saveable.RestoreSaving(obj);

            }

        }



        public void WriteData<T>(string saveID, T dataToSave)
        {
            // Tự động gắn đuôi .json vào cái tên ID
            string fileName = saveID + ".json";
            string path = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                string jsonString = JsonConvert.SerializeObject(dataToSave, Formatting.Indented);

                File.WriteAllText(path, jsonString);
                Debug.Log($"[SaveSystem] Đã lưu {fileName} thành công tại:\n{path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Lỗi khi lưu {fileName}: {e.Message}");
            }
        }

        public object ReadDataFromFile(string saveID, Type typeToLoad)
        {
            if (typeof(ISaveData).IsAssignableFrom(typeToLoad) == false)
            {
                Debug.LogError($"[SaveSystem] Éc éc! Cái type {typeToLoad.Name} chưa được đóng mộc ISaveData!");
                return null; // Đuổi cổ ngay lập tức
            }

            string fileName = saveID + ".json";
            string path = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                if (File.Exists(path))
                {
                    string jsonString = File.ReadAllText(path);

                    object loadedData = JsonConvert.DeserializeObject(jsonString, typeToLoad);

                    Debug.Log($"[SaveSystem] Đã tải {fileName} thành công!");
                    return loadedData;
                }
                else
                {
                    Debug.LogWarning($"[SaveSystem] Không tìm thấy {fileName}. Tạo mới tự động.");
                    // Chữ new() ở trên kia là để cho phép hàm tự đẻ ra 1 Data trống nếu không thấy file
                    return Activator.CreateInstance(typeToLoad);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] File {fileName} bị hỏng: {e.Message}");
                return Activator.CreateInstance(typeToLoad);
            }
        }


        private void OnApplicationQuit()
        {
            Save(); // Lưu phát cuối cùng trước khi từ trần!
        }
        // Dành cho Mobile: Chạy khi người chơi ẩn app xuống Background nghe điện thoại
        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
            {
                Save();
            }
        }
    }
}

public interface ISaveData
{

}