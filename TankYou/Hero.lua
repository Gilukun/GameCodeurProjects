if pcall(require, "lldebugger") then
    require("lldebugger").start()
end
io.stdout:setvbuf("no")

Hero = {}

local Weapons = require("Weapons")
local GUI = require("GUI")
local loot = require("Loot")
local Maps = require("maps")

-- ETATS PLAYER
ETAT_PLAYER = {}
ETAT_PLAYER.IDLE = "IDLE"
ETAT_PLAYER.MOVE = "MOVE"
ETAT_PLAYER.DEAD = "DEAD"
ETAT_PLAYER.COLLISIONE = "COL_E"
ETAT_PLAYER.COLLISIONLAYER = "COL_L"

Hero.Player = {}
Hero.Player.x = 0
Hero.Player.y = 0
Hero.Player.angle = 0
Hero.Player.KeyPressed = false
Hero.Player.etat = ETAT_PLAYER.IDLE
Hero.Player.life = 100

local vitessex = 200
local vitessey = 200

function Hero.Start()
    Hero.Player.x = 400
    Hero.Player.y = 200
    Hero.Player.etat = ETAT_PLAYER.IDLE
    Hero.Player.angle = 0
    Hero.Player.KeyPressed = false
    Hero.Player.life = 100
end

function Hero.Load()
    lScreen = love.graphics.getWidth()
    hScreen = love.graphics.getHeight()

    Img_Player = love.graphics.newImage("Images/tank.png")
    largeurImg_Player = Img_Player:getWidth()
    hauteurImg_Player = Img_Player:getHeight()

    Img_Canon = love.graphics.newImage("Images/Canon.png")
    largeurImg_Canon = Img_Canon:getWidth()
    hauteurImg_Canon = Img_Canon:getHeight()

    Img_Obus = love.graphics.newImage("Images/Obus.png")
    largeurImg_Obus = Img_Obus:getWidth()
    hauteurImg_Obus = Img_Obus:getHeight()
    Hero.Start()
end

function Hero.IsHit(dt)
    local no
    for no = #listObus, 1, -1 do
        local o = listObus[no]
        if o.nom == NomObus.Ennemis or o.nom == NomObus.LaserTower then
            local dist = math.dist(Hero.Player.x, Hero.Player.y, o.x, o.y)
            if Shield_ON == false then
                if dist < largeurImg_Player / 2 then
                    table.remove(listObus, no)
                    Weapons.CreerExplosion(Hero.Player.x, Hero.Player.y)
                    GUI.RemoveHeroLife(dt)
                end
            end
            if Shield_ON == true then
                if dist <= largueurIMG_Shield / 3 then
                    table.remove(listObus, no)
                end
            end
        end
    end
end

function Hero.Etats(dt)
    if Hero.Player.etat == ETAT_PLAYER.IDLE then
        Hero.Player.etat = ETAT_PLAYER.MOVE
    elseif Hero.Player.etat == ETAT_PLAYER.MOVE then
        oldx = Hero.Player.x
        oldy = Hero.Player.y
        if love.keyboard.isDown("z") then
            Hero.Player.x = Hero.Player.x + (vitessex * dt) * math.cos(Hero.Player.angle)
            Hero.Player.y = Hero.Player.y + (vitessey * dt) * math.sin(Hero.Player.angle)
        end
        if love.keyboard.isDown("s") then
            Hero.Player.x = Hero.Player.x - (vitessex * dt) * math.cos(Hero.Player.angle)
            Hero.Player.y = Hero.Player.y - (vitessey * dt) * math.sin(Hero.Player.angle)
        end
        if love.keyboard.isDown("q") then
            Hero.Player.angle = Hero.Player.angle - 3 * dt
        end
        if love.keyboard.isDown("d") then
            Hero.Player.angle = Hero.Player.angle + 3 * dt
        end

        for k, v in ipairs(list_Ennemis) do
            if v.nom == Nom_Ennemis.TANK then
                if math.dist(Hero.Player.x, Hero.Player.y, v.x, v.y) < largeurImg_Player then
                    Hero.Player.x = oldx
                    Hero.Player.y = oldy
                end
            elseif v.nom == Nom_Ennemis.TOWER then
                if math.dist(Hero.Player.x, Hero.Player.y, v.x - largeurImg_Tower / 3, v.y) < largeurImg_Player / 1.5 then
                    Hero.Player.x = oldx
                    Hero.Player.y = oldy
                end
            end
        end

        for z = #PrisonerSpawn, 1, -1 do
            local spawn = PrisonerSpawn[z]
            if math.dist(Hero.Player.x, Hero.Player.y, spawn.x, spawn.y) < largeurImg_Player / 1.5 then
                prisoner.saved = true
                AddPrisonersScore()
                SFX_PRISONER_SAVED:stop()
                SFX_PRISONER_SAVED:play()
                table.remove(PrisonerSpawn, z)
            end
        end

        if Maps.CollisionsLayers(Hero.Player.x, Hero.Player.y, largeurImg_Player, hauteurImg_Player) == true then
            Hero.Player.x = oldx
            Hero.Player.y = oldy
        end

        for n = #list_Loot, 1, -1 do
            local l = list_Loot[n]
            if math.dist(Hero.Player.x, Hero.Player.y, l.x, l.y) < largeurImg_Player then
                if l.nom == TypeLoot.AddLifeSmall then
                    GUI.AddLife()
                    SFX_HEALTH:play()
                end
                if l.nom == TypeLoot.AddLifeBig then
                    GUI.AddLife()
                    SFX_HEALTH:play()
                end
                if l.nom == TypeLoot.Shield then
                    GUI.AddShield()
                    SFX_SHIELD_LOOT:play()
                end
                table.remove(list_Loot, n)
            end
        end
    elseif Hero.Player.etat == ETAT_PLAYER.DEAD then
    end
end

-- VISEE AVEC LE CANON
function Hero.Canon()
    local mousex = love.mouse.getX()
    local mousey = love.mouse.getY()
    angle_Canon = math.angle(Hero.Player.x, Hero.Player.y, mousex, mousey)
end

function Hero.Move(dt)
    Hero.Etats(dt)

    if Hero.Player.x + largeurImg_Player / 2 >= lScreen then
        Hero.Player.x = lScreen - largeurImg_Player / 2
    end

    if Hero.Player.x - largeurImg_Player / 2 <= 0 then
        Hero.Player.x = largeurImg_Player / 2
    end

    if Hero.Player.y + hauteurImg_Player / 2 >= hScreen then
        Hero.Player.y = hScreen - hauteurImg_Player / 2
    end

    if Hero.Player.y - hauteurImg_Player / 2 <= 0 then
        Hero.Player.y = hauteurImg_Player / 2
    end

    Hero.IsHit(dt)
    Hero.Canon()
end

function Hero.mousepressed(x, y, button)
    if WeaponTypes == W_Types.Basic then
        if button == 1 then
            Weapons.CreerObus(NomObus.Hero, Hero.Player.x, Hero.Player.y, angle_Canon, 500, 0.6)
            Sd_SHOOT:stop()
            Sd_SHOOT:play()
        end
    end
end

function Hero.Draw()
    love.graphics.draw(
        Img_Player,
        Hero.Player.x,
        Hero.Player.y,
        Hero.Player.angle,
        1,
        1,
        largeurImg_Player / 2,
        hauteurImg_Player / 2
    )
    love.graphics.draw(
        Img_Canon,
        Hero.Player.x,
        Hero.Player.y,
        angle_Canon,
        1,
        1,
        largeurImg_Canon / 2,
        hauteurImg_Canon / 2
    )
end

return Hero
