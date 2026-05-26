using AntiCheat;
using AntiCheat.Managers;
using AntiCheat.Patches;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using UnityEngine;
using static AntiCheat.Settings;
using static AntiCheat.References;
using System.Collections;
using UnityEngine.InputSystem.Controls;

[assembly: MelonGame("SchellGames")]
[assembly: MelonInfo(typeof(Core), "AntiCheat", "1.0.0", "TeamMessHall")]
namespace AntiCheat
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            foreach (Type type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    ClassInjector.RegisterTypeInIl2Cpp(type);
                }
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName != "Boot" && sceneName != "Title")
            {
                MelonCoroutines.Start(delayedStartSpeedCheck());
            }
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName == "Title")
            {
                ModStampManager.LoadModStamp();
                ReferencesSet = false;
            }
            if (sceneName != "Boot" && sceneName != "Title")
            {
                if (!ReferencesSet)
                {
                    ResetReferences();
                }
            }
        }

        public IEnumerator delayedStartSpeedCheck()
        {
            yield return new WaitForSeconds(5);

            MelonCoroutines.Start(AntiCheatManager.SpeedCheckLoop());
        }
    }
}