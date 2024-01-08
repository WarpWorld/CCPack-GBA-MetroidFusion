using System;
using System.Collections.Generic;
using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.MetroidFusion;

public partial class MetroidFusion
{
    [EffectHandler("bombs_add", "bombs_remove")]
    public class PowerBombs : EffectHandler<MetroidFusion, IGBAConnector>
    {
        private const int ADDR_BOMBS = 0x3001318;

        public PowerBombs(MetroidFusion pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<(string, Type)> Parameters { get; } = new[] { ("quantity", typeof(int)) };

        public override IList<string> Codes { get; } = new[] { "bombs_add", "bombs_remove" };

        public override bool StartAction()
            => Connector.RangeAdd16(ADDR_BOMBS, Quantity * Lookup(1, -1), 0, 99, false);

        public override bool StartFollowup()
            => Connector.SendMessage(
                $"{Request.DisplayViewer} {Lookup("gave", "took")} {Quantity} bombs.");
    }
}