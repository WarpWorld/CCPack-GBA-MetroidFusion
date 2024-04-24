using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.MetroidFusion;

public partial class MetroidFusion
{
    [EffectHandler("freeze")]
    public class Freeze : EffectHandler<MetroidFusion, IGBAConnector>
    {
        public Freeze(MetroidFusion pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Durational;

        public override IList<string> Codes { get; } = new[] { "freeze" };

        public override IList<string> Mutexes { get; } = new[] { "samusAction" };

        public override bool StartAction()
            => Connector.Freeze8(ADDR_SAMUS_ACTIONS, (byte)SamusActions.Freeze) &&
               Connector.Write8(0x03001253, 0xFF);

        public override bool StartFollowup()
            => Connector.SendMessage($"{Request.DisplayViewer} froze you.");

        public override bool StopAction()
            => Connector.Unfreeze(ADDR_SAMUS_ACTIONS);
    }
}