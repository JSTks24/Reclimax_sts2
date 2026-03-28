using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using System.Reflection;

namespace Luminous.Hook;

[HarmonyPatch]
class CardPoolHook {
    static IEnumerable<MethodBase> TargetMethods() {
        yield return AccessTools.Method(typeof(IroncladCardPool), "GenerateAllCards");
        yield return AccessTools.Method(typeof(DefectCardPool), "GenerateAllCards");
        yield return AccessTools.Method(typeof(SilentCardPool), "GenerateAllCards");
        yield return AccessTools.Method(typeof(NecrobinderCardPool), "GenerateAllCards");
        yield return AccessTools.Method(typeof(RegentCardPool), "GenerateAllCards");
    }
    static void Postfix(object __instance, ref CardModel[] __result) {
        if (ModCache.CardCache.TryGetValue(__instance.GetType(), out var cachedCards)) {
            __result = [.. __result, .. cachedCards];
        }
    }
}

[HarmonyPatch]
class RelicPoolHook {
    static IEnumerable<MethodBase> TargetMethods() {
        yield return AccessTools.Method(typeof(SharedRelicPool), "GenerateAllRelics");
    }
    static void Postfix(object __instance, ref IEnumerable<RelicModel> __result) {
        if (ModCache.RelicCache.TryGetValue(__instance.GetType(), out var cachedRelics)) {
            __result = [.. __result, .. cachedRelics];
        }
    }
}

static class ModCache {
    internal static readonly Dictionary<Type, List<CardModel>> CardCache = [];
    internal static readonly Dictionary<Type, List<RelicModel>> RelicCache = [];
    static ModCache() {
        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        var subTypes = currentAssembly.GetTypes();
        var classType = typeof(ModelDb);
        DealHook(classType.GetMethod("Card")!, subTypes, ref CardCache);
        DealHook(classType.GetMethod("Relic")!, subTypes, ref RelicCache);
    }
    private static void DealHook<T>(MethodInfo methodInfo, Type[] subTypes, ref Dictionary<Type, List<T>> caches){
        var types = subTypes.Where(t => t.IsClass
                         && !t.IsAbstract
                         && t.IsSubclassOf(typeof(T)))
                .ToList();
        foreach (Type type in types) {
            var inst = methodInfo!.MakeGenericMethod(type).Invoke(null, null);
            var poolType = type.GetProperty("Pool")?.GetValue(inst)?.GetType();
            if (poolType != null) {
                if (!caches.TryGetValue(poolType, out var list)) {
                    list = [];
                    caches[poolType] = list;
                }
                list.Add((T)inst!);
            }
        }
    }
}