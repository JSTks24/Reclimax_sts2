using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace Luminous;

[ModInitializer("Initialize")]
public static class Luminous {
    public static void Initialize() {
        Util.ModData.Init();
        Harmony harmony = new Harmony("Luminous");
        harmony.PatchAll();
    }
}