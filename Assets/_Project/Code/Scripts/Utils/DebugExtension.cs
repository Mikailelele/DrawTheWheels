using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace DrawCar.Utils
{
    public static class DebugExtension
    {
        private const string UNITY_EDITOR = "UNITY_EDITOR";
        private const string DEV_BUILD = "DEVELOPMENT_BUILD";

        public static string Color(this string myStr, string color)
        {
            return $"<color={color}>{myStr}</color>";
        }

        #region MonoBehaviour

        private static void DoLog(Action<string, Object> LogFunction, string prefix, Object myObj, params object[] msg)
        {
#if UNITY_EDITOR
            var name = (myObj ? myObj.name : "NullObject").Color("lightblue");
            LogFunction($"{prefix}[{name}]: {String.Join("; ", msg)}\n ", myObj);
#endif
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void Log(this Object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "", myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogError(this Object myObj, params object[] msg)
        {
            DoLog(Debug.LogError, "<!>".Color("red"), myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogWarning(this Object myObj, params object[] msg)
        {
            DoLog(Debug.LogWarning, "⚠️".Color("yellow"), myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogSuccess(this Object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "☻".Color("green"), myObj, msg);
        }

        #endregion

        #region Native

        private static void DoLog(Action<string> LogFunction, string prefix, object myObj, params object[] msg)
        {
#if UNITY_EDITOR
            var name = (myObj != null ? myObj.GetType().ToString() : "NullObject").Color("lightblue");
            LogFunction($"{prefix}[{name}]: {String.Join("; ", msg)}\n ");
#endif
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void Log(this object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "", myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogError(this object myObj, params object[] msg)
        {
            DoLog(Debug.LogError, "<!>".Color("red"), myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogWarning(this object myObj, params object[] msg)
        {
            DoLog(Debug.LogWarning, "⚠️".Color("yellow"), myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogSuccess(this object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "☻".Color("green"), myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogRegisterSuccess(this object myObj)
        {
            DoLog(Debug.Log, "☐".Color("green"), myObj, "has been registered!");
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogInjectSuccess(this object myObj)
        {
            DoLog(Debug.Log, "┣▇▇▇═─".Color("green"), myObj, "has been injected!");
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogSubscribeSuccess(this object myObj)
        {
            DoLog(Debug.Log, "☊".Color("green"), myObj, "has been subscribed!");
        }

        #endregion
    }
}
