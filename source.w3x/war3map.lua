gg_rct_WarriorSelectRegion = nil
gg_rct_MageSelectRegion = nil
gg_rct_ArcherSelectRegion = nil
gg_rct_HeroSpawnRegion = nil
gg_rct_JobAdvancementRegion = nil
gg_rct_GameStartRegion = nil
function InitGlobals()
end

function CreateAllItems()
local itemID

BlzCreateItemWithSkin(FourCC("I000"), -2036.3, -3319.7, FourCC("I000"))
BlzCreateItemWithSkin(FourCC("I000"), -2141.4, -3332.2, FourCC("I000"))
BlzCreateItemWithSkin(FourCC("I001"), -1813.1, -3474.6, FourCC("I001"))
BlzCreateItemWithSkin(FourCC("I001"), -2127.4, -3490.3, FourCC("I001"))
BlzCreateItemWithSkin(FourCC("I001"), -1913.9, -3481.0, FourCC("I001"))
BlzCreateItemWithSkin(FourCC("I002"), -2011.7, -3484.5, FourCC("I002"))
end

function CreateNeutralHostile()
local p = Player(PLAYER_NEUTRAL_AGGRESSIVE)
local u
local unitID
local t
local life

u = BlzCreateUnitWithSkin(p, FourCC("n001"), -809.2, -3090.1, 296.717, FourCC("n001"))
u = BlzCreateUnitWithSkin(p, FourCC("n000"), -1193.7, -3026.1, 134.488, FourCC("n000"))
u = BlzCreateUnitWithSkin(p, FourCC("n000"), -1178.2, -3227.5, 150.682, FourCC("n000"))
u = BlzCreateUnitWithSkin(p, FourCC("n000"), -1174.4, -3383.9, 261.109, FourCC("n000"))
end

function CreateNeutralPassiveBuildings()
local p = Player(PLAYER_NEUTRAL_PASSIVE)
local u
local unitID
local t
local life

u = BlzCreateUnitWithSkin(p, FourCC("ncop"), -3200.0, -3200.0, 270.000, FourCC("ncop"))
u = BlzCreateUnitWithSkin(p, FourCC("ncop"), -2944.0, -3200.0, 270.000, FourCC("ncop"))
u = BlzCreateUnitWithSkin(p, FourCC("ncop"), -2688.0, -3200.0, 270.000, FourCC("ncop"))
u = BlzCreateUnitWithSkin(p, FourCC("ncop"), -2112.0, -2752.0, 270.000, FourCC("ncop"))
end

function CreateNeutralPassive()
local p = Player(PLAYER_NEUTRAL_PASSIVE)
local u
local unitID
local t
local life

u = BlzCreateUnitWithSkin(p, FourCC("H002"), -2931.5, -2957.5, 266.352, FourCC("H002"))
u = BlzCreateUnitWithSkin(p, FourCC("H001"), -3190.8, -2950.1, 267.198, FourCC("H001"))
u = BlzCreateUnitWithSkin(p, FourCC("H000"), -2678.7, -2959.3, 266.824, FourCC("H000"))
end

function CreatePlayerBuildings()
end

function CreatePlayerUnits()
end

function CreateAllUnits()
CreateNeutralPassiveBuildings()
CreatePlayerBuildings()
CreateNeutralHostile()
CreateNeutralPassive()
CreatePlayerUnits()
end

function CreateRegions()
local we

gg_rct_WarriorSelectRegion = Rect(-3264.0, -3264.0, -3136.0, -3136.0)
gg_rct_MageSelectRegion = Rect(-3008.0, -3264.0, -2880.0, -3136.0)
gg_rct_ArcherSelectRegion = Rect(-2752.0, -3264.0, -2624.0, -3136.0)
gg_rct_HeroSpawnRegion = Rect(-2176.0, -3456.0, -2048.0, -3328.0)
gg_rct_JobAdvancementRegion = Rect(-2176.0, -2816.0, -2048.0, -2688.0)
gg_rct_GameStartRegion = Rect(-3008.0, -3520.0, -2880.0, -3392.0)
end

function InitCustomPlayerSlots()
SetPlayerStartLocation(Player(0), 0)
SetPlayerColor(Player(0), ConvertPlayerColor(0))
SetPlayerRacePreference(Player(0), RACE_PREF_HUMAN)
SetPlayerRaceSelectable(Player(0), true)
SetPlayerController(Player(0), MAP_CONTROL_USER)
end

function InitCustomTeams()
SetPlayerTeam(Player(0), 0)
end

function main()
SetCameraBounds(-3328.0 + GetCameraMargin(CAMERA_MARGIN_LEFT), -3584.0 + GetCameraMargin(CAMERA_MARGIN_BOTTOM), 3328.0 - GetCameraMargin(CAMERA_MARGIN_RIGHT), 3072.0 - GetCameraMargin(CAMERA_MARGIN_TOP), -3328.0 + GetCameraMargin(CAMERA_MARGIN_LEFT), 3072.0 - GetCameraMargin(CAMERA_MARGIN_TOP), 3328.0 - GetCameraMargin(CAMERA_MARGIN_RIGHT), -3584.0 + GetCameraMargin(CAMERA_MARGIN_BOTTOM))
SetDayNightModels("Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl", "Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl")
NewSoundEnvironment("Default")
SetAmbientDaySound("LordaeronWinterDay")
SetAmbientNightSound("LordaeronWinterNight")
SetMapMusic("Music", true, 0)
CreateRegions()
CreateAllItems()
CreateAllUnits()
InitBlizzard()
InitGlobals()
end

function config()
SetMapName("TRIGSTR_001")
SetMapDescription("TRIGSTR_003")
SetPlayers(1)
SetTeams(1)
SetGamePlacement(MAP_PLACEMENT_USE_MAP_SETTINGS)
DefineStartLocation(0, -3264.0, -3520.0)
InitCustomPlayerSlots()
SetPlayerSlotAvailable(Player(0), MAP_CONTROL_USER)
InitGenericPlayerSlots()
end

