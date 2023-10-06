gg_rct_WarriorSelectRegion = nil
gg_rct_MageSelectRegion = nil
gg_rct_ArcherSelectRegion = nil
gg_rct_HeroSpawnRegion = nil
gg_rct_JobAdvancementRegion = nil
gg_rct_GameStartRegion = nil
gg_rct_Dungeon1Entrance = nil
gg_rct_Dungeon1Start = nil
gg_rct_Dungeon1Zone = nil
gg_rct_Dungeon1Area1 = nil
gg_rct_Dungeon1Gate = nil
gg_rct_Dungeon1Area2 = nil
gg_rct_Dungeon1Exit = nil
function InitGlobals()
end

function CreateAllItems()
local itemID

BlzCreateItemWithSkin(FourCC("I000"), -667.6, -461.9, FourCC("I000"))
BlzCreateItemWithSkin(FourCC("I000"), -562.4, -449.4, FourCC("I000"))
BlzCreateItemWithSkin(FourCC("I001"), -653.5, -620.0, FourCC("I001"))
BlzCreateItemWithSkin(FourCC("I001"), -440.0, -610.8, FourCC("I001"))
BlzCreateItemWithSkin(FourCC("I001"), -339.3, -604.4, FourCC("I001"))
BlzCreateItemWithSkin(FourCC("I002"), -537.9, -614.3, FourCC("I002"))
end

function CreateNeutralHostile()
local p = Player(PLAYER_NEUTRAL_AGGRESSIVE)
local u
local unitID
local t
local life

u = BlzCreateUnitWithSkin(p, FourCC("n001"), -1595.6, -25164.8, 268.711, FourCC("n001"))
u = BlzCreateUnitWithSkin(p, FourCC("n000"), -1924.5, -27956.8, 236.404, FourCC("n000"))
u = BlzCreateUnitWithSkin(p, FourCC("n000"), -1279.8, -27952.9, 306.847, FourCC("n000"))
u = BlzCreateUnitWithSkin(p, FourCC("n000"), -1585.7, -28145.2, 271.583, FourCC("n000"))
u = BlzCreateUnitWithSkin(p, FourCC("n006"), -1252.9, -3231.9, 324.022, FourCC("n006"))
u = BlzCreateUnitWithSkin(p, FourCC("n006"), -1230.9, -3572.5, 6.795, FourCC("n006"))
u = BlzCreateUnitWithSkin(p, FourCC("n006"), -2407.5, -3252.9, 323.147, FourCC("n006"))
u = BlzCreateUnitWithSkin(p, FourCC("n007"), -2733.4, -3352.9, 320.709, FourCC("n007"))
u = BlzCreateUnitWithSkin(p, FourCC("n009"), -3645.4, -4038.3, 338.654, FourCC("n009"))
u = BlzCreateUnitWithSkin(p, FourCC("n008"), -3321.1, -4111.2, 325.807, FourCC("n008"))
u = BlzCreateUnitWithSkin(p, FourCC("n008"), -3524.5, -4332.1, 321.007, FourCC("n008"))
u = BlzCreateUnitWithSkin(p, FourCC("n006"), -2454.1, -4571.5, 344.709, FourCC("n006"))
u = BlzCreateUnitWithSkin(p, FourCC("n006"), -2019.1, -4532.9, 174.555, FourCC("n006"))
u = BlzCreateUnitWithSkin(p, FourCC("n008"), -2235.9, -4569.3, 260.633, FourCC("n008"))
u = BlzCreateUnitWithSkin(p, FourCC("n006"), -2339.2, -4799.0, 32.744, FourCC("n006"))
u = BlzCreateUnitWithSkin(p, FourCC("n006"), -2119.3, -4787.6, 92.734, FourCC("n006"))
u = BlzCreateUnitWithSkin(p, FourCC("n002"), 1261.7, -3143.1, 131.301, FourCC("n002"))
u = BlzCreateUnitWithSkin(p, FourCC("n007"), -979.7, -4606.8, 51.448, FourCC("n007"))
u = BlzCreateUnitWithSkin(p, FourCC("n003"), 2121.2, -4417.9, 129.272, FourCC("n003"))
u = BlzCreateUnitWithSkin(p, FourCC("n002"), 1510.6, -3050.8, 104.312, FourCC("n002"))
u = BlzCreateUnitWithSkin(p, FourCC("n004"), 1188.7, -3827.8, 261.867, FourCC("n004"))
u = BlzCreateUnitWithSkin(p, FourCC("n004"), 1192.1, -4149.6, 351.605, FourCC("n004"))
u = BlzCreateUnitWithSkin(p, FourCC("n004"), 1439.4, -3960.2, 95.210, FourCC("n004"))
u = BlzCreateUnitWithSkin(p, FourCC("n005"), 2087.7, -3564.5, 176.425, FourCC("n005"))
u = BlzCreateUnitWithSkin(p, FourCC("n00A"), -1905.6, -159.7, 313.143, FourCC("n00A"))
end

