using Luminous.Util;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;
using System.Reflection;

namespace Luminous.Cards;

[LuminousPool<DefectCardPool>]
public sealed class Electrodynamics : CardModel {
    public Electrodynamics() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[2]
    {
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<LightningOrb>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("LightingBall", 2)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        for (int i = 0; i < this.DynamicVars["LightingBall"].BaseValue; i++) {
            await PowerCmd.Apply<ElectrodynamicsBuff>(this.Owner.Creature, 1m, this.Owner.Creature, this);
            await OrbCmd.Channel<LightningOrb>(choiceContext, base.Owner);
        }
    }
    protected override void OnUpgrade() => this.DynamicVars["LightingBall"].UpgradeValueBy(1);
}

public sealed class ElectrodynamicsBuff : PowerModel {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
}

[HarmonyLib.HarmonyPatch(typeof(LightningOrb), "ApplyLightningDamage")]
class LightningOrbHoverPatch {
    public static bool Prefix(LightningOrb __instance, ref Task<IEnumerable<Creature>> __result, decimal value, Creature? target, PlayerChoiceContext choiceContext) {
        bool has_buff = __instance.Owner.Creature.GetPower<ElectrodynamicsBuff>() != null;
        if (has_buff) {
            List<Creature> list = (from e in __instance.CombatState.GetOpponentsOf(__instance.Owner.Creature)
                                where e.IsHittable
                                select e).ToList();
            __result = Task.FromResult<IEnumerable<Creature>>(list);
            foreach (Creature item in list) {
                VfxCmd.PlayOnCreature(item, "vfx/vfx_attack_lightning");
            }
            typeof(OrbModel).GetMethod("PlayEvokeSfx", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(__instance, null);
            CreatureCmd.Damage(choiceContext, list, value, ValueProp.Unpowered, __instance.Owner.Creature);
            return false;
        }
        return true;
    }
}
