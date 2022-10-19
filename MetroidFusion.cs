using System;
using System.Collections.Generic;
using CrowdControl.Common;
using JetBrains.Annotations;

namespace CrowdControl.Games.Packs
{
    [UsedImplicitly]
    public class MegaManBattleNetwork3 : GBAEffectPack
    {
        private readonly uint OffsetStoryProgress = 0x02001886;
        private readonly uint OffsetIsGamePaused = 0x02001889;
        private readonly uint OffsetInBattleCheck = 0x020097F8;
        private readonly uint OffsetZenny = 0x020018F4;
        private readonly uint OffsetFrags = 0x020018F8;
        private readonly uint OffsetCurrentHP = 0x02037294;
        private readonly uint OffsetStyleActive = 0x02001881;
        private readonly uint OffsetStyleStored = 0x02001894;
        private readonly uint OffsetStepCounter = 0x02001DDC;
        private readonly uint OffsetBackpack = 0x02001F60;
        private readonly uint OffsetFolders = 0x02001410;
        private readonly uint OffsetCustomChipCount = 0x02006CAE;
        private readonly uint OffsetCustomMeter = 0x02006CCC;
        private readonly uint OffsetHandChipsRemaining = 0x0203728A;
        private readonly uint OffsetHandChipsCount = 0x020384d8; // Divide by 2 for actual Hand Count.
        private readonly uint OffsetHandChipsAttackType = 0x02034060;
        private readonly uint OffsetHandChipsReadLoc = 0x0200F862;
        private readonly uint OffsetHandChipsDamage = 0x0203406C;
        private readonly uint OffsetHandChipsDamageBonus = 0x02034078;
        private readonly uint OffsetHandChipsSprites = 0x06017540;
        private readonly uint OffsetROMBattleChipData = 0x08011510;

        public MegaManBattleNetwork3(IPlayer player, Func<CrowdControlBlock, bool> responseHandler, Action<object> statusUpdateHandler) : base(player, responseHandler, statusUpdateHandler) { }

        private Dictionary<string, byte> StyleChanges = new()
        {
            {"Normal Style",0x0},
            {"Elec Guts",0x9},
            {"Heat Guts",0xA},
            {"Aqua Guts",0xB},
            {"Wood Guts",0xC},
            {"Elec Custom",0x11},
            {"Heat Custom",0x12},
            {"Aqua Custom",0x13},
            {"Wood Custom",0x14},
            {"Elec Team",0x19},
            {"Heat Team",0x1A},
            {"Aqua Team",0x1B},
            {"Wood Team",0x1C},
            {"Elec Shield",0x21},
            {"Heat Shield",0x22},
            {"Aqua Shield",0x23},
            {"Wood Shield",0x24},
            {"Elec Ground",0x29},
            {"Heat Ground",0x2A},
            {"Aqua Ground",0x2B},
            {"Wood Ground",0x2C},
            {"Elec Shadow",0x31},
            {"Heat Shadow",0x32},
            {"Aqua Shadow",0x33},
            {"Wood Shadow",0x34},
            {"Elec Bug",0x39},
            {"Heat Bug",0x3A},
            {"Aqua Bug",0x3B},
            {"Wood Bug",0x3C}
        };

        private Dictionary<string, byte> ChipCodes = new()
        {
            {"A", 0},
            {"B", 1},
            {"C", 2},
            {"D", 3},
            {"E", 4},
            {"F", 5},
            {"G", 6},
            {"H", 7},
            {"I", 8},
            {"J", 9},
            {"K", 10},
            {"L", 11},
            {"M", 12},
            {"N", 13},
            {"O", 14},
            {"P", 15},
            {"Q", 16},
            {"R", 17},
            {"S", 18},
            {"T", 19},
            {"U", 20},
            {"V", 21},
            {"W", 22},
            {"X", 23},
            {"Y", 24},
            {"Z", 25},
            {"*", 26}
        };

        private Dictionary<string, ushort> ProgramAdvances = new()
        {
            {"Z-Canon1", 320},
            {"Z-Canon2", 321},
            {"Z-Canon3", 322},
            {"Z-Punch", 323},
            {"Z-Strght", 324},
            {"Z-Impact", 325},
            {"Z-Varibl", 326},
            {"Z-Yoyo1", 327},
            {"Z-Yoyo2", 328},
            {"Z-Yoyo3", 329},
            {"Z-Step1", 330},
            {"Z-Step2", 331},
            {"TimeBom+", 332},
            {"H-Burst", 333},
            {"BubSprd", 334},
            {"HeatSprd", 335},
            {"LifeSwrd", 336},
            {"ElemSwrd", 337},
            {"EvilCut", 338},
            {"2xHero", 339},
            {"HyperRat", 340},
            {"EverCrse", 341},
            {"GelRain", 342},
            {"PoisPhar", 343},
            {"BodyGrd", 344},
            {"500Barr", 345},
            {"BigHeart", 346},
            {"GtsShoot", 347},
            {"DeuxHero", 348},
            {"MomQuake", 349},
            {"PrixPowr", 350},
            {"MstrStyl", 351}
        };

