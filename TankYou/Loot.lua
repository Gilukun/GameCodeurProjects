Loot = {}

missile = 0

TypeLoot = {}
TypeLoot.AddLifeSmall = "LIFE+"
TypeLoot.AddLifeBig = "LIFE++"
TypeLoot.Shield = "SHIELD"
TypeLoot.IEM = "IEM"

PrisonerSpawn = {}
function CreerPrisonerSpawn()
    PrisonerSpawn[1] = {
        x = 500,
        y = 100
    }
    PrisonerSpawn[2] = {
        x = 100,
        y = 500
    }
    PrisonerSpawn[3] = {
        x = 500,
        y = 700
    }
    PrisonerSpawn[4] = {
        x = 900,
        y = 500
    }
    PrisonerSpawn[5] = {
        x = 500,
        y = 550
    }
end

function Loot.Start()
    list_Loot = {}
    CreerPrisonerSpawn()
end

function Loot.CreerLoot(pNom, pX, pY)
    local loot = {}
    loot.nom = pNom
    loot.x = pX
    loot.y = pY
    table.insert(list_Loot, loot)
end

prisoner = {}
prisoner.frames = {}
prisoner.frame = 1
prisoner.image = nil
prisoner.x = 0
prisoner.y = 0
prisoner.saved = false

function Loot.Load()
    Img_LootS = love.graphics.newImage("Images/Loot_S.png")
    Img_LootL = love.graphics.newImage("Images/Loot_L.png")
    Img_LootShield = love.graphics.newImage("Images/Loot_Shield.png")

    prisoner.image = love.graphics.newImage("Images/Prisoner.png")
    largeurPrisoner = prisoner.image:getWidth()
    hauteurPrisoner = prisoner.image:getHeight()
    prisoner.frames[1] = love.graphics.newQuad(0, 0, 25, 31, prisoner.image:getWidth(), prisoner.image:getHeight())
    prisoner.frames[2] = love.graphics.newQuad(25, 0, 25, 31, prisoner.image:getWidth(), prisoner.image:getHeight())
end

function Loot.Update(dt)
    prisoner.frame = prisoner.frame + 2 * dt
    if prisoner.frame >= #prisoner.frames + 1 then
        prisoner.frame = 1
    end
end

function Loot.Draw()
    for k, v in ipairs(list_Loot) do
        if v.nom == TypeLoot.AddLifeSmall then
            love.graphics.draw(Img_LootS, v.x, v.y)
        end
        if v.nom == TypeLoot.AddLifeBig then
            love.graphics.draw(Img_LootL, v.x, v.y)
        end

        if v.nom == TypeLoot.Shield then
            love.graphics.draw(Img_LootShield, v.x, v.y)
        end
    end

    for z = #PrisonerSpawn, 1, -1 do
        local spwan = PrisonerSpawn[z]
        local frameArrondie = math.floor(prisoner.frame)
        love.graphics.draw(
            prisoner.image,
            prisoner.frames[frameArrondie],
            spwan.x,
            spwan.y,
            0,
            1,
            1,
            largeurPrisoner / 4,
            hauteurPrisoner / 2
        )
    end
end

function Loot.GameWin()
    if #PrisonerSpawn == 0 then
        Sd_Lvl1:stop()
        G_State = GameState.WIN
    end
end
return Loot
