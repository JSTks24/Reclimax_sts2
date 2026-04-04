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
    static ModData() {
        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        var types = currentAssembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToList();
        foreach (Type type in types) {
            var attr = type.GetCustomAttribute<LuminousBaseAttribute>();
            if (attr != null)
                ModHelper.AddModelToPool(attr.PoolType, type);
        }
    }
    static public void Init() { }
}