        private Dictionary<string, ushort> BattleChips = new()
        {
            {"Cannon", 1},
            {"HiCannon", 2},
            {"M-Cannon", 3},
            {"AirShot1", 4},
            {"AirShot2", 5},
            {"AirShot3", 6},
            {"LavaCan1", 7},
            {"LavaCan2", 8},
            {"LavaCan3", 9},
            {"ShotGun", 10},
            {"V-Gun", 11},
            {"SideGun", 12},
            {"Spreader", 13},
            {"Bubbler", 14},
            {"Bub-V", 15},
            {"BublSide", 16},
            {"HeatShot", 17},
            {"Heat-V", 18},
            {"HeatSide", 19},
            {"MiniBomb", 20},
            {"SnglBomb", 21},
            {"DublBomb", 22},
            {"TrplBomb", 23},
            {"CannBall", 24},
            {"IceBall", 25},
            {"LavaBall", 26},
            {"BlkBomb1", 27},
            {"BlkBomb2", 28},
            {"BlkBomb3", 29},
            {"Sword", 30},
            {"WideSwrd", 31},
            {"LongSwrd", 32},
            {"FireSwrd", 33},
            {"AquaSwrd", 34},
            {"ElecSwrd", 35},
            {"BambSwrd", 36},
            {"CustSwrd", 37},
            {"VarSwrd", 38},
            {"StepSwrd", 39},
            {"StepCros", 40},
            {"Panic", 41},
            {"AirSwrd", 42},
            {"Slasher", 43},
            {"ShockWav", 44},
            {"SonicWav", 45},
            {"DynaWave", 46},
            {"GutPunch", 47},
            {"GutStrgt", 48},
            {"GutImpct", 49},
            {"AirStrm1", 50},
            {"AirStrm2", 51},
            {"AirStrm3", 52},
            {"DashAtk", 53},
            {"Burner", 54},
            {"Totem1", 55},
            {"Totem2", 56},
            {"Totem3", 57},
            {"Ratton1", 58},
            {"Ratton2", 59},
            {"Ratton3", 60},
            {"Wave", 61},
            {"RedWave", 62},
            {"MudWave", 63},
            {"Hammer", 64},
            {"Tornado", 65},
            {"ZapRing1", 66},
            {"ZapRing2", 67},
            {"ZapRing3", 68},
            {"Yo-Yo1", 69},
            {"Yo-Yo2", 70},
            {"Yo-Yo3", 71},
            {"Spice1", 72},
            {"Spice2", 73},
            {"Spice3", 74},
            {"Lance", 75},
            {"Scuttlst", 76},
            {"Momogra", 77},
            {"Rope1", 78},
            {"Rope2", 79},
            {"Rope3", 80},
            {"Magnum1", 81},
            {"Magnum2", 82},
            {"Magnum3", 83},
            {"Boomer1", 84},
            {"Boomer2", 85},
            {"Boomer3", 86},
            {"RndmMetr", 87},
            {"HoleMetr", 88},
            {"ShotMetr", 89},
            {"IceWave1", 90},
            {"IceWave2", 91},
            {"IceWave3", 92},
            {"Plasma1", 93},
            {"Plasma2", 94},
            {"Plasma3", 95},
            {"Arrow1", 96},
            {"Arrow2", 97},
            {"Arrow3", 98},
            {"TimeBomb", 99},
            {"Mine", 100},
            {"Sensor1", 101},
            {"Sensor2", 102},
            {"Sensor3", 103},
            {"CrsShld1", 104},
            {"CrsShld2", 105},
            {"CrsShld3", 106},
            {"Geyser", 107},
            {"PoisMask", 108},
            {"PoisFace", 109},
            {"Shake1", 110},
            {"Shake2", 111},
            {"Shake3", 112},
            {"BigWave", 113},
            {"Volcano", 114},
            {"Condor", 115},
            {"Burning", 116},
            {"FireRatn", 117},
            {"Guard", 118},
            {"PanlOut1", 119},
            {"PanlOut3", 120},
            {"Recov10", 121},
            {"Recov30", 122},
            {"Recov50", 123},
            {"Recov80", 124},
            {"Recov120", 125},
            {"Recov150", 126},
            {"Recov200", 127},
            {"Recov300", 128},
            {"PanlGrab", 129},
            {"AreaGrab", 130},
            {"Snake", 131},
            {"Team1", 132},
            {"MetaGel1", 133},
            {"MetaGel2", 134},
            {"MetaGel3", 135},
            {"GrabBack", 136},
            {"GrabRvng", 137},
            {"Geddon1", 138},
            {"Geddon2", 139},
            {"Geddon3", 140},
            {"RockCube", 141},
            {"Prism", 142},
            {"Wind", 143},
            {"Fan", 144},
            {"RockArm1", 145},
            {"RockArm2", 146},
            {"RockArm3", 147},
            {"NoBeam1", 148},
            {"NoBeam2", 149},
            {"NoBeam3", 150},
            {"Pawn", 151},
            {"Knight", 152},
            {"Rook", 153},
            {"Needler1", 154},
            {"Needler2", 155},
            {"Needler3", 156},
            {"SloGauge", 157},
            {"FstGauge", 158},
            {"Repair", 159},
            {"Invis", 160},
            {"Hole", 161},
            {"Mole1", 162},
            {"Mole2", 163},
            {"Mole3", 164},
            {"Shadow", 165},
            {"Mettaur", 166},
            {"Bunny", 167},
            {"AirShoes", 168},
            {"Team2", 169},
            {"Fanfare", 170},
            {"Discord", 171},
            {"Timpani", 172},
            {"Barrier", 173},
            {"Barr100", 174},
            {"Barr200", 175},
            {"Aura", 176},
            {"NrthWind", 177},
            {"HolyPanl", 178},
            {"LavaStge", 179},
            {"IceStage", 180},
            {"GrassStg", 181},
            {"SandStge", 182},
            {"MetlStge", 183},
            {"Snctuary", 184},
            {"Swordy", 185},
            {"Spikey", 186},
            {"Mushy", 187},
            {"Jelly", 188},
            {"KillrEye", 189},
            {"AntiNavi", 190},
            {"AntiDmg", 191},
            {"AntiSwrd", 192},
            {"AntiRecv", 193},
            {"CopyDmg", 194},
            {"Atk+10", 195},
            {"Fire+30", 196},
            {"Aqua+30", 197},
            {"Elec+30", 198},
            {"Wood+30", 199},
            {"Navi+20", 200},
            {"LifeAura", 201},
            {"Muramasa", 202},
            {"Guardian", 203},
            {"Anubis", 204},
            {"Atk+30", 205},
            {"Navi+40", 206},
            {"HeroSwrd", 207},
            {"ZeusHamr", 208},
            {"GodStone", 209},
            {"OldWood", 210},
            {"FullCust", 211},
            {"Meteors", 212},
            {"Poltrgst", 213},
            {"Jealousy", 214},
            {"StandOut", 215},
            {"WatrLine", 216},
            {"Ligtning", 217},
            {"GaiaSwrd", 218},
            {"Roll V1", 219},
            {"Roll V2", 220},
            {"Roll V3", 221},
            {"GutsMan V1", 222},
            {"GutsMan V2", 223},
            {"GutsMan V3", 224},
            {"GutsMan V4", 225},
            {"GutsMan V5", 226},
            {"ProtoMan V1", 227},
            {"ProtoMan V2", 228},
            {"ProtoMan V3", 229},
            {"ProtoMan V4", 230},
            {"Protoman V5", 231},
            {"FlashMan V1", 232},
            {"FlashMan V2", 233},
            {"FlashMan V3", 234},
            {"FlashMan V4", 235},
            {"FlashMan V5", 236},
            {"BeastMan V1", 237},
            {"BeastMan V2", 238},
            {"BeastMan V3", 239},
            {"BeastMan V4", 240},
            {"BeastMan V5", 241},
            {"BubblMan V1", 242},
            {"BubblMan V2", 243},
            {"BubblMan V3", 244},
            {"BubblMan V4", 245},
            {"BubblMan V5", 246},
            {"DesrtMan V1", 247},
            {"DesrtMan V2", 248},
            {"DesrtMan V3", 249},
            {"DesrtMan V4", 250},
            {"DesrtMan V5", 251},
            {"PlantMan V1", 252},
            {"PlantMan V2", 253},
            {"PlantMan V3", 254},
            {"PlantMan V4", 255},
            {"PlantMan V5", 256},
            {"FlamMan V1", 257},
            {"FlamMan V2", 258},
            {"FlamMan V3", 259},
            {"FlamMan V4", 260},
            {"FlamMan V5", 261},
            {"DrillMan V1", 262},
            {"DrillMan V2", 263},
            {"DrillMan V3", 264},
            {"DrillMan V4", 265},
            {"DrillMan V5", 266},
            {"MetalMan V1", 267},
            {"MetalMan V2", 268},
            {"MetalMan V3", 269},
            {"MetalMan V4", 270},
            {"MetalMan V5", 271},
            {"Punk", 272},
            {"Salamndr", 273},
            {"Fountain", 274},
            {"Bolt", 275},
            {"GaiaBlad", 276},
            {"KingMan V1", 277},
            {"KingMan V2", 278},
            {"KingMan V3", 279},
            {"KingMan V4", 280},
            {"KingMan V5", 281},
            {"MistMan V1", 282},
            {"MistMan V2", 283},
            {"MistMan V3", 284},
            {"MistMan V4", 285},
            {"MistMan V5", 286},
            {"BowlMan V1", 287},
            {"BowlMan V2", 288},
            {"BowlMan V3", 289},
            {"BowlMan V4", 290},
            {"BowlMan V5", 291},
            {"DarkMan V1", 292},
            {"DarkMan V2", 293},
            {"DarkMan V3", 294},
            {"DarkMan V4", 295},
            {"DarkMan V5", 296},
            {"JapanMan V1", 297},
            {"JapanMan V2", 298},
            {"JapanMan V3", 299},
            {"JapanMan V4", 300},
            {"JapanMan V5", 301},
            {"DeltaRay", 302},
            {"FoldrBak", 303},
            {"NavRcycl", 304},
            {"AlphArmSigma", 305},
            {"Bass", 306},
            {"Serenade", 307},
            {"Balance", 308},
            {"DarkAura", 309},
            {"AlphArmOmega", 310},
            {"Bass+", 311},
            {"BassGS", 312}
        };

