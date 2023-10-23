Weapons = {}

NomObus = {}
NomObus.Hero = "HERO"
NomObus.Ennemis = "ENNEMY"
NomObus.LaserTower = "LASER"

W_Types = {}
W_Types.Basic = "CANON"
W_Types.Heavy = "MISSILE"
W_Types.Shield = "BOUCLIER"

WeaponTypes = W_Types.Basic

W_Style = {}
W_Style.ATTACK = "ATTAQUE"
W_Style.SHIELD = "BOUCLIER"

-- Super Pouvoir
EMI_Duration = 1
EMI_Timer = EMI_Duration
EMI_Radius = 0
EMI_Radius_Init = EMI_Radius
EMI_ON = false

-- Bouclier
Shield_Duration = 5
Shield_Timer = Shield_Duration
Shield_Radius = 50
Shield_ON = false

function Weapons.Load()
    Sd_Shield = love.audio.newSource("Sounds/Shield.wav", "static")
    Img_Laser = love.graphics.newImage("Images/vortex.png")
    largeurImg_Laser = Img_Laser:getWidth()
    hauteurImg_Laser = Img_Laser:getHeight()
    Img_Hit = love.graphics.newImage("Images/Hit.png")
    largeurImg_Hit = Img_Hit:getWidth()
    hauteurImg_Hit = Img_Hit:getHeight()
    Img_Dead = love.graphics.newImage("Images/Explosion.png")
    largeurImg_Dead = Img_Dead:getWidth()
    hauteurImg_Dead = Img_Dead:getHeight()
end

function Weapons.Start()
    listObus = {}
    list_Explosion = {}
end

function Weapons.CreerExplosion(pX, pY)
    local Boum = {}
    Boum.x = pX
    Boum.y = pY
    Boum.vie = 0.2
    table.insert(list_Explosion, Boum)
end
function Weapons.CreerObus(pNom, pX, pY, pAngle, pVitesse, pLife)
    local Obus = {}
    Obus.nom = pNom
    Obus.x = pX
    Obus.y = pY
    Obus.angle = pAngle
    Obus.vitesse = pVitesse
    Obus.life = pLife
    table.insert(listObus, Obus)
end

function Weapons.Obus(dt)
    local n
    for n = #listObus, 1, -1 do
        local o = listObus[n]
        o.x = o.x + (o.vitesse * dt) * math.cos(o.angle)
        o.y = o.y + (o.vitesse * dt) * math.sin(o.angle)
        o.life = o.life - dt
        if o.life <= 0 then
            table.remove(listObus, n)
        end

        if o.x > lScreen or o.x < 0 or o.y > hScreen or o.y < 0 then
            table.remove(listObus, n)
        end

        local nbligne = #list_Layers.background / TILE_WIDTH
        local nbcol = TILE_HEIGHT
        local l, c
        Collision = false
        for l = nbligne, 1, -1 do
            for c = 1, nbcol do
                local tuile = list_Layers.walls[((l - 1) * TILE_HEIGHT) + c]
                if tuile > 0 then
                    if
                        CheckCollision(
                            o.x,
                            o.y,
                            largeurImg_Obus,
                            largeurImg_Obus,
                            (c - 1) * TILE_WIDTH,
                            (l - 1) * TILE_HEIGHT,
                            TILE_WIDTH,
                            TILE_HEIGHT
                        ) == true
                     then
                        table.remove(listObus, n)
                        Weapons.CreerExplosion(o.x, o.y)
                    end
                end
            end
        end
    end
end

function Weapons.Type(pNom)
    if pNom == W_Style.ATTACK then
        if WeaponTypes == W_Types.Basic then
            WeaponTypes = W_Types.Heavy
        end
    end

    if pNom == W_Style.SHIELD then
        if WeaponTypes == W_Types.Basic then
            WeaponTypes = W_Types.Shield
        end
    end
end

function Weapons.EMI(dt)
    if WeaponTypes == W_Types.Heavy then
        EMI_ON = true
        if EMI_Timer >= 0 then
            EMI_Timer = EMI_Timer - dt
            EMI_Radius_Init = EMI_Radius_Init + 500 * dt
            if EMI_Radius_Init >= 100 then
                EMI_Radius_Init = 100
            end
        end
        if EMI_Timer <= 0 then
            WeaponTypes = W_Types.Basic
            EMI_Timer = EMI_Duration
            EMI_Radius_Init = EMI_Radius
            EMI_ON = false
        end
    end
end

list_Dead = {}
function Weapons.Kill(pX, pY)
    local Dead = {}
    Dead.x = pX
    Dead.y = pY
    Dead.vie = 1
    Dead.alpha = 0
    table.insert(list_Dead, Dead)
end

function Weapons.Shield(dt)
    if WeaponTypes == W_Types.Shield then
        if Shield_Timer >= 0 then
            Shield_Timer = Shield_Timer - dt
            Shield_ON = true
        end

        if Shield_Timer <= 0 then
            WeaponTypes = W_Types.Basic
            Shield_Timer = Shield_Duration
            ShieldActiveWidth = 0
            Shield_ON = false
        end
    end
end

function Weapons.Update(dt)
    for n = #list_Explosion, 1, -1 do
        local exp = list_Explosion[n]
        exp.vie = exp.vie - dt
        if exp.vie <= 0 then
            table.remove(list_Explosion, n)
        end
    end

    for n = #list_Dead, 1, -1 do
        local dead = list_Dead[n]
        dead.vie = dead.vie - dt
        if dead.vie <= 0 then
            table.remove(list_Dead, n)
        end
    end

    for k, d in ipairs(list_Dead) do
        if d.alpha <= 1 then
            d.alpha = d.alpha + dt
        end
    end
end

function Weapons.Draw()
    if WeaponTypes == W_Types.Heavy then
        love.graphics.setColor(love.math.colorFromBytes(158, 26, 11, 100))
        love.graphics.circle("fill", Hero.Player.x, Hero.Player.y, EMI_Radius_Init)
        love.graphics.setColor(1, 1, 1, 1)
    end

    for k, v in ipairs(listObus) do
        if v.nom == NomObus.Hero or v.nom == NomObus.Ennemis then
            love.graphics.draw(Img_Obus, v.x, v.y, v.angle, 1 / 2, 1 / 2, largeurImg_Obus / 2, hauteurImg_Obus / 2)
        elseif v.nom == NomObus.LaserTower then
            love.graphics.draw(
                Img_Laser,
                v.x - largeurImg_Tower / 3,
                v.y,
                v.angle,
                1 / 2,
                1 / 2,
                largeurImg_Laser / 2,
                largeurImg_Laser / 2
            )
        end
    end
    for k, e in ipairs(list_Explosion) do
        love.graphics.draw(Img_Hit, e.x, e.y, 0, 1 / 2, 1 / 2, largeurImg_Hit / 2, largeurImg_Hit / 2)
    end

    for k, d in ipairs(list_Dead) do
        love.graphics.setColor(1, 1, 1, d.alpha)
        love.graphics.draw(Img_Dead, d.x, d.y, 0, 1 / 2, 1 / 2, largeurImg_Dead / 2, largeurImg_Dead / 2)
        love.graphics.setColor(1, 1, 1, 1)
    end
end

return Weapons
