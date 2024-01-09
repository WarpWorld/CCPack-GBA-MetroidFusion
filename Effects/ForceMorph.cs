using System.Collections.Generic;
using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.MetroidFusion;

public partial class MetroidFusion
{
    [EffectHandler("forcemorph")]
    public class ForceMorph : EffectHandler<MetroidFusion, IGBAConnector>
    {
        public ForceMorph(MetroidFusion pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Durational;

        public override IList<string> Codes { get; } = new[] { "forcemorph" };

        public override IList<string> Mutexes { get; } = new[] { "samusAction" };

        public override bool StartAction()
            => Connector.Write8(Packs.MetroidFusion.MetroidFusion.ADDR_SAMUS_ACTIONS, (byte)Packs.MetroidFusion.MetroidFusion.SamusActions.Morphball);

        public override bool StartFollowup()
            => Connector.SendMessage($"{Request.DisplayViewer} morphed you.");

        public override bool StopAction()
            => Connector.Unfreeze(Packs.MetroidFusion.MetroidFusion.ADDR_SAMUS_ACTIONS);
    }
}