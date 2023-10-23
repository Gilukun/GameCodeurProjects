GUI = {}

function GUI.AddPlayerData()
    Player_Score = 0
    Player_LifeInit = 100
    Player_life = Player_LifeInit
    AddScore = 100
    RemoveLife = 10
    Prisoner = 0
    bonus = 500
end

local Player_lifeContour = {}
Player_lifeContour.x = 10
Player_lifeContour.y = 10
Player_lifeContour.Width = 300
Player_lifeContour.Height = 10

local Player_lifeInlay = {}
Player_lifeInlay.x = 10
Player_lifeInlay.y = 10
Player_lifeInlay.Width = 300
Player_lifeInlay.Height = 10

local ShieldActive = {}
ShieldActive.Iconx = 10
ShieldActive.Icony = 50
ShieldActive.x = 25
ShieldActive.y = 28
ShieldActive.Width = 0
ShieldActive.Height = 10
ShieldActiveWidthInit = 100
ShieldActiveWidth = ShieldActive.Width

local EMIActive = {}
EMIActive.x = 10
EMIActive.y = 44
EMIActive.Width = 100
EMIActive.Height = 10

function GUI.Start()
    GUI.AddPlayerData()
    Player_life = Player_LifeInit
    ShieldActiveWidth = 0
end

function GUI.AddScore()
    Player_Score = Player_Score + AddScore
    if Hero.Player.etat == ETAT_PLAYER.DEAD then
        Player_Score = 0
    end
end

function GUI.AddLife()
    for k, v in ipairs(list_Loot) do
        if v.nom == TypeLoot.AddLifeSmall then
            if Player_life < Player_LifeInit then
                Player_life = Player_life + 5
            end
        elseif v.nom == TypeLoot.AddLifeBig then
            if Player_life < Player_LifeInit then
                Player_life = Player_life + 10
            end
        end
    end
end

function GUI.AddShield()
    for k, v in ipairs(list_Loot) do
        if v.nom == TypeLoot.Shield then
            if ShieldActiveWidth < ShieldActiveWidthInit then
                ShieldActiveWidth = ShieldActiveWidth + 10
            end
        end
    end
end

function GUI.RemoveHeroLife(dt)
    if Player_life > 0 then
        Player_life = Player_life - RemoveLife
    elseif Player_life <= 0.5 then
        Sd_DEAD:play()
        G_State = GameState.GameOver
        Hero.Player.etat = ETAT_PLAYER.DEAD
    end
end

function AddPrisonersScore()
    Prisoner = Prisoner + 100
    if Prisoner >= 300 then
        Player_Score = Player_Score + bonus
    end
end

function GUI.Load()
    IMG_Shield = love.graphics.newImage("Images/forcefield.png")
    largueurIMG_Shield = IMG_Shield:getWidth()
    hauteurIMG_Shield = IMG_Shield:getHeight()
    Img_Shield1 = love.graphics.newQuad(0, 0, 128, 128, largueurIMG_Shield, hauteurIMG_Shield)
    Img_Shield2 = love.graphics.newQuad(140, 0, 128, 128, largueurIMG_Shield, hauteurIMG_Shield)
    Img_Shield3 = love.graphics.newQuad(280, 0, 128, 128, largueurIMG_Shield, hauteurIMG_Shield)
    Icon_Shield = love.graphics.newImage("Images/Loot_Shield.png")

    font = love.graphics.newFont(20)
end

local largeurGrid = 32
local HauteurGrid = 38

local nb_ligne = math.ceil(1024 / largeurGrid)
local nb_col = math.ceil(1024 / HauteurGrid)
local grid = {}
for l = 1, nb_ligne do
    grid[l] = {}
    for c = 1, nb_col do
        grid[l][c] = ((l - 1) * nb_col + c)
    end
end

