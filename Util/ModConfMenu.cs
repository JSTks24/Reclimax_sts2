using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;

namespace Luminous.Util;
[HarmonyPatch(typeof(NModInfoContainer), nameof(NModInfoContainer.Fill))]
public static class ModConfMenu {

    private static readonly Dictionary<string, bool> ModSettings = [];

    private const string ModId = "Reclimax";
    private const string SettingsBtnName = "ReclimaxBtn";
    private const string SettingsPanelName = "ReclimaxSettingsPanel";

    public static void SetSetting(string name, bool defaultValue) => ModSettings.Add(name, defaultValue);

    public static bool GetSetting(string name) => ModSettings[name];

    [HarmonyPostfix]
    public static void AddModifyBtn(NModInfoContainer __instance, Mod mod) {
        var imageNode = __instance.GetNodeOrNull<Control>("ModImage");
        if (imageNode == null)
            return;

        var parentPanel = imageNode.GetParent();
        var settingsBtn = imageNode.GetNodeOrNull<NButton>(SettingsBtnName);
        var settingsPanel = imageNode.GetNodeOrNull<PanelContainer>(SettingsPanelName);

        if (settingsBtn == null) {
            var templateBtn = __instance.Owner.GetNodeOrNull<NButton>("%GetModsButton");

            settingsBtn = (NButton)templateBtn.Duplicate();
            imageNode.AddChild(settingsBtn);

            settingsBtn.Name = SettingsBtnName;
            settingsBtn.Position = new Vector2(settingsBtn.Position.X, 0);
            settingsBtn.GetNode<MegaLabel>("Visuals/Label").SetTextAutoSize(new LocString("settings_ui", "mod_setting_btn").GetFormattedText());

            settingsBtn.Visible = true;

            settingsPanel = new PanelContainer { Name = SettingsPanelName, Visible = false, Size = new Vector2(imageNode.Size.X, 0) };
            imageNode.AddChild(settingsPanel);

            settingsPanel.Position = new Vector2(0, settingsBtn.Size.Y);

            var vbox = new VBoxContainer();
            settingsPanel.AddChild(vbox);

            foreach (var setting in ModSettings) {
                var confRow = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath("screens/modding/modding_screen_row")).Instantiate<NModMenuRow>();
                ;
                vbox.AddChild(confRow);
                var Tickbox = confRow.GetNodeOrNull<NTickbox>("Tickbox");
                Tickbox.SetAnchorsPreset(Control.LayoutPreset.TopRight);
                Tickbox.IsTicked = setting.Value;

                confRow.GetNodeOrNull<TextureRect>("PlatformIcon").Visible = false;
                confRow.GetNodeOrNull<MegaRichTextLabel>("Title").SetTextAutoSize(new LocString("settings_ui", setting.Key).GetFormattedText());

                Tickbox.Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>((_) => {
                    ModSettings[setting.Key] = Tickbox.IsTicked;
                }));
            }
            
            settingsBtn?.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((_) => {
                settingsPanel.Visible = !settingsPanel.Visible;
            }));
        }

        bool isMyMod = (mod.manifest?.id == ModId);
        if (settingsBtn != null)
            settingsBtn.Visible = isMyMod;
        if (!isMyMod && settingsPanel != null)
            settingsPanel.Visible = false;
    }
}
