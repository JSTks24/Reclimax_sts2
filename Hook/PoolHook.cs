using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using System.Reflection;

namespace Luminous.Hook;

[HarmonyPatch(typeof(IroncladCardPool), "GenerateAllCards")]
class IroncladPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) => Util.SetupCardPool<IroncladCardPool>(ref __result);
}

[HarmonyPatch(typeof(DefectCardPool), "GenerateAllCards")]
class DefectPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) => Util.SetupCardPool<DefectCardPool>(ref __result);
}

[HarmonyPatch(typeof(SilentCardPool), "GenerateAllCards")]
class SilentPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) => Util.SetupCardPool<SilentCardPool>(ref __result);
}

[HarmonyPatch(typeof(NecrobinderCardPool), "GenerateAllCards")]
class NecrobinderCardPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) => Util.SetupCardPool<NecrobinderCardPool>(ref __result);
}

[HarmonyPatch(typeof(RegentCardPool), "GenerateAllCards")]
class RegentCardPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) => Util.SetupCardPool<RegentCardPool>(ref __result);
}

[HarmonyPatch(typeof(SharedRelicPool), "GenerateAllRelics")]
class SharedRelicPoolHook {
    static void Postfix(ref IEnumerable<RelicModel> __result) => Util.SetupRelicPool<SharedRelicPool>(ref __result);
}

static class Util {
    public static void SetupCardPool<T>(ref IEnumerable<CardModel> result) where T : CardPoolModel {
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
    public static void SetupRelicPool<T>(ref IEnumerable<RelicModel> result) where T : RelicPoolModel {
        List<RelicModel> relics = [.. result];

        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        var metrelicinfo = typeof(ModelDb).GetMethod("Relic");
        var subTypes = currentAssembly.GetTypes()
                .Where(t => t.IsClass
                         && !t.IsAbstract
                         && t.IsSubclassOf(typeof(RelicModel)))
                .ToList();
        foreach (Type relic in subTypes) {
            var inst = metrelicinfo!.MakeGenericMethod(relic).Invoke(null, null);
            if (relic.GetProperty("Pool")!.GetValue(inst) is T)
                relics.Add((RelicModel)inst!);
        }

        result = [.. relics];
    }
}