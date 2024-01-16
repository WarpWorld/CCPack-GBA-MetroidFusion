/*using System.Collections.Generic;
using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.MetroidFusion;

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

        public override bool BidWarAction(string code)
            => Connector.Write8(ADDR_LANGUAGE, (byte)(code switch
            {
                "language_jp" => LanguageOption.Japanese,
                "language_en" => LanguageOption.English,
                "language_de" => LanguageOption.German,
                "language_fr" => LanguageOption.French,
                "language_it" => LanguageOption.Italian,
                "language_es" => LanguageOption.Spanish,
                _ => LanguageOption.English
            }));
    }
}*/