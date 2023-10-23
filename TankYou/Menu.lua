Menu = {}

local carte = require("Menu_Data")
local Maps = require("maps")

-- Timer pour que "Press Start" clignote
local flash = 2
local timer = flash
local pressStart = true
-- Liste de layers utilisés pour le menu
local Layers = {
    background = carte.layers[1].data,
    walls = carte.layers[2].data,
    decor = carte.layers[3].data,
    Animation = carte.layers[4].data
}

local tile = {}

function Menu.Load()
    Img_title = love.graphics.newImage("Images/Tank You.png")
    largeurImg_title = love.graphics.getWidth() / 2
    hauteurImg_title = love.graphics.getHeight() / 2

    Img_Press_enter = love.graphics.newImage("Images/PressEntertxt.png")

    Tilesheet = love.graphics.newImage("Images/TileSet1.png")
    Largeurtilesheet = Tilesheet:getWidth()
    HauteurTilesheet = Tilesheet:getHeight()

    KeyboardSheet = love.graphics.newImage("Images/keyboards.png")
    LargeurKeyboardSheet = KeyboardSheet:getWidth()
    HauteurKeyboardSheet = KeyboardSheet:getHeight()
    Key_Z = love.graphics.newQuad(280, 38, 34, 38, LargeurKeyboardSheet, HauteurKeyboardSheet)
    Key_Q = love.graphics.newQuad(560, 0, 34, 38, LargeurKeyboardSheet, HauteurKeyboardSheet)
    Key_S = love.graphics.newQuad(34, 38, 34, 38, LargeurKeyboardSheet, HauteurKeyboardSheet)
    Key_D = love.graphics.newQuad(103, 0, 34, 38, LargeurKeyboardSheet, HauteurKeyboardSheet)
    Key_C = love.graphics.newQuad(68, 0, 34, 38, LargeurKeyboardSheet, HauteurKeyboardSheet)
    Key_V = love.graphics.newQuad(138, 38, 34, 38, LargeurKeyboardSheet, HauteurKeyboardSheet)
    Key_B = love.graphics.newQuad(34, 0, 34, 38, LargeurKeyboardSheet, HauteurKeyboardSheet)
    Key_U = love.graphics.newQuad(103, 38, 34, 38, LargeurKeyboardSheet, HauteurKeyboardSheet)
    Key_I = love.graphics.newQuad(280, 0, 34, 38, LargeurKeyboardSheet, HauteurKeyboardSheet)
    Mouse = love.graphics.newImage("Images/Mouse.png")

    local nbcol = Largeurtilesheet / TILE_HEIGHT
    local nbline = HauteurTilesheet / TILE_WIDTH

    TileTextures[0] = nil
    local c, l
    local id = 1
    for l = 1, nbline do
        for c = 1, nbcol do
            TileTextures[id] =
                love.graphics.newQuad(
                (c - 1) * TILE_WIDTH,
                (l - 1) * TILE_HEIGHT,
                TILE_WIDTH,
                TILE_HEIGHT,
                Largeurtilesheet,
                HauteurTilesheet
            )
            id = id + 1
        end
    end
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

function Menu.Update(dt)
    if timer > 1 then
        pressStart = true
        timer = timer - dt
    end
    if timer < 1 then
        timer = timer - dt
        pressStart = false
    end
    if timer <= 0 then
        timer = flash
    end
end

function AfficherKeys(pTile, pKey, pText)
    for l = 1, nb_ligne do
        for c = 1, nb_col do
            if grid[l][c] == pTile then
                love.graphics.draw(KeyboardSheet, pKey, (l - 1) * largeurGrid, (c - 1) * HauteurGrid)
            end
            if grid[l][c] == pTile + 1 then
                love.graphics.print(tostring(pText), (l - 1) * largeurGrid, (c - 1) * HauteurGrid)
            end
        end
    end
end

function Menu.Draw()
    Maps.AfficheLayers(Layers.background)
    Maps.AfficheLayers(Layers.walls)
    Maps.AfficheLayers(Layers.decor)

    love.graphics.draw(
        Img_title,
        love.graphics.getWidth() / 2,
        love.graphics.getHeight() / 2,
        0,
        1,
        1,
        Img_title:getWidth() / 2,
        Img_title:getHeight() / 2
    )

    if pressStart == true then
        Maps.AfficheLayers(Layers.Animation)
        love.graphics.draw(
            Img_Press_enter,
            love.graphics.getWidth() / 2,
            love.graphics.getHeight() / 2 + hauteurImg_title / 2,
            0,
            1,
            1,
            Img_Press_enter:getWidth() / 2,
            Img_Press_enter:getHeight() / 2
        )
        love.graphics.setColor(1, 1, 1)
    end

    AfficherKeys(239, Key_Z, "Avancer")
    AfficherKeys(186, Key_Q, "Gauche")
    AfficherKeys(241, Key_S, "Reculer")
    AfficherKeys(294, Key_D, "Droite")
    AfficherKeys(429, Key_V, "IEM")
    AfficherKeys(483, Key_C, "Commandes")
    AfficherKeys(564, Key_B, "Bouclier")
    AfficherKeys(645, Key_U, "Volume +")
    AfficherKeys(699, Key_I, "Volume -")

    for l = 1, nb_ligne do
        for c = 1, nb_col do
            if grid[l][c] == 780 then
                love.graphics.draw(Mouse, (l - 1) * largeurGrid, (c - 1) * HauteurGrid)
            end
            if grid[l][c] == 781 then
                love.graphics.print(tostring("Tirer"), (l - 1) * largeurGrid, (c - 1) * HauteurGrid)
            end
        end
    end

    for l = 1, nb_ligne do
        for c = 1, nb_col do
            -- love.graphics.print(tostring(grid[l][c]), (l - 1) * largeurGrid, (c - 1) * HauteurGrid)
        end
    end
end

return Menu
