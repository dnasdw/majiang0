using UnityEngine;
using UnityEngine.Rendering;

namespace sdw.unity
{
    // https://github.com/psygames/UnitySkipSplash
    public class CSkipSplash
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void BeforeSplashScreen()
        {
#if (UNITY_WEBGL || UNITY_EDITOR)
            Application.focusChanged += Application_focusChanged;
#endif
#if !UNITY_WEBGL
            System.Threading.Tasks.Task.Run(AsyncSkip);
#endif
        }

#if (UNITY_WEBGL || UNITY_EDITOR)
        private static void Application_focusChanged(bool hasFocus)
        {
            Application.focusChanged -= Application_focusChanged;
            AsyncSkip();
        }
#endif

        private static void AsyncSkip()
        {
            SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
        }
    }
}