        public override List<Effect> Effects
        {
            get
            {
                List<Effect> effects = new List<Effect>();

                effects.Add(new Effect("Currencies", "currencies", ItemKind.Folder));
                effects.Add(new Effect("Overworld Effects", "overworld", ItemKind.Folder));
                effects.Add(new Effect("Battle Effects", "battle", ItemKind.Folder));

                effects.Add(new Effect("Gain Zenny", "gainzenny", "currencies"));
                effects.Add(new Effect("Lose Zenny", "losezenny", "currencies"));
                effects.Add(new Effect("Gain BugFrags", "gainbugfrags", "currencies"));
                effects.Add(new Effect("Lose BugFrags", "losebugfrags", "currencies"));

                effects.Add(new Effect("(Overworld) Shuffle Folders", "shufflefolders", "overworld"));
                effects.Add(new Effect("(Overworld) Force Random Virus Encounter", "forcebattle", "overworld"));
                effects.Add(new Effect("(Overworld) Randomize Active Style", "randomizeactivestyle", "overworld"));
                effects.Add(new Effect("(Overworld) Randomize Stored Style", "randomizestoredstyle", "overworld"));
                effects.Add(new Effect("(Overworld) Add Random Chip to Pack", "addrandomtopack", "overworld"));

                effects.Add(new Effect("(Battle) Take Damage", "takedamage", "battle"));
                effects.Add(new Effect("(Battle) Heal Damage", "healdamage", "battle"));
                effects.Add(new Effect("(Battle) Empty Custom Meter", "emptycustommeter", "battle"));
                effects.Add(new Effect("(Battle) Fill Custom Meter", "fillcustommeter", "battle"));
                effects.Add(new Effect("(Battle) Empty Custom Screen (1 Minute)", "emptycustomscreen", "battle"));
                effects.Add(new Effect("(Battle) Fill Custom Screen (1 Minute)", "fillcustomscreen", "battle"));
                effects.Add(new Effect("(Battle) Shuffle Hand", "shufflehand", "battle"));
                effects.Add(new Effect("(Battle) Randomize Hand from Folders", "randomizehand", "battle"));
                effects.Add(new Effect("(Battle) Remove Last Chip from Hand", "removelastfromhand", "battle"));
                effects.Add(new Effect("(Battle) Empty Hand", "removeallfromhand", "battle"));
                effects.Add(new Effect("(Battle) Slot-In Random Chip from Folders", "randomaddtohand", "battle"));

                foreach (string style in StyleChanges.Keys)
                {
                    effects.Add(new Effect(style, style.ToLowerInvariant(), ItemKind.Usable, "stylechange"));
                }

                foreach (string chip in BattleChips.Keys)
                {
                    if (chip.Equals("BassGS") || chip.Equals("DeltaRay") || chip.Equals("FoldrBak") || chip.Equals("NavRcycl") || chip.Equals("AlphArmSigma") || chip.Equals("Bass")
                        || chip.Equals("Serenade") || chip.Equals("Balance") || chip.Equals("DarkAura") || chip.Equals("AlphArmOmega") || chip.Equals("Bass+")) continue;

                    effects.Add(new Effect(chip, chip.ToLowerInvariant(), ItemKind.Usable, "backpack"));
                    if (!chip.Equals("Panic")) effects.Add(new Effect(chip, chip.ToLowerInvariant(), ItemKind.Usable, "slotin"));
                }

                foreach (string pa in ProgramAdvances.Keys)
                {
                    effects.Add(new Effect(pa, pa.ToLowerInvariant(), ItemKind.Usable, "slotin"));
                }

                effects.Add(new Effect("(Overworld) Force Active Style Change", "setactivestyle", new[] { "stylechange" }, new object[] { "Normal Style" }, "overworld"));
                effects.Add(new Effect("(Overworld) Force Stored Style Change", "setstoredstyle", new[] { "stylechange" }, new object[] { "Normal Style" }, "overworld"));
                effects.Add(new Effect("(Overworld) Add Specific Battle Chip to Pack", "addchiptopack", new[] { "backpack" }, new object[] { "Cannon" }, "overworld"));
                effects.Add(new Effect("(Battle) Slot-In Specific Battle Chip", "addchiptohand", new[] { "slotin" }, new object[] { "Cannon" }, "battle"));

                return effects;
            }
        }

        public override List<ItemType> ItemTypes => new(new[]
        {
            new ItemType("Style Change", "stylechange", ItemType.Subtype.ItemList),
            new ItemType("Backpack Chip", "backpack", ItemType.Subtype.ItemList),
            new ItemType("Slot-In Chip", "slotin", ItemType.Subtype.ItemList),
        });

        public override List<(string, Action)> MenuActions
        {
            get
            {
                List<(string, Action)> result = new List<(string, Action)> { ("Fill Library", (FillLibrary)) };

                return result;
            }
        }

        public override List<ROMInfo> ROMTable => new(new[]
        {
            new ROMInfo("Mega Man Battle Network 3 White", ROMStatus.ValidPatched,s => Patching.MD5(s, "68817204a691449e655cba739dbb0165")),
            new ROMInfo("Mega Man Battle Network 3 Blue", ROMStatus.ValidPatched,s => Patching.MD5(s, "6fe31df0144759b34ad666badaacc442"))
        });

        public override Game Game { get; } = new(14, "Mega Man Battle Network 3", "MegaManBattleNetwork3", "GBA", ConnectorType.GBAConnector);

        protected override bool IsReady(EffectRequest request)
        {
            return false;
        }

        protected override void RequestData(DataRequest request)
        {
            return;
        }

        private bool IsInBattle() => Connector.IsEqual8(OffsetInBattleCheck, 0x08);

        private bool IsGamePaused() => Connector.IsEqual8(OffsetIsGamePaused, 0x01);

        // Used to clear old chip data from Mega Man during battle.
        private void ClearUsedChips(int start, int end)
        {
            Connector.Write8(OffsetHandChipsRemaining, 0);
            Connector.Write8(OffsetHandChipsCount, 5);

            // Save unused Chip Information
            ushort[,] chipdata = new ushort[5, 4];
            for (int i = 0; i < 5; i++)
            {
                if (Connector.Read16(OffsetHandChipsAttackType + (uint)(i * 2), out ushort b))
                {
                    if (b > 0 && b < 0xFFFF)
                    {
                        Connector.Read16(OffsetHandChipsAttackType + (uint)(i * 2), out chipdata[i, 0]);
                        Connector.Read16(OffsetHandChipsReadLoc + (uint)(i * 2), out chipdata[i, 1]);
                        Connector.Read16(OffsetHandChipsDamage + (uint)(i * 2), out chipdata[i, 2]);
                        Connector.Read16(OffsetHandChipsDamageBonus + (uint)(i * 2), out chipdata[i, 3]);
                    }
                }
            }

            // Rewrite old Chip Information
            for (int i = 0; i < 5; i++)
            {
                if (i >= start && i <= end && chipdata[i, 0] > 0 && chipdata[i, 0] < 0xFFFF)
                {
                    Connector.Write16(OffsetHandChipsAttackType + (uint)(i * 2), chipdata[i, 0]);
                    Connector.Write16(OffsetHandChipsReadLoc + (uint)(i * 2), chipdata[i, 1]);
                    Connector.Write16(OffsetHandChipsDamage + (uint)(i * 2), chipdata[i, 2]);
                    Connector.Write16(OffsetHandChipsDamageBonus + (uint)(i * 2), chipdata[i, 3]);
                }
                else
                {
                    Connector.Write16(OffsetHandChipsAttackType + (uint)(i * 2), 0xFFFF);
                    Connector.Write16(OffsetHandChipsReadLoc + (uint)(i * 2), 0);
                    Connector.Write16(OffsetHandChipsDamage + (uint)(i * 2), 0);
                    Connector.Write16(OffsetHandChipsDamageBonus + (uint)(i * 2), 0);
                }
            }
            return;
        }

