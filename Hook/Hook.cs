using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Luminous.Hook;

[HarmonyPatch(typeof(MegaCrit.Sts2.Core.Models.Characters.Necrobinder))]  // 替换为实际角色类名
[HarmonyPatch(nameof(MegaCrit.Sts2.Core.Models.Characters.Necrobinder.StartingDeck), MethodType.Getter)]
public static class Tx1 {
    static void Postfix(ref IEnumerable<CardModel> __result) {
        __result = new CardModel[10]
        {
            (CardModel) ModelDb.Card<BoneShards>(),
            (CardModel) ModelDb.Card<StrikeNecrobinder>(),
            (CardModel) ModelDb.Card<StrikeNecrobinder>(),
            (CardModel) ModelDb.Card<StrikeNecrobinder>(),
            (CardModel) ModelDb.Card<DefendNecrobinder>(),
            (CardModel) ModelDb.Card<DefendNecrobinder>(),
            (CardModel) ModelDb.Card<DefendNecrobinder>(),
            (CardModel) ModelDb.Card<DefendNecrobinder>(),
            (CardModel) ModelDb.Card<Bodyguard>(),
            (CardModel) ModelDb.Card<Unleash>()
        };
    }
}
[HarmonyPatch(typeof(MegaCrit.Sts2.Core.Models.Characters.Ironclad))]  // 替换为实际角色类名
[HarmonyPatch(nameof(MegaCrit.Sts2.Core.Models.Characters.Ironclad.StartingDeck), MethodType.Getter)]
public static class Tx2 {
    static void Postfix(ref IEnumerable<CardModel> __result) {
        __result = new CardModel[11]
        {
            (CardModel) ModelDb.Card<Feed>(),
            (CardModel) ModelDb.Card<StrikeIronclad>(),
            (CardModel) ModelDb.Card<StrikeIronclad>(),
            (CardModel) ModelDb.Card<StrikeIronclad>(),
            (CardModel) ModelDb.Card<StrikeIronclad>(),
            (CardModel) ModelDb.Card<DefendIronclad>(),
            (CardModel) ModelDb.Card<DefendIronclad>(),
            (CardModel) ModelDb.Card<DefendIronclad>(),
            (CardModel) ModelDb.Card<DefendIronclad>(),
            (CardModel) ModelDb.Card<Bash>(),
            (CardModel) ModelDb.Card<Bloodletting>()
        };
    }
}
[HarmonyPatch(typeof(MegaCrit.Sts2.Core.Models.Characters.Silent))]  // 替换为实际角色类名
[HarmonyPatch(nameof(MegaCrit.Sts2.Core.Models.Characters.Silent.StartingDeck), MethodType.Getter)]
public static class Tx3 {
    static void Postfix(ref IEnumerable<CardModel> __result) {
        __result = new CardModel[10]
        {
            (CardModel) ModelDb.Card<StrikeSilent>(),
            (CardModel) ModelDb.Card<StrikeSilent>(),
            (CardModel) ModelDb.Card<StrikeSilent>(),
            (CardModel) ModelDb.Card<StrikeSilent>(),
            (CardModel) ModelDb.Card<DefendSilent>(),
            (CardModel) ModelDb.Card<DefendSilent>(),
            (CardModel) ModelDb.Card<DefendSilent>(),
            (CardModel) ModelDb.Card<DefendSilent>(),
            (CardModel) ModelDb.Card<Neutralize>(),
            (CardModel) ModelDb.Card<Survivor>()
        };
    }
}
[HarmonyPatch(typeof(MegaCrit.Sts2.Core.Models.Characters.Regent))]  // 替换为实际角色类名
[HarmonyPatch(nameof(MegaCrit.Sts2.Core.Models.Characters.Regent.StartingDeck), MethodType.Getter)]
public static class Tx4 {
    static void Postfix(ref IEnumerable<CardModel> __result) {
        __result = new CardModel[10]
        {
            (CardModel) ModelDb.Card<CrushUnder>(),
            (CardModel) ModelDb.Card<StrikeRegent>(),
            (CardModel) ModelDb.Card<StrikeRegent>(),
            (CardModel) ModelDb.Card<StrikeRegent>(),
            (CardModel) ModelDb.Card<Patter>(),
            (CardModel) ModelDb.Card<DefendRegent>(),
            (CardModel) ModelDb.Card<DefendRegent>(),
            (CardModel) ModelDb.Card<DefendRegent>(),
            (CardModel) ModelDb.Card<FallingStar>(),
            (CardModel) ModelDb.Card<Venerate>()
        };
    }
}
[HarmonyPatch(typeof(MegaCrit.Sts2.Core.Models.Characters.Defect))]  // 替换为实际角色类名
[HarmonyPatch(nameof(MegaCrit.Sts2.Core.Models.Characters.Defect.StartingDeck), MethodType.Getter)]
public static class TX5 {
    static void Postfix(ref IEnumerable<CardModel> __result) {
        __result = new CardModel[10]
        {
            (CardModel) ModelDb.Card<BallLightning>(),
            (CardModel) ModelDb.Card<StrikeDefect>(),
            (CardModel) ModelDb.Card<StrikeDefect>(),
            (CardModel) ModelDb.Card<StrikeDefect>(),
            (CardModel) ModelDb.Card<DefendDefect>(),
            (CardModel) ModelDb.Card<DefendDefect>(),
            (CardModel) ModelDb.Card<DefendDefect>(),
            (CardModel) ModelDb.Card<DefendDefect>(),
            (CardModel) ModelDb.Card<Zap>(),
            (CardModel) ModelDb.Card<Dualcast>()
        };
    }
}