Sounds = {}

function Sounds.Load()
    Sd_Menu = love.audio.newSource("Sounds/MENU.mp3", "static")
    Sd_Lvl1 = love.audio.newSource("Sounds/Lvl1.mp3", "static")
    Sd_DEAD = love.audio.newSource("Sounds/DEAD.mp3", "static")
    Sd_GAMEOVER = love.audio.newSource("Sounds/GAMEOVER.mp3", "static")
    Sd_WIN = love.audio.newSource("Sounds/WIN.mp3", "static")

    Sd_TANK = love.audio.newSource("Sounds/TANK.mp3", "static")
    Sd_SHOOT = love.audio.newSource("Sounds/SHOOT.wav", "static")
    Sd_TOWER = love.audio.newSource("Sounds/TOWER.wav", "static")
    Sd_Shield = love.audio.newSource("Sounds/Shield.wav", "static")
    SFX_HIT_ENNEMY = love.audio.newSource("Sounds/HITENNEMY.wav", "static")
    SFX_DEAD_ENNEMY = love.audio.newSource("Sounds/DEADENNEMY.wav", "static")
    SFX_HIT_HERO = love.audio.newSource("Sounds/HITHERO.wav", "static")
    SFX_PRISONER_SAVED = love.audio.newSource("Sounds/SAVED.mp3", "static")
    SFX_HEALTH = love.audio.newSource("Sounds/HEALTH.wav", "static")
    SFX_SHIELD_LOOT = love.audio.newSource("Sounds/SHIELD_LOOT.wav", "static")
end

-- Fonctions pour augmenter / diminuer le volume genéral
local volumeLevel = love.audio.getVolume()
function Sounds.LevelUp()
    love.audio.setVolume(volumeLevel)
    if volumeLevel <= 1 then
        volumeLevel = volumeLevel + 0.1
    elseif volumeLevel >= 1 then
        volumeLevel = 1
    end
end
function Sounds.LevelDown()
    love.audio.setVolume(volumeLevel)
    if volumeLevel >= 0 then
        volumeLevel = volumeLevel - 0.1
    elseif volumeLevel >= 1 then
        volumeLevel = 1
    end
end

return Sounds
