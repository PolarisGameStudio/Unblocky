using System;

namespace Flavor
{
    public interface ISaveable
    {
        public string SaveID { get; }
        Type SaveDataType { get; }
        public object CaptureSaving();
        public void RestoreSaving(object data);

    }
}