function GUI.Draw()
    local ratio_Life = Player_life / Player_LifeInit
    love.graphics.setColor(love.math.colorFromBytes(255, 18, 0))
    love.graphics.rectangle(
        "fill",
        Player_lifeInlay.x,
        Player_lifeInlay.y,
        Player_lifeInlay.Width * ratio_Life,
        Player_lifeInlay.Height
    )
    love.graphics.setColor(1, 1, 1)
    love.graphics.rectangle(
        "line",
        Player_lifeContour.x,
        Player_lifeContour.y,
        Player_lifeContour.Width,
        Player_lifeContour.Height
    )

    love.graphics.draw(
        Icon_Shield,
        ShieldActive.Iconx,
        ShieldActive.Icony,
        0,
        1 / 2,
        1 / 2,
        ShieldActive.Iconx,
        ShieldActive.Icony
    )

    if Shield_ON == false then
        love.graphics.setColor(love.math.colorFromBytes(27, 175, 173))
        love.graphics.rectangle("fill", ShieldActive.x, ShieldActive.y, ShieldActiveWidth, ShieldActive.Height)
        love.graphics.setColor(1, 1, 1)
    elseif Shield_ON == true then
        if Shield_Timer >= Shield_Duration * 0.7 then
            love.graphics.setColor(1, 1, 1, 0.7)
            love.graphics.draw(
                IMG_Shield,
                Img_Shield1,
                Hero.Player.x,
                Hero.Player.y,
                0,
                1,
                1,
                largueurIMG_Shield / 6,
                hauteurIMG_Shield / 2
            )
        elseif Shield_Timer >= Shield_Duration * 0.3 and Shield_Timer <= Shield_Duration * 0.7 then
            love.graphics.draw(
                IMG_Shield,
                Img_Shield2,
                Hero.Player.x,
                Hero.Player.y,
                0,
                1,
                1,
                largueurIMG_Shield / 6,
                hauteurIMG_Shield / 2
            )
        elseif Shield_Timer <= Shield_Duration * 0.3 then
            love.graphics.draw(
                IMG_Shield,
                Img_Shield3,
                Hero.Player.x,
                Hero.Player.y,
                0,
                1,
                1,
                largueurIMG_Shield / 6,
                hauteurIMG_Shield / 2
            )
            love.graphics.setColor(1, 1, 1, 1)
        end

        local ratio_Shield = Shield_Timer / Shield_Duration
        love.graphics.setColor(love.math.colorFromBytes(27, 175, 173))
        love.graphics.rectangle(
            "fill",
            ShieldActive.x,
            ShieldActive.y,
            ShieldActiveWidth * ratio_Shield,
            ShieldActive.Height
        )
        love.graphics.setColor(1, 1, 1)
    end

    if EMI_ON == true then
        local ratio_EMI = EMI_Timer / EMI_Duration
        love.graphics.setColor(love.math.colorFromBytes(27, 255, 173))
        love.graphics.rectangle("fill", EMIActive.x, EMIActive.y, EMIActive.Width * ratio_EMI, EMIActive.Height)
        love.graphics.setColor(1, 1, 1)
    end

    for k, v in ipairs(list_Ennemis) do
        if v.vom == Nom_Ennemis.TANK then
            love.graphics.setColor(love.math.colorFromBytes(231, 50, 36))
            love.graphics.rectangle("fill", v.x - largeurImg_tank_E / 2, v.y - hauteurImg_tank_E / 2, v.life, 4)
            love.graphics.setColor(1, 1, 1)
        elseif v.nom == Nom_Ennemis.TOWER then
            love.graphics.setColor(love.math.colorFromBytes(231, 50, 36))
            love.graphics.rectangle("fill", v.x - largeurImg_Tower / 2, v.y - largeurImg_Tower / 2, v.life * 2, 4)
            love.graphics.setColor(1, 1, 1)
        end
    end

    --love.graphics.setFont(font)

    for l = 1, nb_ligne do
        for c = 1, nb_col do
            if grid[l][c] == 676 then
                love.graphics.setColor(1, 1, 1, 0.6)
                love.graphics.draw(
                    prisoner.image,
                    prisoner.frames[1],
                    (l - 1) * largeurGrid + largeurPrisoner / 3,
                    (c - 1) * HauteurGrid + hauteurPrisoner / 2,
                    0,
                    1,
                    1,
                    largeurPrisoner / 3,
                    hauteurPrisoner / 2
                )
                love.graphics.setColor(1, 1, 1, 1)
            end
            if grid[l][c] == 703 then
                local font = love.graphics.getFont()
                local largeurPScore = font:getWidth(tostring(Prisoners))
                local hauteurPScore = font:getHeight(tostring(Prisoner))
                love.graphics.print(
                    tostring(Prisoner),
                    (l - 1) * largeurGrid + largeurGrid / 2,
                    (c - 1) * HauteurGrid + HauteurGrid / 2,
                    O,
                    1,
                    1,
                    largeurPScore / 2,
                    hauteurPScore / 2
                )
            end
            if grid[l][c] == 757 then
                love.graphics.setColor(1, 1, 1, 0.9)
                love.graphics.draw(Img_tank_E, (l - 1) * largeurGrid, (c - 1) * HauteurGrid, 0, 0.8, 0.8)
                love.graphics.setColor(1, 1, 1, 1)
            end
            if grid[l][c] == 811 then
                local font = love.graphics.getFont()
                local largeurScore = font:getWidth(tostring(Player_Score))
                local hauteurScore = font:getHeight(tostring(Player_Score))
                love.graphics.setColor(1, 1, 1, 0.9)
                love.graphics.print(
                    tostring(Player_Score),
                    (l - 1) * largeurGrid + largeurGrid / 2,
                    (c - 1) * HauteurGrid + HauteurGrid / 2,
                    0,
                    1,
                    1,
                    largeurScore / 2,
                    hauteurScore / 2
                )
                love.graphics.setColor(1, 1, 1, 1)
            end
        end
    end
    for l = 1, nb_ligne do
        for c = 1, nb_col do
            --love.graphics.print(tostring(grid[l][c]), (l - 1) * largeurGrid, (c - 1) * HauteurGrid)
            -- love.graphics.rectangle("line", (l - 1) * largeurGrid, (c - 1) * HauteurGrid, largeurGrid, HauteurGrid)
        end
    end
end

return GUI