function CreateNeutralPassiveBuildings()
local p = Player(PLAYER_NEUTRAL_PASSIVE)
local u
local unitID
local t
local life

u = BlzCreateUnitWithSkin(p, FourCC("ncop"), -29760.0, -29568.0, 270.000, FourCC("ncop"))
u = BlzCreateUnitWithSkin(p, FourCC("ncop"), -29504.0, -29568.0, 270.000, FourCC("ncop"))
u = BlzCreateUnitWithSkin(p, FourCC("ncop"), -29248.0, -29568.0, 270.000, FourCC("ncop"))
u = BlzCreateUnitWithSkin(p, FourCC("ncop"), -1664.0, -1536.0, 270.000, FourCC("ncop"))
end

function CreateNeutralPassive()
local p = Player(PLAYER_NEUTRAL_PASSIVE)
local u
local unitID
local t
local life

u = BlzCreateUnitWithSkin(p, FourCC("H002"), -29491.5, -29325.5, 266.352, FourCC("H002"))
u = BlzCreateUnitWithSkin(p, FourCC("H001"), -29750.8, -29318.1, 267.198, FourCC("H001"))
u = BlzCreateUnitWithSkin(p, FourCC("H000"), -29238.7, -29327.3, 266.824, FourCC("H000"))
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

gg_rct_WarriorSelectRegion = Rect(-29824.0, -29632.0, -29696.0, -29504.0)
gg_rct_MageSelectRegion = Rect(-29568.0, -29632.0, -29440.0, -29504.0)
gg_rct_ArcherSelectRegion = Rect(-29312.0, -29632.0, -29184.0, -29504.0)
gg_rct_HeroSpawnRegion = Rect(-64.0, -1248.0, 64.0, -1120.0)
gg_rct_JobAdvancementRegion = Rect(-1728.0, -1600.0, -1600.0, -1472.0)
gg_rct_GameStartRegion = Rect(-29568.0, -30176.0, -29440.0, -30048.0)
gg_rct_Dungeon1Entrance = Rect(-736.0, -992.0, -256.0, -640.0)
gg_rct_Dungeon1Start = Rect(-3456.0, -30208.0, -3328.0, -30080.0)
gg_rct_Dungeon1Zone = Rect(-3456.0, -30208.0, 224.0, -24480.0)
gg_rct_Dungeon1Area1 = Rect(-3456.0, -30208.0, 224.0, -26816.0)
gg_rct_Dungeon1Gate = Rect(-1920.0, -26816.0, -1280.0, -26688.0)
gg_rct_Dungeon1Area2 = Rect(-3456.0, -26848.0, 224.0, -24480.0)
gg_rct_Dungeon1Exit = Rect(-1344.0, -992.0, -1088.0, -640.0)
end

function InitCustomPlayerSlots()
SetPlayerStartLocation(Player(0), 0)
SetPlayerColor(Player(0), ConvertPlayerColor(0))
SetPlayerRacePreference(Player(0), RACE_PREF_HUMAN)
SetPlayerRaceSelectable(Player(0), true)
SetPlayerController(Player(0), MAP_CONTROL_USER)
SetPlayerStartLocation(Player(1), 1)
SetPlayerColor(Player(1), ConvertPlayerColor(1))
SetPlayerRacePreference(Player(1), RACE_PREF_HUMAN)
SetPlayerRaceSelectable(Player(1), true)
SetPlayerController(Player(1), MAP_CONTROL_USER)
SetPlayerStartLocation(Player(2), 2)
SetPlayerColor(Player(2), ConvertPlayerColor(2))
SetPlayerRacePreference(Player(2), RACE_PREF_HUMAN)
SetPlayerRaceSelectable(Player(2), true)
SetPlayerController(Player(2), MAP_CONTROL_USER)
SetPlayerStartLocation(Player(3), 3)
SetPlayerColor(Player(3), ConvertPlayerColor(3))
SetPlayerRacePreference(Player(3), RACE_PREF_HUMAN)
SetPlayerRaceSelectable(Player(3), true)
SetPlayerController(Player(3), MAP_CONTROL_USER)
SetPlayerStartLocation(Player(4), 4)
SetPlayerColor(Player(4), ConvertPlayerColor(4))
SetPlayerRacePreference(Player(4), RACE_PREF_HUMAN)
SetPlayerRaceSelectable(Player(4), true)
SetPlayerController(Player(4), MAP_CONTROL_USER)
SetPlayerStartLocation(Player(5), 5)
SetPlayerColor(Player(5), ConvertPlayerColor(5))
SetPlayerRacePreference(Player(5), RACE_PREF_HUMAN)
SetPlayerRaceSelectable(Player(5), true)
SetPlayerController(Player(5), MAP_CONTROL_USER)
SetPlayerStartLocation(Player(6), 6)
SetPlayerColor(Player(6), ConvertPlayerColor(6))
SetPlayerRacePreference(Player(6), RACE_PREF_HUMAN)
SetPlayerRaceSelectable(Player(6), true)
SetPlayerController(Player(6), MAP_CONTROL_USER)
SetPlayerStartLocation(Player(7), 7)
SetPlayerColor(Player(7), ConvertPlayerColor(7))
SetPlayerRacePreference(Player(7), RACE_PREF_HUMAN)
SetPlayerRaceSelectable(Player(7), true)
SetPlayerController(Player(7), MAP_CONTROL_USER)
end

