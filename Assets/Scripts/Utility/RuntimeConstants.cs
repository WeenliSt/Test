using UnityEngine;

namespace DefaultNamespace
{
    public static class RuntimeConstants
    {
        public static string APPLICATION_PATH = Application.persistentDataPath + "/";
        public const string BOOTSTRAP_SCENE_NAME = "Bootstrap";
        public const string CORE_GAMEPLAY_SCENE_NAME = "CoreGameplay";
    }

    public  enum PrefabName
    {
        Splash,
        HUDContainer,
        HUDBrushSize,
        HUDBrushColor,
        HUDSave,
        HUDLoad,
        HUDClear,
        PaintObject,
    }
}