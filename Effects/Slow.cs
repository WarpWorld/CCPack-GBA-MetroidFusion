using System.Collections.Generic;
using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.MetroidFusion.Effects;

public partial class MetroidFusion
{
    [EffectHandler("slow")]
    public class Slow : EffectHandler<MetroidFusion, IGBAConnector>
    {
        private const uint ADDR_SLOW = 0x3001330;

        public Slow(MetroidFusion pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Durational;

        public override IList<string> Codes { get; } = new[] { "slow" };

        public override bool RetryOnFail => false;

        public override bool StartAction()
            => Connector.Write8(ADDR_SLOW, 0xFF);

        public override bool StartFollowup()
            => Connector.SendMessage($"{Request.DisplayViewer} slowed you down.");
    }
}