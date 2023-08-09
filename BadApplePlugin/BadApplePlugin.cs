using System.Collections;
using BadAppleSkulls.Commands;
using BepInEx;
using GameConsole;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BadAppleSkulls

{
    [BepInProcess("ULTRAKILL.exe")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class BadApplePlugin : BaseUnityPlugin
    {

        private void Awake()
        {
            StartCoroutine(Startup());
        }

        private IEnumerator Startup()
        {
            SceneManager.sceneLoaded += (scene, mode) => StartCoroutine(OnSceneLoaded(scene, mode));
            yield return null;
        }

        private IEnumerator OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (SceneHelper.CurrentScene != "uk_construct")
                yield break;

            GameObject.Find("Level Info").AddComponent<BadAppleScreenController>();
            
            MonoSingleton<Console>.Instance.RegisterCommand(new BadAppleScreenCommand());
            MonoSingleton<Console>.Instance.RegisterCommand(new BadApplePlayCommand());
        }
    }
}