// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder					

namespace Common.Network
{
	public static class Op
	{
		// NA:      160200
		// KR test: 170100 (not fully supported yet)
		// EU:      140400 (not fully supported yet)
		public const uint Version = 160200;

		// Login Server															
		public readonly static uint ClientIdent = 0x0FD1020A;
		public readonly static uint ClientIdentR = 0x1F;
		public readonly static uint Login = 0x0FD12002;
		public readonly static uint LoginR = 0x23;
		public readonly static uint CharInfoRequest = 0x29;
		public readonly static uint CharInfo = 0x2A;
		public readonly static uint CreateCharacter = 0x2B;
		public readonly static uint CharacterCreated = 0x2C;
		public readonly static uint DeleteCharRequest = 0x2D;
		public readonly static uint DeleteCharRequestR = 0x2E;
		public readonly static uint EnterGame = 0x2F;
		public readonly static uint ChannelInfo = 0x30;
		public readonly static uint DeleteChar = 0x35;
		public readonly static uint DeleteCharR = 0x36;
		public readonly static uint RecoverChar = 0x37;
		public readonly static uint RecoverCharR = 0x38;
		public readonly static uint NameCheck = 0x39;
		public readonly static uint NameCheckR = 0x3A;
		public readonly static uint PetInfoRequest = 0x3B;
		public readonly static uint PetInfo = 0x3C;
		public readonly static uint CreatePet = 0x3D;
		public readonly static uint PetCreated = 0x3E;
		public readonly static uint DeletePetRequest = 0x3F;
		public readonly static uint DeletePetRequestR = 0x40;
		public readonly static uint DeletePet = 0x41;
		public readonly static uint DeletePetR = 0x42;
		public readonly static uint RecoverPet = 0x43;
		public readonly static uint RecoverPetR = 0x44;
		public readonly static uint CreatePartner = 0x45;
		public readonly static uint CreatePartnerR = 0x46;
		public readonly static uint AcceptGift = 0x47;
		public readonly static uint RefuseGift = 0x49;
		public readonly static uint Disconnect = 0x4D;
		public readonly static uint CreatingPet = 0x50;
		public readonly static uint CreatingPartner = 0x55;

		// World Server
		public readonly static uint LoginW = 0x4E22;
		public readonly static uint LoginWR = 0x4E23;
		public readonly static uint DisconnectW = 0x4E24;
		public readonly static uint DisconnectWR = 0x4E25;
		public readonly static uint Disappear = 0x4E2A;
		//public readonly static uint GoRebirth = 0x4E32;
		public readonly static uint GMCPOpen = 0x4EE9;
		public readonly static uint GMCPClose = 0x4EEA;
		public readonly static uint GMCPSummon = 0x4EEB;
		public readonly static uint GMCPMoveToChar = 0x4EEC;
		public readonly static uint GMCPMove = 0x4EED;
		public readonly static uint GMCPRevive = 0x4EEE;
		public readonly static uint GMCPInvisibility = 0x4EEF;
		public readonly static uint GMCPInvisibilityR = 0x4EF0;
		public readonly static uint GMCPExpel = 0x4EF6;
		public readonly static uint GMCPBan = 0x4EF7;

		public readonly static uint CharInfoRequestW = 0x5208;
		public readonly static uint CharInfoRequestWR = 0x5209;
		public readonly static uint EntityAppears = 0x520C;
		public readonly static uint EntityDisappears = 0x520D;
		public readonly static uint ItemAppears = 0x5211;
		public readonly static uint ItemDisappears = 0x5212;
		public readonly static uint Chat = 0x526C;
		public readonly static uint Notice = 0x526D;
		public readonly static uint MsgBox = 0x526F;
		public readonly static uint WhisperChat = 0x5273;
		public readonly static uint BeginnerChat = 0x5275;
		public readonly static uint EntitiesSpawn = 0x5334;
		public readonly static uint EntitiesDisappear = 0x5335;
		public readonly static uint BackFromTheDead1 = 0x53FD;
		public readonly static uint IsNowDead = 0x53FC;
		public readonly static uint Revive = 0x53FE;
		public readonly static uint Revived = 0x53FF;
		public readonly static uint DeadMenu = 0x5401;
		public readonly static uint DeadMenuR = 0x5402;
		public readonly static uint DeadFeather = 0x5403;
		public readonly static uint UseGesture = 0x540E;
		public readonly static uint UseGestureR = 0x540F;
		public readonly static uint NPCTalkStart = 0x55F0;
		public readonly static uint NPCTalkStartR = 0x55F1;
		public readonly static uint NPCTalkEnd = 0x55F2;
		public readonly static uint NPCTalkEndR = 0x55F3;
		public readonly static uint ItemMove = 0x59D8;
		public readonly static uint ItemMoveR = 0x59D9;
		public readonly static uint ItemPickUp = 0x59DA;
		public readonly static uint ItemPickUpR = 0x59DB;
		public readonly static uint ItemDrop = 0x59DC;
		public readonly static uint ItemDropR = 0x59DD;
		public readonly static uint ItemMoveInfo = 0x59DE;
		public readonly static uint ItemSwitchInfo = 0x59DF;
		public readonly static uint ItemNew = 0x59E0;
		public readonly static uint ItemRemove = 0x59E1;
		public readonly static uint ItemDestroy = 0x59E2;
		public readonly static uint ItemDestroyR = 0x59E4;
		public readonly static uint EquipmentChanged = 0x59E6;
		public readonly static uint EquipmentMoved = 0x59E7;
		public readonly static uint ItemSplit = 0x59E8;
		public readonly static uint ItemSplitR = 0x59E9;
		public readonly static uint ItemAmount = 0x59EA;
		public readonly static uint UsePotion = 0x59EB;
		public readonly static uint NPCTalkSelectEnd = 0x59F9;
		public readonly static uint SwitchSet = 0x5BCD;
		public readonly static uint SwitchSetR = 0x5BCE;
		public readonly static uint SwitchedSet = 0x5BCF;
		public readonly static uint ItemStateChange = 0x5BD0;
		public readonly static uint ItemStateChangeR = 0x5BD1;
		public readonly static uint ItemStateChanged = 0x5BD9;
		public readonly static uint NPCTalkKeyword = 0x5DC4;
		public readonly static uint NPCTalkKeywordR = 0x5DC5;