        // Used to sort the chips above Mega Man's Head in battles.
        private void SortHandChips(bool FindFirst = false)
        {
            uint earliest = 5;
            for (int checks = 0; checks < 4; checks++)
            {
                for (uint i = 0; i <= 4; i++)
                {
                    // Reorganize Chip Information
                    if (Connector.Read16(OffsetHandChipsAttackType + (i * 2), out ushort b))
                    {
                        if (b == 0xFFFF)
                        {
                            if (checks <= 3 && i <= earliest) { earliest = i; }
                            if (i == 4) { break; }

                            Connector.Read16(OffsetHandChipsAttackType + (i * 2) + 2, out ushort b2);
                            Connector.Write16(OffsetHandChipsAttackType + (i * 2), b2);
                            Connector.Write16(OffsetHandChipsAttackType + (i * 2) + 2, 0xFFFF);

                            Connector.Read16(OffsetHandChipsReadLoc + (i * 2) + 2, out b2);
                            Connector.Write16(OffsetHandChipsReadLoc + (i * 2), b2);
                            Connector.Write16(OffsetHandChipsReadLoc + (i * 2) + 2, 0x0000);

                            Connector.Read16(OffsetHandChipsDamage + (i * 2) + 2, out b2);
                            Connector.Write16(OffsetHandChipsDamage + (i * 2), b2);
                            Connector.Write16(OffsetHandChipsDamage + (i * 2) + 2, 0x0000);

                            Connector.Read16(OffsetHandChipsDamageBonus + (i * 2) + 2, out b2);
                            Connector.Write16(OffsetHandChipsDamageBonus + (i * 2), b2);
                            Connector.Write16(OffsetHandChipsDamageBonus + (i * 2) + 2, 0x0000);
                        }
                    }
                }
            }

            // Overwrite Chip Sprites (Pretty slow and doesn't work, not sure why)
            /*
			for (uint i = 0; i < 5; i++)
			{
				if (Connector.Read16(OffsetHandChipsAttackType + (i * 2), out ushort check))
				{
					if (check == 0xFFFF) {continue;}
				}
				Connector.Read16(OffsetHandChipsAttackType + (i * 2), out ushort readloc);
				Connector.Read32(OffsetROMBattleChipData + (uint)(readloc * 32) + 20, out uint spriteloc);
				for (uint j = 0; j < 0x80; j++)
				{
					Connector.Read8(spriteloc + j, out byte spritedata);
					Connector.Write8(OffsetHandChipsSprites + (i * 0x80) + j, spritedata);
				}
			}*/

            if (FindFirst)
            {
                Connector.Write8(OffsetHandChipsRemaining, (byte)(earliest));
                Connector.Write8(OffsetHandChipsCount, 0);
            }
            return;
        }

        // Looks at a Battle Chip from your Hand and determines whether or not it's Gaia Blade or Gaia Sword, then fuses chip damage.
        // Also checks if the ChipID is from a select few damage increasing chips.
        private bool IsDamagePlusChip(byte handNumber, ushort chipID)
        {
            if (!IsInBattle()) { return false; }

            // Gaia Blade Check first
            Connector.Read16(OffsetHandChipsAttackType + (uint)(handNumber * 2), out ushort handChipID);
            if (BattleChips["GaiaSwrd"] == handChipID || BattleChips["GaiaBlad"] == handChipID)
            {
                Connector.Read16(OffsetROMBattleChipData + (uint)(chipID * 32) + 0xC, out ushort addValue);
                if (addValue > 0 && addValue < 9999)
                {
                    // Rewrite Damage Bonus Value
                    Connector.Read16(OffsetHandChipsDamageBonus + (uint)(handNumber * 2), out ushort curDamage);
                    Connector.Write16(OffsetHandChipsDamageBonus + (uint)(handNumber * 2), (ushort)(curDamage + addValue > 9999 ? 9999 : curDamage + addValue));
                    return true;
                }
            }

            // Damage Plus Chips Check Next
            // Make sure the chip you're giving damage to already has more than zero damage.
            Connector.Read16(OffsetROMBattleChipData + (uint)(handChipID * 32) + 0xC, out ushort dmgValue);
            if (dmgValue > 0 && dmgValue < 9999)
            {
                // Grab Damage Bonus Value
                Connector.Read16(OffsetHandChipsDamageBonus + (uint)(handNumber * 2), out ushort curDamage);

                // Add Damage Bonus Values to Array
                ushort[] powerups = { 0, 0, 0, 0, 0, 0 };
                switch (chipID)
                {
                    case 195:
                        {
                            powerups[0] += 10;
                            break;
                        }
                    case 205:
                        {
                            powerups[0] += 30;
                            break;
                        }
                    case 200:
                        {
                            powerups[5] += 20;
                            break;
                        }
                    case 206:
                        {
                            powerups[5] += 40;
                            break;
                        }
                    case 199:
                        {
                            powerups[1] += 30;
                            break;
                        }
                    case 196:
                        {
                            powerups[2] += 30;
                            break;
                        }
                    case 197:
                        {
                            powerups[3] += 30;
                            break;
                        }
                    case 198:
                        {
                            powerups[4] += 30;
                            break;
                        }
                    default: { break; }
                }

                // Make sure something is increasing damage
                bool check = false;
                for (int i = 0; i < 6; i++)
                {
                    if (powerups[i] > 0)
                    {
                        check = true;
                        break;
                    }
                }
                if (!check) { return false; }

                // Add non-specific Damage Bonuses
                ushort newDamage = (ushort)(curDamage + powerups[0] > 9999 ? 9999 : curDamage + powerups[0]);

                // Check for Navi Chip and add those bonuses
                if ((handChipID >= 219 && handChipID <= 272) || (handChipID >= 277 && handChipID <= 301))
                {
                    newDamage = (ushort)(newDamage + powerups[5] > 9999 ? 9999 : newDamage + powerups[5]);
                }

                // Find Battle Chip's Element and add those bonuses
                Connector.Read8(OffsetROMBattleChipData + (uint)(handChipID * 32) + 0x6, out byte elemValue);
                if (elemValue > 0)
                {
                    newDamage = (ushort)(newDamage + powerups[elemValue] > 9999 ? 9999 : newDamage + powerups[elemValue]);
                }

                // Set final damage bonus value.
                if (newDamage > curDamage)
                {
                    Connector.Write16(OffsetHandChipsDamageBonus + (uint)(handNumber * 2), newDamage);
                    return true;
                }
            }

            return false;
        }

