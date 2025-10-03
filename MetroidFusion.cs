using ConnectorLib;
using CrowdControl.Common;
using CrowdControl.Games.SmartEffects;
using JetBrains.Annotations;
using ConnectorType = CrowdControl.Common.ConnectorType;

//ccpragma { "include" : [ ".\\Effects\\*.cs" ] }
namespace CrowdControl.Games.Packs.MetroidFusion;

[UsedImplicitly]
partial class MetroidFusion : GBAEffectPack, IHandlerCollection
{
    private const uint ADDR_SAMUS_ACTIONS = 0x03001245;

    private enum SamusActions : byte
    {
        Morphball = 0x0C,
        Kill = 0x3E,
        Freeze = 0x30
    }

    private const uint ADDR_GAME_MODE = 0x3000BDE;

    public MetroidFusion(UserRecord player, Func<CrowdControlBlock, bool> responseHandler, Action<object> statusUpdateHandler) : base(player, responseHandler, statusUpdateHandler) { }

    public override EffectList Effects { get; } = new Effect[]
    {
        new("Kill Samus", "killplayer") { Alignment = (Alignment)Morality.ExtremelyHarmful + Orderliness.Controlled },
        new("Force Morphball", "forceMorph") { Duration = 15, Alignment = (Alignment)Morality.SlightlyHarmful + Orderliness.Controlled /* TODO: Confirm severity (mobility tradeoffs) */ },
        new("Freeze Samus", "freeze") { Duration = 10, Alignment = (Alignment)Morality.Harmful + Orderliness.Controlled },

        new("Enable Shinespark", "shinespark") { Alignment = (Alignment)Morality.Helpful + Orderliness.Controlled },
        //new("Force Shinespark", "shinesparkForced"),
        new("Slow Samus", "slow") { Duration = 15, Alignment = (Alignment)Morality.SlightlyHarmful + Orderliness.Controlled },

        new("Open Map", "openMap") { Alignment = (Alignment)Morality.SlightlyHelpful + Orderliness.Controlled },
        new("SA-X Close Up!", "saxCutscene") { Alignment = (Alignment)Morality.SlightlyHarmful + Orderliness.Chaotic }, /* not actually harmful, but disruptive */
        new("Restart Game", "reboot") { Alignment = (Alignment)Morality.ExtremelyHarmful + Orderliness.Controlled },

        new("Give Missiles", "missiles_add") { Quantity = 999, Alignment = (Alignment)Morality.Helpful + Orderliness.Controlled },
        new("Take Missiles", "missiles_remove") { Quantity = 999, Alignment = (Alignment)Morality.VeryHarmful + Orderliness.Controlled },
        new("Give Power Bombs", "bombs_add") { Quantity = 99, Alignment = (Alignment)Morality.Helpful + Orderliness.Controlled },
        new("Take Power Bombs", "bombs_remove") { Quantity = 99, Alignment = (Alignment)Morality.VeryHarmful + Orderliness.Controlled },

        /* Mixed: can consume resources but can help offensively or with obstacles */
        new("Fire Charge Beam", "fire_chargebeam") { Alignment = (Alignment)Morality.Neutral + Orderliness.SlightlyChaotic },
        new("Fire Missiles", "fire_missile") { Alignment = (Alignment)Morality.Neutral + Orderliness.SlightlyChaotic },
        new("Fire Bombs", "fire_bomb") { Alignment = (Alignment)Morality.Neutral + Orderliness.SlightlyChaotic },
        new("Fire Charge Bombs", "fire_chargebomb") { Alignment = (Alignment)Morality.Neutral + Orderliness.SlightlyChaotic },
        new("Fire Power Bombs", "fire_powerbomb") { Alignment = (Alignment)Morality.Neutral + Orderliness.SlightlyChaotic },

        new("One-Hit KO", "ohko") { Duration = 15, Alignment = (Alignment)Morality.VeryHarmful + Orderliness.Controlled },

        //language_jp", "language_en", "language_de", "language_fr", "language_it", "language_es
        /*new("Game Language", "language", ItemKind.BidWar)
        {
            Parameters = new ParameterList("Language",
                new("Japanese", "language_jp"),
                new("English", "language_en"),
                new("German", "language_de"),
                new("French", "language_fr"),
                new("Italian", "language_it"),
                new("Spanish", "language_es")
                )
        }*/
        /*Invert Controls/Buttons
            Enemy Effects
            Energy Tank 
            Increase Boss health
            Equip/Unequip weapons
            Change Room Type*/
    };

    public override ROMTable ROMTable => new[]
    {
        new ROMInfo("Metroid Fusion (USA)", ROMStatus.ValidPatched, s => Patching.MD5(s, "AF5040FC0F579800151EE2A683E2E5B5")),
        new ROMInfo("Metroid Fusion (Japan)", ROMStatus.NotSupported, s => Patching.MD5(s, "E535A6EC2EB86D183453037289527A63"), "The Japanese ROM is not supported."),
        new ROMInfo("Metroid Fusion (Europe)", ROMStatus.NotSupported, s => Patching.MD5(s, "EB462F708C715309D08FD7968825AE9E"), "The European ROM is not supported."),
        new ROMInfo("Metroid Fusion Beta 2002-09-16 (Europe)", ROMStatus.NotSupported, s => Patching.MD5(s, "305060AA2E1F0A854B772FA23988C2B9"), "Beta ROM versions are not supported."),
        new ROMInfo("Metroid Fusion Beta 2002-09-11 (Europe)", ROMStatus.NotSupported, s => Patching.MD5(s, "37477683A1E646431C5396164DF388F3"), "Beta ROM versions are not supported.")
    };

    public override Game Game { get; } = new("Metroid Fusion", "MetroidFusion", "GBA", ConnectorType.GBAConnector);

    protected override GameState GetGameState()
    {
        return Connector.IsEqual8(ADDR_GAME_MODE, 0x01) ? GameState.Ready : GameState.WrongMode;
    }

    public override bool StopAllEffects()
    {
        bool success = base.StopAllEffects();
        try
        {
            success &= Connector.Unfreeze(ADDR_SAMUS_ACTIONS);
            //success &= Connector.Write8(ADDR_SLOW, 0xFF);
        }
        catch { success = false; }
        return success;
    }
}