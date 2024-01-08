using System.Collections.Generic;
using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.MetroidFusion;

public partial class MetroidFusion
{
    [EffectHandler("shinespark")]
    public class Shinespark : EffectHandler<MetroidFusion, IGBAConnector>
    {
        private const uint ADDR_SHINESPARK = 0x30012DC;

        public Shinespark(MetroidFusion pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "shinespark" };

        public override bool RetryOnFail => false;

        public override bool StartCondition()
            => Connector.IsZero8(ADDR_SHINESPARK);

        public override bool StartAction()
            => Connector.Write8(ADDR_SHINESPARK, 0xFF);

        public override bool StartFollowup()
            => Connector.SendMessage($"{Request.DisplayViewer} gave you shinespark.");
    }
}