        // Used to fill the game's Battle Chip Library completely so that chips don't appear as "????" and fail to activate, among other things
        private void FillLibrary()
        {
            // Version-specific Anti-Cheat Removal and Library Filling
            Connector.Read8(0x080000AA, out byte check);
            if (check == 0x42) // Version Blue
            {
                Connector.Write16(0x020019B0, 0xD6A3);
                Connector.Write8(0x02000330, 0x7F);
                Connector.Write8(0x02000356, 0x07);
                Connector.Write8(0x02000357, 0x00);
                for (uint i = 0x331; i <= 0x355; i++)
                {
                    Connector.Write8(0x02000000 + i, 0xFF);
                }
                for (uint i = 0x358; i <= 0x35B; i++)
                {
                    Connector.Write8(0x02000000 + i, 0xFF);
                }
            }
            else // Version White
            {
                Connector.Write16(0x020019B0, 0xD22E);
                Connector.Write8(0x02000330, 0x7F);
                Connector.Write8(0x02000357, 0x80);
                for (uint i = 0x331; i <= 0x356; i++)
                {
                    Connector.Write8(0x02000000 + i, 0xFF);
                }
                for (uint i = 0x358; i <= 0x35D; i++)
                {
                    Connector.Write8(0x02000000 + i, 0xFF);
                }
            }

            Connector.SendMessage("Filled out your Library.");
            return;
        }

        protected override void StartEffect(EffectRequest request)
        {
            string[] codeParams = request.FinalCode.Split('_');
            switch (codeParams[0])
            {
                case "gainzenny":
                    {
                        Connector.Read32(OffsetZenny, out uint inc);
                        if (inc >= 999999)
                        {
                            Respond(request, EffectStatus.FailTemporary, "The streamer has max Zenny.");
                            return;
                        }

                        uint v = (uint)(RNG.Next(1, 20) * 50);
                        inc += v;
                        bool result = Connector.Write32(OffsetZenny, (uint)(inc > 999999 ? 999999 : inc));

                        if (result)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} gave {v} Zenny!");
                            Respond(request, result ? EffectStatus.Success : EffectStatus.FailTemporary, string.Empty);
                        }
                        return;
                    }

