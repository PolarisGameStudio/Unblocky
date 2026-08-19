
namespace DVAH
{
    public static class CONSTANT
    {
#if UNITY_EDITOR
        public const string Prefix = "<color=cyan>[Huynn3rdLib]</color>";
#else
        public const string Prefix = "[Huynn3rdLib]";
#endif


        #region MiraiSDK key
        public const string SdkUrl = "https://github.com/nhathuy7996";
        public const string AdUnitClassName = "DVAH.AdUnitModule{0}_{1}";
        #endregion

        #region Custom
        public const string Check_rev = "CHECK_REV";
        #endregion
    }
}
