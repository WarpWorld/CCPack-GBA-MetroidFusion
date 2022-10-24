using System.Collections.Generic;
using ConnectorLib;
using CrowdControl.Common;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs
{
    public partial class MetroidFusion
    {
        [EffectHandler("language_jp", "language_en", "language_de", "language_fr", "language_it", "language_es")]
        public class Language : EffectHandler<MetroidFusion, IGBAConnector>
        {
            private const uint ADDR_LANGUAGE = 0x3000011;

            private enum LanguageOption : byte
            {
                //Japanese = 0x00,
                Japanese = 0x01,
                English = 0x02,
                German = 0x03,
                French = 0x04,
                Italian = 0x05,
                Spanish = 0x06
            }

            public Language(MetroidFusion pack) : base(pack) { }

            public override EffectHandlerType Type => EffectHandlerType.BidWar;

            public override IList<string> Codes { get; } = new[] { "language_jp", "language_en", "language_de", "language_fr", "language_it", "language_es" };

            public override bool StartAction()
                => Connector.Write8(ADDR_LANGUAGE, (byte)Lookup(LanguageOption.Japanese, LanguageOption.English, LanguageOption.German, LanguageOption.French, LanguageOption.Italian, LanguageOption.Spanish));
        }
    }
}