function InitCustomTeams()
SetPlayerTeam(Player(0), 0)
SetPlayerTeam(Player(1), 0)
SetPlayerTeam(Player(2), 0)
SetPlayerTeam(Player(3), 0)
SetPlayerTeam(Player(4), 0)
SetPlayerTeam(Player(5), 0)
SetPlayerTeam(Player(6), 0)
SetPlayerTeam(Player(7), 0)
SetPlayerAllianceStateAllyBJ(Player(0), Player(1), true)
SetPlayerAllianceStateAllyBJ(Player(0), Player(2), true)
SetPlayerAllianceStateAllyBJ(Player(0), Player(3), true)
SetPlayerAllianceStateAllyBJ(Player(0), Player(4), true)
SetPlayerAllianceStateAllyBJ(Player(0), Player(5), true)
SetPlayerAllianceStateAllyBJ(Player(0), Player(6), true)
SetPlayerAllianceStateAllyBJ(Player(0), Player(7), true)
SetPlayerAllianceStateAllyBJ(Player(1), Player(0), true)
SetPlayerAllianceStateAllyBJ(Player(1), Player(2), true)
SetPlayerAllianceStateAllyBJ(Player(1), Player(3), true)
SetPlayerAllianceStateAllyBJ(Player(1), Player(4), true)
SetPlayerAllianceStateAllyBJ(Player(1), Player(5), true)
SetPlayerAllianceStateAllyBJ(Player(1), Player(6), true)
SetPlayerAllianceStateAllyBJ(Player(1), Player(7), true)
SetPlayerAllianceStateAllyBJ(Player(2), Player(0), true)
SetPlayerAllianceStateAllyBJ(Player(2), Player(1), true)
SetPlayerAllianceStateAllyBJ(Player(2), Player(3), true)
SetPlayerAllianceStateAllyBJ(Player(2), Player(4), true)
SetPlayerAllianceStateAllyBJ(Player(2), Player(5), true)
SetPlayerAllianceStateAllyBJ(Player(2), Player(6), true)
SetPlayerAllianceStateAllyBJ(Player(2), Player(7), true)
SetPlayerAllianceStateAllyBJ(Player(3), Player(0), true)
SetPlayerAllianceStateAllyBJ(Player(3), Player(1), true)
SetPlayerAllianceStateAllyBJ(Player(3), Player(2), true)
SetPlayerAllianceStateAllyBJ(Player(3), Player(4), true)
SetPlayerAllianceStateAllyBJ(Player(3), Player(5), true)
SetPlayerAllianceStateAllyBJ(Player(3), Player(6), true)
SetPlayerAllianceStateAllyBJ(Player(3), Player(7), true)
SetPlayerAllianceStateAllyBJ(Player(4), Player(0), true)
SetPlayerAllianceStateAllyBJ(Player(4), Player(1), true)
SetPlayerAllianceStateAllyBJ(Player(4), Player(2), true)
SetPlayerAllianceStateAllyBJ(Player(4), Player(3), true)
SetPlayerAllianceStateAllyBJ(Player(4), Player(5), true)
SetPlayerAllianceStateAllyBJ(Player(4), Player(6), true)
SetPlayerAllianceStateAllyBJ(Player(4), Player(7), true)
SetPlayerAllianceStateAllyBJ(Player(5), Player(0), true)
SetPlayerAllianceStateAllyBJ(Player(5), Player(1), true)
SetPlayerAllianceStateAllyBJ(Player(5), Player(2), true)
SetPlayerAllianceStateAllyBJ(Player(5), Player(3), true)
SetPlayerAllianceStateAllyBJ(Player(5), Player(4), true)
SetPlayerAllianceStateAllyBJ(Player(5), Player(6), true)
SetPlayerAllianceStateAllyBJ(Player(5), Player(7), true)
SetPlayerAllianceStateAllyBJ(Player(6), Player(0), true)
SetPlayerAllianceStateAllyBJ(Player(6), Player(1), true)
SetPlayerAllianceStateAllyBJ(Player(6), Player(2), true)
SetPlayerAllianceStateAllyBJ(Player(6), Player(3), true)
SetPlayerAllianceStateAllyBJ(Player(6), Player(4), true)
SetPlayerAllianceStateAllyBJ(Player(6), Player(5), true)
SetPlayerAllianceStateAllyBJ(Player(6), Player(7), true)
SetPlayerAllianceStateAllyBJ(Player(7), Player(0), true)
SetPlayerAllianceStateAllyBJ(Player(7), Player(1), true)
SetPlayerAllianceStateAllyBJ(Player(7), Player(2), true)
SetPlayerAllianceStateAllyBJ(Player(7), Player(3), true)
SetPlayerAllianceStateAllyBJ(Player(7), Player(4), true)
SetPlayerAllianceStateAllyBJ(Player(7), Player(5), true)
SetPlayerAllianceStateAllyBJ(Player(7), Player(6), true)
end

