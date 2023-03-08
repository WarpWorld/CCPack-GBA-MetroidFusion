using System;
using CrowdControl.Common;
using CrowdControl.Games.SmartEffects;
using JetBrains.Annotations;

//ccpragma { "include" : [ ".\\Effects\\*.cs" ] }
namespace CrowdControl.Games.Packs
{
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

        public MetroidFusion(Player player, Func<CrowdControlBlock, bool> responseHandler, Action<object> statusUpdateHandler) : base(player, responseHandler, statusUpdateHandler) { }

        public override EffectList Effects { get; } = new Effect[]
        {
            new("Kill Samus", "killplayer"),
            new("Force Morphball", "forceMorph"){Duration = 15},
            new("Freeze Samus", "freeze"){Duration = 10},

            new("Enable Shinespark", "shinespark"),
            //new("Force Shinespark", "shinesparkForced"),
            new("Slow Samus", "slow"){Duration = 15},

            new("Open Map", "openMap"),
            new("SA-X Close Up!", "saxCutscene"),
            new("Restart Game", "reboot"),

            new("Give Missiles", "missiles_add") { Quantity = 999 },
            new("Take Missiles", "missiles_remove") { Quantity = 999 },
            new("Give Power Bombs", "bombs_add") { Quantity = 99 },
            new("Take Power Bombs", "bombs_remove") { Quantity = 99 },

            new("Fire Charge Beam", "fire_chargebeam"),
            new("Fire Missiles", "fire_missile"),
            new("Fire Bombs", "fire_bomb"),
            new("Fire Charge Bombs", "fire_chargebomb"),
            new("Fire Power Bombs", "fire_powerbomb"),

            new("One-Hit KO", "ohko"){Duration = 15},

            //language_jp", "language_en", "language_de", "language_fr", "language_it", "language_es
            new("Game Language", "language", ItemKind.BidWar)
            {
                Parameters = new ParameterGroup("Language",
                    new("Japanese", "language_jp"),
                    new("English", "language_en"),
                    new("German", "language_de"),
                    new("French", "language_fr"),
                    new("Italian", "language_it"),
                    new("Spanish", "language_es")
                    )
            }
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

        public override Game Game { get; } = new(0, "Metroid Fusion", "MetroidFusion", "GBA", ConnectorType.GBAConnector);

        protected override bool IsReady(EffectRequest request) => Connector.IsEqual8(ADDR_GAME_MODE, 0x01);

        protected override void StartEffect(EffectRequest request)
        {
            if (!IsReady(request))
            {
                DelayEffect(request);
                return;
            }

            string[] codeParams = FinalCode(request).Split('_');
            switch (codeParams[0])
            {
                
            }
        }
    }
}