		public readonly static uint WARP_ENTER = 0x6594;
		public readonly static uint EnterRegionPermission = 0x6597;
		public readonly static uint EnterRegion = 0x6598;
		public readonly static uint WarpRegion = 0x6599;
		public readonly static uint EnterRegionR = 0x659C;
		public readonly static uint SkillInfo = 0x6979;
		public readonly static uint SkillPrepare = 0x6982;
		public readonly static uint SkillPrepared = 0x6983;
		public readonly static uint SkillUse = 0x6986;
		public readonly static uint SkillUsed = 0x6987;
		public readonly static uint SkillCancel = 0x6989;
		public readonly static uint SkillStart = 0x698A;
		public readonly static uint SkillStop = 0x698B;
		public readonly static uint SkillUnkown1 = 0x698D;
		public readonly static uint SkillStackSet = 0x6991;
		public readonly static uint SkillStackUpdate = 0x6992;
		public readonly static uint Motions = 0x6D62;
		public readonly static uint MotionCancel = 0x6D65;
		public readonly static uint LevelUp = 0x6D69;
		public readonly static uint Resting = 0x6D6C;
		public readonly static uint StandUp = 0x6D6D;
		public readonly static uint ChangeStance = 0x6E28;
		public readonly static uint ChangeStanceR = 0x6E29;
		public readonly static uint ChangesStance = 0x6E2A;

		public readonly static uint BackFromTheDead2 = 0x701D;
		public readonly static uint LoginWLock = 0x701E;
		public readonly static uint LoginWUnlock = 0x701F;
		public readonly static uint ShopBuyItem = 0x7150;
		public readonly static uint ShopBuyItemR = 0x7151;
		public readonly static uint ShopSellItem = 0x7152;
		public readonly static uint ShopSellItemR = 0x7153;
		public readonly static uint ShopOpen = 0x715E;
		public readonly static uint StatUpdatePrivate = 0x7530;
		public readonly static uint StatUpdatePublic = 0x7532;
		public readonly static uint CombatTargetSet = 0x791A;
		public readonly static uint CombatSetTarget = 0x7920;
		public readonly static uint CombatSetFinisher = 0x7921;
		public readonly static uint CombatSetFinisher2 = 0x7922;
		public readonly static uint CombatAction = 0x7924;
		public readonly static uint CombatActionEnd = 0x7925;
		public readonly static uint CombatActionBundle = 0x7926;
		public readonly static uint CombatAttackR = 0x7D01;

		public readonly static uint ChangeTitle = 0x8FC4;
		public readonly static uint ChangedTitle = 0x8FC5;
		public readonly static uint ChangeTitleR = 0x8FC6;
		public readonly static uint AreaChange = 0x88B8;

		public readonly static uint PetRegister = 0x9024;
		public readonly static uint PetUnRegister = 0x9025;
		public readonly static uint PetSummon = 0x902C;
		public readonly static uint PetSummonR = 0x902D;
		public readonly static uint PetUnsummon = 0x9031;
		public readonly static uint PetUnsummonR = 0x9032;
		public readonly static uint PortalUse = 0x908B;
		public readonly static uint PortalUseR = 0x908C;
		public readonly static uint Effect = 0x9090;

		public readonly static uint StatusEffectUpdate = 0xA028;
		public readonly static uint MoonGateRequest = 0xA428;
		public readonly static uint MailsRequest = 0xA898;
		public readonly static uint MailsRequestR = 0xA899;
		public readonly static uint SosButton = 0xA9A9;
		public readonly static uint SosButtonR = 0xA9AA;
		public readonly static uint SubsribeStun = 0xAA1C; // ?
		public readonly static uint StunMeter = 0xAA1D;
		public readonly static uint AfterLogin = 0xAA54;

		public readonly static uint NPCTalkSelectable = 0x13882;
		public readonly static uint NPCTalkSelect = 0x13883;

		public readonly static uint PetMount = 0x1FBD0;
		public readonly static uint PetMountR = 0x1FBD1;
		public readonly static uint PetUnmount = 0x1FBD2;
		public readonly static uint PetUnmountR = 0x1FBD3;
		public readonly static uint VehicleBond = 0x1FBD4;

		public readonly static uint Run = 0x0F213303;
		public readonly static uint Running = 0x0F44BBA3;
		public readonly static uint CombatAttack = 0x0FCC3231;
		public readonly static uint Walking = 0x0FD13021;
		public readonly static uint Walk = 0x0FF23431;

		static Op()
		{
			// EU
			if (Version == 140400)
			{
				ClientIdent = 0x1E;
				Login = 0x0F010000;
				Disconnect = 0x4B;
				Run = 0x0F010300;
				Walk = 0x0F010800;
				Running = 0x0F0400A0;
				CombatAttack = 0x0FB10001;
			}
		}
	}
}
