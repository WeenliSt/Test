using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace DefaultNamespace
{
    public enum ServiceType
    {
        BrushService,
        PaintService,
        HUDService,
        ObjectLoaderService,
        SceneLoader,
        HUD,
        PaintObjectService
    }
    public interface IService  : IDisposable
    {
       void Initialize();
       void PostInitialize();
    }
    public interface IProjectService : IService
    {
    }

    public abstract class DataService<T> : IService where T :class,  new()
    {
        private bool _dataUpdated = false;
        private string _extension = ".json";
        protected T _data;
        
        protected abstract string FILE_NAME { get; }
        
        public void TrySaveData()
        {
            if(!_dataUpdated) return;
            SaveData(_data);
            _dataUpdated = false;
        }

        protected void DataUpdated()
        {
            _dataUpdated = true;
        }      
        private void SaveData(T data)
        {
            var json = SerializeData();
            
            if (string.IsNullOrEmpty(json))
                return;
            
            var path = RuntimeConstants.APPLICATION_PATH + FILE_NAME + _extension;
            var fileDirectory = Path.GetDirectoryName(path);
            
            if (!Directory.Exists(fileDirectory)) Directory.CreateDirectory(fileDirectory);
            
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                fileStream.SetLength(0);
                byte[] info = Encoding.UTF8.GetBytes(json);
                fileStream.Write(info, 0, info.Length);
            }
        }

        private string SerializeData()
        {
            return JsonConvert.SerializeObject(_data, Formatting.None);
        }   
        private  T Deserialize(string data)
        {
            T getObject = JsonConvert.DeserializeObject<T>(data);
            return getObject;
        }
        private T LoadData()
        {
            var path = RuntimeConstants.APPLICATION_PATH + FILE_NAME + _extension;
            var data = string.Empty;
            if (File.Exists(path))
                data = File.ReadAllText(path);

            try
            {
                return Deserialize(data);
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public void Initialize()
        {
            try
            {
                _data = LoadData() ?? new T();
            }
            catch (Exception e)
            {
                _data = new T();
            }

            OnDataInitialized();
        }

        protected virtual void OnDataInitialized() { }

        public void PostInitialize()
        {
            OnPostInitialize();
        }

        protected virtual void OnPostInitialize() { }

        public void Dispose()
        {
            OnDispose();
        }        
        protected virtual void OnDispose() { }

    }
}