using System;
using System.Collections.Generic;
using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs
{
    public partial class MetroidFusion
    {
        [EffectHandler("missiles_add", "missiles_remove")]
        public class MissilesUpDown : EffectHandler<MetroidFusion, IGBAConnector>
        {
            private const int ADDR_MISSILES = 0x3001314;

            public MissilesUpDown(MetroidFusion pack) : base(pack) { }

            public override EffectHandlerType Type => EffectHandlerType.Instant;

            public override IList<(string, Type)> Parameters { get; } = new[] { ("quantity", typeof(int)) };

            public override IList<string> Codes { get; } = new[] { "missiles_add", "missiles_remove" };

            public override bool StartAction()
                => Connector.RangeAdd16(ADDR_MISSILES, Quantity * Lookup(1, -1), 0, 999, false);

            public override bool StartFollowup()
                => Connector.SendMessage(
                    $"{Request.DisplayViewer} {Lookup("gave", "took")} {Quantity} missiles.");
        }
    }
}
