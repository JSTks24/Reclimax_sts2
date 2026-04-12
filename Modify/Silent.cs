//using HarmonyLib;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.Models.Cards;

//namespace Luminous.Modify;

//[HarmonyPatch]
//public class Silent {
//    [HarmonyPatch(typeof(BladeDance), nameof(BladeDance.CanonicalKeywords), MethodType.Getter)]
//    [HarmonyPostfix]
//    static public void RemoveExhaust(ref IEnumerable<CardKeyword> __result) {
//        __result = [];
//    }
//}