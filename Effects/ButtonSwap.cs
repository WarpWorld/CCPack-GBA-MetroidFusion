using System.Collections.Generic;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs
{
    public partial class Zelda3Randomizer
    {
        [EffectHandler("invertdpad1", "invertbuttons1", "invertboth1", "swap1")]
        public class ButtonSwap : TimedFlagEffect
        {
            public ButtonSwap(Zelda3Randomizer pack) : base(pack) { }

            public override ulong Address => ADDR_CONTROLLER_EFFECT;
            public override sbyte Flag => Lookup((sbyte)1, (sbyte)2, (sbyte)3, (sbyte)4);

            public override IList<string> Codes { get; } =
                new[] { "invertdpad1", "invertbuttons1", "invertboth1", "swap1" };

            public override IList<string> Mutexes { get; } = new[] { "buttonswap" };

            public override void StartFollowup()
            {
                EffectPack.PlaySFX(SFXType.Warning2);
                Connector.SendMessage(
                    $"{Request.DisplayViewer} {Lookup("inverted your d-pad", "inverted your buttons", "inverted your d-pad and buttons", "swapped your d-pad and buttons")}.");
            }

            public override bool StopAction()
                => Connector.Write8(ADDR_CONTROLLER_EFFECT, 0x00);

            public override void StopFollowup()
                => Connector.SendMessage(
                    $"{Request.DisplayViewer}'s control {Lookup("inversion", "inversion", "inversion", "swap")} has ended.");
        }
    }
}
