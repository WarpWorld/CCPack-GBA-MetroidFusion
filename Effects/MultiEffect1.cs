using System.Collections.Generic;
using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.MetroidFusion.Effects;

public partial class MetroidFusion
{
    [EffectHandler("openMap", "saxCutscene", "reboot")]
    public class MultiEffect1 : EffectHandler<MetroidFusion, IGBAConnector>
    {
        public MultiEffect1(MetroidFusion pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "openMap", "saxCutscene", "reboot" };

        public override IList<string> Mutexes { get; } = new[] { "mainState" };

        public override bool StartAction()
            => Connector.Write8(Packs.MetroidFusion.MetroidFusion.ADDR_GAME_MODE, Lookup<byte>(0x03, 0x05, 0x02));

        public override bool StartFollowup()
            => Connector.SendMessage($"{Request.DisplayViewer} {Lookup("opened your map.", "summoned SA-X?", "reset your game.")}");
    }
}