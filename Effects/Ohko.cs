using System.Collections.Generic;
using ConnectorLib;
using CrowdControl.Common;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.MetroidFusion;

public partial class MetroidFusion
{
    [EffectHandler("ohko")]
    public class Ohko : EffectHandler<MetroidFusion, IGBAConnector>
    {
        private const uint ADDR_HEALTH = 0x3001310;
        private const uint ADDR_MAX_HEALTH = 0x3001312;

        public Ohko(MetroidFusion pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Durational;

        public override IList<string> Codes { get; } = new[] { "health" };

        private ushort oldHealth;

        public override bool StartCondition()
            => Connector.Read16(ADDR_HEALTH, out oldHealth);

        public override bool RefreshAction()
            => Connector.Write16(ADDR_HEALTH, 0x01);

        public override SITimeSpan RefreshInterval { get; } = 0.2;

        public override bool StopAction()
            => Connector.Write16(ADDR_HEALTH, oldHealth);

        public override bool StartFollowup()
            => Connector.SendMessage($"{Request.DisplayViewer} started one-hit KO.");
    }
}