using Steamworks;

public class TestConstants {
	public readonly CSteamID k_SteamId_Group_SteamUniverse = new CSteamID(103582791434672565);
	public readonly CSteamID k_SteamId_rlabrecque = new CSteamID(76561197991230424);
	public readonly AppId_t k_AppId_TeamFortress2 = new AppId_t(440);
	public readonly AppId_t k_AppId_PieterwTestDLC = new AppId_t(110902);
	public readonly AppId_t k_AppId_FreeToPlay = new AppId_t(343450);
	public readonly PublishedFileId_t k_PublishedFileId_Champions = new PublishedFileId_t(280762427);
	public const uint k_IpAdress127_0_0_1 = 2130706433;
	public const uint k_IpAddress208_78_165_233 = 3494815209; // Valve Matchmaking Server (Virginia iad-3/srcds150 #51) 
	public const ushort k_Port27015 = 27015;

	private static TestConstants _instance;

	private TestConstants() { }

	public static TestConstants Instance {
		get {
			if (_instance == null) {
				_instance = new TestConstants();
			}
			return _instance;
		}
	}
}