function InitAllyPriorities()
SetStartLocPrioCount(0, 1)
SetStartLocPrio(0, 0, 1, MAP_LOC_PRIO_HIGH)
SetStartLocPrioCount(1, 2)
SetStartLocPrio(1, 0, 0, MAP_LOC_PRIO_HIGH)
SetStartLocPrio(1, 1, 2, MAP_LOC_PRIO_HIGH)
SetStartLocPrioCount(2, 2)
SetStartLocPrio(2, 0, 1, MAP_LOC_PRIO_HIGH)
SetStartLocPrio(2, 1, 3, MAP_LOC_PRIO_HIGH)
SetStartLocPrioCount(3, 2)
SetStartLocPrio(3, 0, 2, MAP_LOC_PRIO_HIGH)
SetStartLocPrio(3, 1, 4, MAP_LOC_PRIO_HIGH)
SetStartLocPrioCount(4, 2)
SetStartLocPrio(4, 0, 3, MAP_LOC_PRIO_HIGH)
SetStartLocPrio(4, 1, 5, MAP_LOC_PRIO_HIGH)
SetStartLocPrioCount(5, 2)
SetStartLocPrio(5, 0, 4, MAP_LOC_PRIO_HIGH)
SetStartLocPrio(5, 1, 6, MAP_LOC_PRIO_HIGH)
SetStartLocPrioCount(6, 2)
SetStartLocPrio(6, 0, 5, MAP_LOC_PRIO_HIGH)
SetStartLocPrio(6, 1, 7, MAP_LOC_PRIO_HIGH)
SetStartLocPrioCount(7, 1)
SetStartLocPrio(7, 0, 6, MAP_LOC_PRIO_HIGH)
end

function main()
SetCameraBounds(-29952.0 + GetCameraMargin(CAMERA_MARGIN_LEFT), -30208.0 + GetCameraMargin(CAMERA_MARGIN_BOTTOM), 29952.0 - GetCameraMargin(CAMERA_MARGIN_RIGHT), 29696.0 - GetCameraMargin(CAMERA_MARGIN_TOP), -29952.0 + GetCameraMargin(CAMERA_MARGIN_LEFT), 29696.0 - GetCameraMargin(CAMERA_MARGIN_TOP), 29952.0 - GetCameraMargin(CAMERA_MARGIN_RIGHT), -30208.0 + GetCameraMargin(CAMERA_MARGIN_BOTTOM))
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
SetPlayers(8)
SetTeams(8)
SetGamePlacement(MAP_PLACEMENT_TEAMS_TOGETHER)
DefineStartLocation(0, -29696.0, -29952.0)
DefineStartLocation(1, -29632.0, -29952.0)
DefineStartLocation(2, -29568.0, -29952.0)
DefineStartLocation(3, -29504.0, -29952.0)
DefineStartLocation(4, -29440.0, -29952.0)
DefineStartLocation(5, -29376.0, -29952.0)
DefineStartLocation(6, -29312.0, -29952.0)
DefineStartLocation(7, -29248.0, -29952.0)
InitCustomPlayerSlots()
InitCustomTeams()
InitAllyPriorities()
end

