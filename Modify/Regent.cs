using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Luminous.Modify;

[HarmonyPatch]
public class Regent {
    [HarmonyPatch(typeof(Alignment), "CanonicalEnergyCost", MethodType.Getter)]
    [HarmonyPostfix]
    public static void Alignment(ref int __result) {
        __result = 2;
    }

    [HarmonyPatch(typeof(Glow))]
    public class GlowPatch {

    }
}