                case "losezenny":
                    {
                        Connector.Read32(OffsetZenny, out uint dec);
                        if (dec <= 0)
                        {
                            Respond(request, EffectStatus.FailTemporary, "The streamer has no Zenny.");
                            return;
                        }

                        uint v = (uint)(RNG.Next(1, 20) * 50);
                        dec = (uint)((int)(dec - v) < 0 ? 0 : dec - v);
                        bool result = Connector.Write32(OffsetZenny, (uint)(dec < 0 ? 0 : dec));

                        if (result)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} stole {v} Zenny!");
                            Respond(request, result ? EffectStatus.Success : EffectStatus.FailTemporary, string.Empty);
                        }
                        return;
                    }

                case "gainbugfrags":
                    {
                        Connector.Read16(OffsetFrags, out ushort inc);
                        if (inc >= 9999)
                        {
                            Respond(request, EffectStatus.FailTemporary, "The streamer has max BugFrags.");
                            return;
                        }

                        ushort v = (ushort)(RNG.Next(1, 10));
                        inc += v;
                        bool result = Connector.Write16(OffsetFrags, (ushort)(inc > 9999 ? 9999 : inc));

                        if (result)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} gave {v} Bugfrag(s)!");
                            Respond(request, result ? EffectStatus.Success : EffectStatus.FailTemporary, string.Empty);
                        }
                        return;
                    }

                case "losebugfrags":
                    {
                        Connector.Read16(OffsetFrags, out ushort dec);
                        if (dec <= 0)
                        {
                            Respond(request, EffectStatus.FailTemporary, "The streamer has no BugFrags.");
                            return;
                        }

                        ushort v = (ushort)(RNG.Next(1, 10));
                        dec = (ushort)((short)(dec - v) < 0 ? 0 : dec - v);
                        bool result = Connector.Write16(OffsetFrags, (ushort)(dec < 0 ? 0 : dec));

                        if (result)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} stole {v} BugFrag(s)!");
                            Respond(request, result ? EffectStatus.Success : EffectStatus.FailTemporary, string.Empty);
                        }
                        return;
                    }

                case "shufflefolders":
                    {
                        if (IsInBattle())
                        {
                            Connector.SendMessage("The player is currently in a battle.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        uint[] folders1 = new uint[30];
                        uint[] folders2 = new uint[30];

                        // Loop through and add all Chips for the two Main Folders into a couple lists.
                        for (uint i = 0; i < 30; i++)
                        {
                            Connector.Read32(OffsetFolders + (i * 4), out uint chip);
                            folders1[(int)i] = chip;
                            Connector.Read32(OffsetFolders + ((i + 60) * 4), out chip);
                            folders2[(int)i] = chip;
                        }

                        // Shuffle both folders
                        folders1.Shuffle();
                        folders2.Shuffle();

                        // Rewrite data through a loop
                        bool IsSuccessful = false;
                        for (uint i = 0; i < 30; i++)
                        {
                            bool check1 = Connector.Write32(OffsetFolders + (i * 4), folders1[(int)i]);
                            bool check2 = Connector.Write32(OffsetFolders + ((i + 60) * 4), folders2[(int)i]);
                            if (check1 || check2) IsSuccessful = true;
                        }
                        if (IsSuccessful)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} shuffled your Folder(s)!");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }
                        return;
                    }

                case "randomizeactivestyle":
                    {
                        if (IsInBattle())
                        {
                            Connector.SendMessage("The player is currently in a battle.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        int val = RNG.Next(0, StyleChanges.Count - 1);
                        byte styleChange = 0x0;
                        string styleName = "";
                        foreach (KeyValuePair<string, byte> pair in StyleChanges)
                        {
                            if (val == 0)
                            {
                                styleChange = pair.Value;
                                styleName = pair.Key;
                                break;
                            }
                            val--;
                        }

                        bool IsSuccessful = Connector.Write8(OffsetStyleActive, styleChange);
                        if (IsSuccessful)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} changed your active Style to {styleName}!");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }
                        return;
                    }

                case "randomizestoredstyle":
                    {
                        if (IsInBattle())
                        {
                            Connector.SendMessage("The player is currently in a battle.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        int val = RNG.Next(1, StyleChanges.Count - 1);
                        byte styleChange = 0x0;
                        string styleName = "";
                        foreach (KeyValuePair<string, byte> pair in StyleChanges)
                        {
                            if (val == 0)
                            {
                                styleChange = pair.Value;
                                styleName = pair.Key;
                                break;
                            }
                            val--;
                        }

                        bool IsSuccessful = Connector.Write8(OffsetStyleStored, styleChange);
                        if (IsSuccessful)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} changed your stored Style to {styleName}!");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }
                        return;
                    }

                case "forcebattle":
                    TryEffect(request, IsInBattle, () => Connector.Write16(OffsetStepCounter, 0xFFFF),
                        () => { Connector.SendMessage($"{request.DisplayViewer} sent viruses!"); });
                    return;
                case "addrandomtopack":
                    {
                        if (IsInBattle())
                        {
                            Connector.SendMessage("The player is currently in a battle.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        // Find a Battle Chip that exists and you don't have literally every variant at maximum quantity.
                        ushort chipID = 0;
                        byte i = 0;
                        do
                        {
                            chipID = (ushort)RNG.Next(1, BattleChips.Count);
                            for (uint j = 0; j < 5; j++)
                            {
                                Connector.Read8(OffsetBackpack + (uint)((chipID * 18) + j), out byte quantity);
                                if (quantity < 99)
                                {
                                    i = 100;
                                    break;
                                }
                            }
                            i++;
                        } while (i < 100);
                        string chipName = "";
                        foreach (KeyValuePair<string, ushort> pair in BattleChips)
                        {
                            if (chipID == pair.Value)
                            {
                                chipName = pair.Key;
                                break;
                            }
                        }

                        // Find a code that exists.
                        byte codeVal = 0xFF;
                        uint chipCodeIndex = 0;
                        do
                        {
                            chipCodeIndex = (uint)RNG.Next(0, 5);
                            Connector.Read8(OffsetROMBattleChipData + ((uint)chipID * 32) + chipCodeIndex, out codeVal);
                        } while (codeVal == 0xFF);

                        if (codeVal == 0xFF)
                        {
                            Connector.SendMessage($"Failed to find available Battle Chip Code.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                        }

                        // save the Name of the Code.
                        string codeName = "";
                        foreach (KeyValuePair<string, byte> pair in ChipCodes)
                        {
                            if (codeVal == pair.Value)
                            {
                                codeName = pair.Key;
                                break;
                            }
                        }

                        // Add the Chip with that specific Code.
                        uint final = OffsetBackpack + (uint)((chipID * 18) + chipCodeIndex);
                        Connector.Read8(final, out byte chipcount);
                        chipcount++;
                        bool IsSuccessful = Connector.Write8(final, (byte)(chipcount > 99 ? 99 : chipcount));
                        if (IsSuccessful && chipcount <= 99)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} gave a copy of {chipName} {codeName}!");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }
                        else
                        {
                            Connector.SendMessage($"Failed to give a copy of {chipName} {codeName}.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                        }
                        return;
                    }

                case "takedamage":
                    {
                        if (!IsInBattle() || IsGamePaused())
                        {
                            Connector.SendMessage("The player isn't in battle, or their game is paused.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        Connector.Read16(OffsetCurrentHP, out ushort dec);
                        if (dec <= 0)
                        {
                            Respond(request, EffectStatus.FailTemporary, "The streamer has no Health.");
                            return;
                        }

                        ushort v = (ushort)(RNG.Next(1, 5) * 5);
                        dec = (ushort)((short)(dec - v) < 0 ? 0 : dec - v);
                        bool result = Connector.Write16(OffsetCurrentHP, (ushort)(dec < 0 ? 0 : dec));

                        if (result)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} dealt {v} damage!");
                            Respond(request, result ? EffectStatus.Success : EffectStatus.FailTemporary, string.Empty);
                        }
                        return;
                    }

                case "healdamage":
                    {
                        if (!IsInBattle() || IsGamePaused())
                        {
                            Connector.SendMessage("The player isn't in battle, or their game is paused.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        Connector.Read16(OffsetCurrentHP, out ushort inc);
                        Connector.Read16(OffsetCurrentHP + 2, out ushort maxhp);

                        if (inc >= maxhp)
                        {
                            Respond(request, EffectStatus.FailTemporary, "The streamer has max Health.");
                            return;
                        }

                        ushort v = (ushort)(RNG.Next(1, 5) * 5);
                        inc += v;
                        bool result = Connector.Write16(OffsetCurrentHP, (ushort)(inc > maxhp ? maxhp : inc));

                        if (result)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} healed {v} HP!");
                            Respond(request, result ? EffectStatus.Success : EffectStatus.FailTemporary, string.Empty);
                        }
                        return;
                    }

                case "shufflehand":
                    {
                        if (!IsInBattle() || IsGamePaused())
                        {
                            Connector.SendMessage("The player isn't in battle, or their game is paused.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        ushort[,] hand = new ushort[5, 4];
                        Connector.Read8(OffsetHandChipsCount, out byte count);
                        uint handcount = (uint)(count / 2);

                        Connector.Read8(OffsetHandChipsRemaining, out byte c);

                        if (c < 2)
                        {
                            Connector.SendMessage("Not enough Battle Chips in the player's Hand.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        // Loop through and add all Chips from your hand to lists.
                        for (uint i = handcount; i < 5; i++)
                        {
                            Connector.Read16(OffsetHandChipsAttackType + (i * 2), out hand[(int)i, 0]);
                            Connector.Read16(OffsetHandChipsReadLoc + (i * 2), out hand[(int)i, 1]);
                            Connector.Read16(OffsetHandChipsDamage + (i * 2), out hand[(int)i, 2]);
                            Connector.Read16(OffsetHandChipsDamageBonus + (i * 2), out hand[(int)i, 3]);
                        }

                        // Shuffle lists
                        int n = hand.GetLength(0);
                        while (n > 1)
                        {
                            n--;
                            int k = RNG.Next((int)handcount, n + 1);

                            ushort val;
                            for (int i = (int)handcount; i < 4; i++)
                            {
                                val = hand[k, i];
                                hand[k, i] = hand[n, i];
                                hand[n, i] = val;
                            }
                        }

                        // Rewrite data through a loop
                        bool IsSuccessful = false;
                        for (uint i = handcount; i < 5; i++)
                        {
                            bool check1 = Connector.Write16(OffsetHandChipsAttackType + (i * 2), hand[(int)i, 0]);
                            bool check2 = Connector.Write16(OffsetHandChipsReadLoc + (i * 2), hand[(int)i, 1]);
                            bool check3 = Connector.Write16(OffsetHandChipsDamage + (i * 2), hand[(int)i, 2]);
                            bool check4 = Connector.Write16(OffsetHandChipsDamageBonus + (i * 2), hand[(int)i, 3]);
                            if (check1 || check2 || check3 || check4) IsSuccessful = true;
                        }

                        SortHandChips(false);

                        if (IsSuccessful)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} shuffled your Hand!");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }
                        return;
                    }

                case "randomizehand":
                    {
                        if (!IsInBattle() || IsGamePaused())
                        {
                            Connector.SendMessage("The player isn't in battle, or their game is paused.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        Connector.SendMessage("Preparing Hand, please wait...");
                        ushort[] folders = new ushort[60];
                        ushort[] chips = { 0, 0, 0, 0, 0 };

                        // Loop through and add all Chips for the two Main Folders into a couple lists.
                        for (uint i = 0; i < 30; i++)
                        {
                            Connector.Read16(OffsetFolders + (i * 4), out ushort chip);
                            folders[(int)i] = chip;
                            Connector.Read16(OffsetFolders + ((i + 60) * 4), out chip);
                            folders[(int)(i + 30)] = chip;
                        }

                        // Find 5 chips in the folders
                        for (int c = 0; c < 5; c++)
                        {
                            // Shuffle both folders
                            folders.Shuffle();
                            for (int i = 0; i < 60; i++)
                            {
                                if (folders[i] == 0 || folders[i] == 0xFFFF) continue;
                                chips[c] = folders[i];
                                break;
                            }
                        }

                        // Shove those chips in your hand
                        bool IsSuccessful = false;
                        for (uint i = 0; i < 5; i++)
                        {
                            bool write1 = Connector.Write16(OffsetHandChipsAttackType + (i * 2), chips[(int)i]);
                            bool write2 = Connector.Write16(OffsetHandChipsReadLoc + (i * 2), chips[(int)i]);
                            Connector.Read16(OffsetROMBattleChipData + (ushort)(chips[(int)i] * 32) + 0xC, out ushort dmg);
                            bool write3 = Connector.Write16(OffsetHandChipsDamage + (i * 2), dmg);
                            bool write4 = Connector.Write16(OffsetHandChipsDamageBonus + (i * 2), 0x0);

                            if (write1 || write2 || write3 || write4) IsSuccessful = true;
                        }

                        Connector.Write8(OffsetHandChipsCount, 0);
                        Connector.Write8(OffsetHandChipsRemaining, 5);

                        if (IsSuccessful)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} randomized your Hand!");
                            Connector.SendMessage($"Chips in Hand: 5");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }

                        return;
                    }

                case "randomaddtohand":
                    {
                        if (!IsInBattle() || IsGamePaused())
                        {
                            Connector.SendMessage("The player isn't in battle, or their game is paused.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        uint handcount = 0;
                        while (handcount < 5)
                        {
                            Connector.Read16(OffsetHandChipsReadLoc + (handcount * 2), out ushort check);
                            if (check == 0x0000) break;
                            handcount++;
                        }

                        Connector.Read8(OffsetHandChipsRemaining, out byte c);
                        if (handcount == 5 && c == 5)
                        {
                            Connector.SendMessage("The player's hand is full!");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        Connector.SendMessage("Preparing Hand, please wait...");
                        ClearUsedChips((int)(handcount - c), (int)c);
                        SortHandChips(true);

                        ushort[] folders = new ushort[60];

                        // Loop through and add all Chips for the two Main Folders into a couple lists.
                        for (uint i = 0; i < 30; i++)
                        {
                            Connector.Read16(OffsetFolders + (i * 4), out ushort chip);
                            folders[(int)i] = (ushort)(chip == 0 ? 0xFFFF : chip);
                            Connector.Read16(OffsetFolders + ((i + 60) * 4), out chip);
                            folders[(int)i + 30] = (ushort)(chip == 0 ? 0xFFFF : chip);
                        }

                        // Shuffle both folders
                        folders.Shuffle();
                        ushort ch = 0;
                        for (int i = 0; i < 60; i++)
                        {
                            if (folders[i] == 0 || folders[i] == 0xFFFF) continue;
                            ch = folders[i];
                            break;
                        }

                        // Grab the first chip and shove it in your hand.
                        // Check the Chip
                        bool IsSuccessful = false;
                        if (IsDamagePlusChip((byte)(c - 1 < 0 ? 0 : c - 1), ch))
                        {
                            IsSuccessful = true;
                        }
                        else
                        {
                            bool write1 = Connector.Write16(OffsetHandChipsAttackType + (uint)(c * 2), ch);
                            bool write2 = Connector.Write16(OffsetHandChipsReadLoc + (uint)(c * 2), ch);
                            Connector.Read16(OffsetROMBattleChipData + (uint)(ch * 32) + 0xC, out ushort dmg);
                            bool write3 = Connector.Write16(OffsetHandChipsDamage + (uint)(c * 2), dmg);
                            bool write4 = Connector.Write16(OffsetHandChipsDamageBonus + (uint)(c * 2), 0x0);

                            Connector.Write8(OffsetHandChipsRemaining, (byte)(++c));
                            Connector.Write8(OffsetHandChipsCount, 0);

                            if (write1 || write2 || write3 || write4) IsSuccessful = true;
                        }

                        if (IsSuccessful)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} slotted-in a random Battle Chip!");
                            Connector.SendMessage($"Chips in Hand: {c}");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }

                        return;
                    }

                case "removelastfromhand":
                    {
                        if (!IsInBattle() || IsGamePaused())
                        {
                            Connector.SendMessage("The player isn't in battle, or their game is paused.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        uint handcount = 0;
                        while (handcount < 5)
                        {
                            Connector.Read16(OffsetHandChipsAttackType + (handcount * 2), out ushort check);
                            if (check == 0xFFFF) break;
                            handcount++;
                        }

                        Connector.SendMessage("Preparing Hand, please wait...");
                        Connector.Read8(OffsetHandChipsRemaining, out byte c);
                        ClearUsedChips((int)(handcount - c), (int)c);
                        SortHandChips(true);
                        Connector.Read8(OffsetHandChipsRemaining, out c);

                        if (c <= 0)
                        {
                            Connector.SendMessage("The player's hand is completely empty!");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        c = (byte)(c - 1 <= 0 ? 0 : c - 1);

                        // Grab the last chip from your hand and remove it.
                        bool IsSuccessful = false;
                        bool write1 = Connector.Write16(OffsetHandChipsAttackType + (uint)((c) * 2), 0xFFFF);
                        bool write2 = Connector.Write16(OffsetHandChipsReadLoc + (uint)((c) * 2), 0x0);
                        bool write3 = Connector.Write16(OffsetHandChipsDamage + (uint)((c) * 2), 0x0);
                        bool write4 = Connector.Write16(OffsetHandChipsDamageBonus + (uint)((c) * 2), 0x0);

                        if (write1 || write2 || write3 || write4) IsSuccessful = true;

                        if (IsSuccessful)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} removed a chip from your hand!");
                            Connector.SendMessage($"Chips in Hand: {c}");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }


                        return;
                    }

                case "removeallfromhand":
                    {
                        if (!IsInBattle() || IsGamePaused())
                        {
                            Connector.SendMessage("The player isn't in battle, or their game is paused.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        Connector.Read8(OffsetHandChipsRemaining, out byte c);
                        if (c == 0)
                        {
                            Connector.SendMessage("The player's hand is already empty!");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        // Remove all chips from your Hand.
                        bool IsSuccessful = false;
                        for (uint i = 0; i < 5; i++)
                        {
                            bool write1 = Connector.Write16(OffsetHandChipsAttackType + (i * 2), 0xFFFF);
                            bool write2 = Connector.Write16(OffsetHandChipsReadLoc + (i * 2), 0x0);
                            bool write3 = Connector.Write16(OffsetHandChipsDamage + (i * 2), 0x0);
                            bool write4 = Connector.Write16(OffsetHandChipsDamageBonus + (i * 2), 0x0);

                            if (write1 || write2 || write3 || write4) IsSuccessful = true;
                        }

                        Connector.Write8(OffsetHandChipsCount, 10);
                        Connector.Write8(OffsetHandChipsRemaining, 0);

                        if (IsSuccessful)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} emptied your hand!");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }
                        return;
                    }
                case "emptycustommeter":
                    TryEffect(request,
                        () => (IsInBattle() && (!IsGamePaused())),
                        () => Connector.Write16(OffsetCustomMeter, 0x0000) && Connector.Write8(0x02006CAC, 0x00),
                        () => { Connector.SendMessage($"{request.DisplayViewer} emptied your Custom Gauge!"); });
                    return;
                case "fillcustommeter":
                    TryEffect(request,
                        () => (IsInBattle() && (!IsGamePaused())),
                        () => Connector.Write16(OffsetCustomMeter, 0x4000),
                        () => { Connector.SendMessage($"{request.DisplayViewer} filled your Custom Gauge!"); });
                    return;
                case "emptycustomscreen":
                    {
                        var s = RepeatAction(request, TimeSpan.FromMinutes(1),
                            () => IsInBattle() && Connector.Read8(OffsetStoryProgress, out byte p) && (p > 0),
                            () => Connector.SendMessage("Custom Screen has been completely emptied!"), TimeSpan.FromSeconds(1),
                            () => true, TimeSpan.FromSeconds(1),
                            () => Connector.Write8(OffsetCustomChipCount, 0), TimeSpan.FromSeconds(1), true, "customscreen");
                        s.WhenCompleted.Then(_ => { Connector.SendMessage("Custom Screen is back to normal."); });
                        return;
                    }

                case "fillcustomscreen":
                    {
                        var s = RepeatAction(request, TimeSpan.FromMinutes(1),
                            () => IsInBattle() && Connector.Read8(OffsetStoryProgress, out byte p) && (p > 0),
                            () => Connector.SendMessage("Custom Screen has been completely filled!"), TimeSpan.FromSeconds(1),
                            () => true, TimeSpan.FromSeconds(1),
                            () => Connector.Write8(OffsetCustomChipCount, 10), TimeSpan.FromSeconds(1), true, "customscreen");
                        s.WhenCompleted.Then(_ => { Connector.SendMessage("Custom Screen is back to normal."); });
                        return;
                    }
                case "setactivestyle":
                    {
                        if (IsInBattle())
                        {
                            Connector.SendMessage("The player is currently in a battle.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        string styleName = codeParams[1];
                        byte styleChange = 0xFF;
                        foreach (KeyValuePair<string, byte> pair in StyleChanges)
                        {
                            if (pair.Key.ToLowerInvariant().Equals(codeParams[1]))
                            {
                                styleChange = pair.Value;
                                break;
                            }
                        }
                        if (styleChange == 0xFF)
                        {
                            Connector.SendMessage($"ERROR: Style not found.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        bool IsSuccessful = Connector.Write8(OffsetStyleActive, styleChange);
                        if (IsSuccessful)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} changed your active Style to {styleName}!");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }
                        return;
                    }

                case "setstoredstyle":
                    {
                        if (IsInBattle())
                        {
                            Connector.SendMessage("The player is currently in a battle.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        string styleName = codeParams[1];
                        byte styleChange = 0xFF;
                        foreach (KeyValuePair<string, byte> pair in StyleChanges)
                        {
                            if (pair.Key.ToLowerInvariant().Equals(codeParams[1]))
                            {
                                styleChange = pair.Value;
                                break;
                            }
                        }
                        if (styleChange == 0xFF)
                        {
                            Connector.SendMessage($"ERROR: Style not found.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        bool IsSuccessful = Connector.Write8(OffsetStyleStored, styleChange);
                        if (IsSuccessful)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} changed your stored Style to {styleName}!");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }
                        return;
                    }

                case "addchiptopack":
                    {
                        if (IsInBattle())
                        {
                            Connector.SendMessage("The player is currently in a battle.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        // Specific Battle Chip Choice
                        string chipName = codeParams[1];
                        ushort chipID = 0;
                        foreach (KeyValuePair<string, ushort> pair in BattleChips)
                        {
                            if (pair.Key.ToLowerInvariant().Equals(codeParams[1]))
                            {
                                chipID = pair.Value;
                                break;
                            }
                        }

                        // Make Sure the Chip actually exists
                        if (chipID == 0)
                        {
                            Connector.SendMessage($"ERROR: Battle Chip not found.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        // Find a code that exists.
                        byte codeVal = 0xFF;
                        uint chipCodeIndex = 0;
                        do
                        {
                            chipCodeIndex = (uint)RNG.Next(0, 5);
                            Connector.Read8(OffsetROMBattleChipData + ((uint)chipID * 32) + chipCodeIndex, out codeVal);
                        } while (codeVal == 0xFF);

                        if (codeVal == 0xFF)
                        {
                            Connector.SendMessage($"Failed to find available Battle Chip Code.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                        }

                        // Save the Name of the Code.
                        string codeName = "";
                        foreach (KeyValuePair<string, byte> pair in ChipCodes)
                        {
                            if (codeVal == pair.Value)
                            {
                                codeName = pair.Key;
                                break;
                            }
                        }

                        uint final = OffsetBackpack + (uint)(((uint)chipID * 18) + chipCodeIndex);
                        Connector.Read8(final, out byte chipcount);
                        chipcount++;
                        bool IsSuccessful = Connector.Write8(final, (byte)(chipcount > 99 ? 99 : chipcount));
                        if (IsSuccessful && chipcount <= 99)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} gave a copy of {chipName} {codeName}!");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }
                        else
                        {
                            Connector.SendMessage($"Failed to give a copy of {chipName} {codeName}.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                        }
                        return;
                    }

                case "addchiptohand":
                    {
                        if (!IsInBattle() || IsGamePaused())
                        {
                            Connector.SendMessage("The player isn't in battle, or their game is paused.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        uint handcount = 0;
                        while (handcount < 5)
                        {
                            Connector.Read16(OffsetHandChipsReadLoc + (handcount * 2), out ushort check);
                            if (check == 0x0000) break;
                            handcount++;
                        }

                        Connector.Read8(OffsetHandChipsRemaining, out byte c);
                        if (handcount == 5 && c == 5)
                        {
                            Connector.SendMessage("The player's hand is full!");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        Connector.SendMessage("Preparing Hand, please wait...");
                        ClearUsedChips((int)(handcount - c), (int)c);
                        SortHandChips(true);

                        ushort chipID = 0;
                        foreach (KeyValuePair<string, ushort> pair in ProgramAdvances)
                        {
                            if (pair.Key.ToLowerInvariant().Equals(codeParams[1]))
                            {
                                chipID = pair.Value;
                                break;
                            }
                        }
                        if (chipID == 0)
                        {
                            foreach (KeyValuePair<string, ushort> pair in BattleChips)
                            {
                                if (pair.Key.ToLowerInvariant().Equals(codeParams[1]))
                                {
                                    chipID = pair.Value;
                                    break;
                                }
                            }
                        }
                        if (chipID == 0)
                        {
                            Connector.SendMessage($"ERROR: Battle Chip not found.");
                            Respond(request, EffectStatus.FailTemporary, string.Empty);
                            return;
                        }

                        // Check the Chip
                        bool IsSuccessful = false;
                        if (IsDamagePlusChip((byte)(c - 1 < 0 ? 0 : c - 1), chipID))
                        {
                            IsSuccessful = true;
                        }
                        else
                        {
                            // Grab the chip and shove it in your hand.
                            bool write1 = Connector.Write16(OffsetHandChipsAttackType + (uint)(c * 2), chipID);
                            bool write2 = Connector.Write16(OffsetHandChipsReadLoc + (uint)(c * 2), chipID);
                            Connector.Read16(OffsetROMBattleChipData + ((uint)chipID * 32) + 0xC, out ushort dmg);
                            bool write3 = Connector.Write16(OffsetHandChipsDamage + (uint)(c * 2), dmg);
                            bool write4 = Connector.Write16(OffsetHandChipsDamageBonus + (uint)(c * 2), 0);

                            Connector.Write8(OffsetHandChipsRemaining, (byte)(++c));
                            Connector.Write8(OffsetHandChipsCount, 0);

                            if (write1 || write2 || write3 || write4) IsSuccessful = true;
                        }

                        if (IsSuccessful)
                        {
                            Connector.SendMessage($"{request.DisplayViewer} slotted-in {codeParams[1]}!");
                            Connector.SendMessage($"Chips in Hand: {c}");
                            Respond(request, EffectStatus.Success, string.Empty);
                        }

                        return;
                    }
            }
        }

        protected override bool StopEffect(EffectRequest request)
        {
            return true;
        }

        public override bool StopAllEffects()
        {
            return true;
        }

        private enum SFXType : ushort
        {
            None = 0x00
        }

        private void PlaySFX(SFXType type)
        {

        }
    }
}
