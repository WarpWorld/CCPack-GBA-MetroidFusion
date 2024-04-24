using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.MetroidFusion;

public partial class MetroidFusion
{
    [EffectHandler("fire_chargebeam", "fire_missile", "fire_bomb", "fire_chargebomb", "fire_powerbomb")]
    public class FireWeapon : EffectHandler<MetroidFusion, IGBAConnector>
    {
        private const uint ADDR_FIRE_WEAPON = 0x0300124D;
        private const uint ADDR_COOLDOWN = 0x300124E;

        private enum WeaponType : byte
        {
            None = 0x00,
            Beam = 0x01,
            Missle = 0x02,
            Bomb = 0x04,
            ChargeBomb = 0x05,
            PowerBomb = 0x06
        }

        public FireWeapon(MetroidFusion pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "fire_chargebeam", "fire_missile", "fire_bomb", "fire_chargebomb", "fire_powerbomb" };

        public override IList<string> Mutexes { get; } = new[] { "fireWeapon" };

        public override bool StartCondition()
            => Connector.IsZero8(ADDR_COOLDOWN);

        public override bool StartFollowup()
            => Connector.SendMessage($"{Request.DisplayViewer} fired your {Lookup("charge beam", "missiles", "bombs", "charge bombs", "power bombs")}.");

        public override bool StartAction()
            => Connector.Write8(ADDR_FIRE_WEAPON, (byte)Lookup(WeaponType.Beam, WeaponType.Missle, WeaponType.Bomb, WeaponType.ChargeBomb, WeaponType.PowerBomb));
    }
}