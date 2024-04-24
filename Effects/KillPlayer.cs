using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.MetroidFusion;

public partial class MetroidFusion
{
    [EffectHandler("killplayer")]
    public class KillPlayer : EffectHandler<MetroidFusion, IGBAConnector>
    {
        public KillPlayer(MetroidFusion pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "killplayer" };

        public override IList<string> Mutexes { get; } = new[] { "samusAction", "health" };

        public override bool StartAction()
            => Connector.Write8(ADDR_SAMUS_ACTIONS, (byte)SamusActions.Kill);

        public override bool StartFollowup()
            => Connector.SendMessage($"{Request.DisplayViewer} killed you.");
    }
}