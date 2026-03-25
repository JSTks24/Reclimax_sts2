using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace Luminous;

[ModInitializer("runHook")]
public static class Luminous {
    public static void runHook() {
        Harmony harmony = new Harmony("Luminous");
        harmony.PatchAll();
    }
}