using HarmonyLib;
using Luminous.Cards;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Luminous.Hook;

[HarmonyPatch(typeof(IroncladCardPool), "GenerateAllCards")]
class IroncladPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) {
        List<CardModel> cards = [.. __result];
        cards.Add((CardModel)ModelDb.Card<DeathHarvest>());
        __result = [.. cards];
    }
}

[HarmonyPatch(typeof(DefectCardPool), "GenerateAllCards")]
class DefectPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) {
        List<CardModel> cards = [.. __result];
        cards.Add((CardModel)ModelDb.Card<ElectricityBing>());
        __result = [.. cards];
    }
}

[HarmonyPatch(typeof(SilentCardPool), "GenerateAllCards")]
class SilentPoolHook {
    static void Postfix(ref IEnumerable<CardModel> __result) {
        List<CardModel> cards = [.. __result];
        cards.Add((CardModel)ModelDb.Card<Absorbed>());
        __result = [.. cards];
    }
}