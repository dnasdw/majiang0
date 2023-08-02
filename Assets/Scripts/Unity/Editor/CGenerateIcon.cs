using System.IO;
using UnityEditor;
using UnityEngine;

using static sdw.cpp;
using static sdw.std;
using static sdw.zzz;

// alias
// typedef begin
using n8 = System.SByte;
using n16 = System.Int16;
using n32 = System.Int32;
using n64 = System.Int64;
using u8 = System.Byte;
using u16 = System.UInt16;
using u32 = System.UInt32;
using u64 = System.UInt64;

using f32 = System.Single;
using f64 = System.Double;

using static_cast_n8 = System.SByte;
using static_cast_n16 = System.Int16;
using static_cast_n32 = System.Int32;
using static_cast_n64 = System.Int64;
using static_cast_u8 = System.Byte;
using static_cast_u16 = System.UInt16;
using static_cast_u32 = System.UInt32;
using static_cast_u64 = System.UInt64;

using static_cast_f32 = System.Single;
using static_cast_f64 = System.Double;
// typedef end

namespace majiang0.Editor
{
    public class CGenerateIcon
    {
        public static string s_sIconName = "IconPanel";
        public static string s_sCameraName = "Main Camera";
        public static string s_sDirName = "Icons";
        public static string s_sIconFileName = "IconMajiang0.png";

        [MenuItem("SdwTools/Generate Icon")]
        public static void GenerateIcon()
        {
            GameObject iconObject = GameObject.Find(s_sIconName);
            if (iconObject == null)
            {
                Debug.Log("Icon not found");
                return;
            }
            GameObject cameraObject = GameObject.Find(s_sCameraName);
            if (cameraObject == null)
            {
                Debug.Log("Camera not found");
                return;
            }
            Camera camera = cameraObject.GetComponent<Camera>();
            if (camera == null)
            {
                Debug.Log("Camera not found");
                return;
            }
            RenderTexture renderTexture = new RenderTexture(1024, 1024, 32);
            camera.targetTexture = renderTexture;
            camera.Render();
            Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();
            byte[] pngBytes = texture2D.EncodeToPNG();
            Directory.CreateDirectory(Application.dataPath + "/" + s_sDirName);
            string filePath = Application.dataPath + "/" + s_sDirName + "/" + s_sIconFileName;
            Debug.Log(filePath);
            File.WriteAllBytes(filePath, pngBytes);
            RenderTexture.active = null;
            camera.targetTexture = null;
            Object.DestroyImmediate(renderTexture);
            Object.DestroyImmediate(texture2D);
        }
    }
}
