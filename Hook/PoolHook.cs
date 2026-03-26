using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using System.Reflection;

namespace Luminous.Hook;

[HarmonyPatch(typeof(IroncladCardPool), "GenerateAllCards")]
class IroncladPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) => Util.SetupPool<IroncladCardPool>(ref __result);
}

[HarmonyPatch(typeof(DefectCardPool), "GenerateAllCards")]
class DefectPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) => Util.SetupPool<DefectCardPool>(ref __result);
}

[HarmonyPatch(typeof(SilentCardPool), "GenerateAllCards")]
class SilentPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) => Util.SetupPool<SilentCardPool>(ref __result);
}

[HarmonyPatch(typeof(NecrobinderCardPool), "GenerateAllCards")]
class NecrobinderCardPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) => Util.SetupPool<NecrobinderCardPool>(ref __result);
}

[HarmonyPatch(typeof(RegentCardPool), "GenerateAllCards")]
class RegentCardPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) => Util.SetupPool<RegentCardPool>(ref __result);
}

static class Util {
    public static void SetupPool<T>(ref IEnumerable<CardModel> result) where T : CardPoolModel {
        List<CardModel> cards = [.. result];

        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        var metcardinfo = typeof(ModelDb).GetMethod("Card");
        var subTypes = currentAssembly.GetTypes()
                .Where(t => t.IsClass
                         && !t.IsAbstract
                         && t.IsSubclassOf(typeof(CardModel)))
                .ToList();
        foreach (Type card in subTypes) {
            var inst = metcardinfo!.MakeGenericMethod(card).Invoke(null, null);
            if (card.GetProperty("Pool")!.GetValue(inst) is T)
                cards.Add((CardModel)inst!);
        }

        result = [.. cards];
    }
}