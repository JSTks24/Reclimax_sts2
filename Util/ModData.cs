using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using System.Reflection;

namespace Luminous.Util;

[AttributeUsage(AttributeTargets.Class)]
public class LuminousPoolAttribute<T>() : LuminousBaseAttribute where T : AbstractModel {
    public override Type PoolType => typeof(T);
}

public abstract class LuminousBaseAttribute : Attribute {
    public abstract Type PoolType { get; }
}

static class ModData {
    static private readonly List<Type> Types = [];
    static ModData() {
        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        Types = currentAssembly.GetTypes().Where(t => t.IsClass && (!t.IsAbstract || t.IsSealed)).ToList();
        SetModels();
    }
    static private void SetModels() {
        foreach (Type type in Types) {
            var attr = type.GetCustomAttribute<LuminousBaseAttribute>();
            if (attr != null)
                ModHelper.AddModelToPool(attr.PoolType, type);
        }
    }
    static public void Init() {
        ModConfMenu.SetSetting("StartingDeck", true);
    }
}