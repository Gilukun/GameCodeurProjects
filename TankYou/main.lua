-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end
-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

-- LOVE2D functions
function math.angle(x1, y1, x2, y2)
    return math.atan2(y2 - y1, x2 - x1)
end
function math.dist(x1, y1, x2, y2)
    return ((x2 - x1) ^ 2 + (y2 - y1) ^ 2) ^ 0.5
end
function CheckCollision(x1, y1, w1, h1, x2, y2, w2, h2)
    return x1 < x2 + w2 and x2 < x1 + w1 and y1 < y2 + h2 and y2 < y1 + h1
end

local Menu = require("Menu")
local Cartes = require("maps")
local Player = require("Hero")
local Ennemy = require("Ennemis")
local Weapons = require("Weapons")
local GUI = require("GUI")
local Loot = require("Loot")
local Pause = require("Pause")
local GameOver = require("GameOver")
local Sounds = require("Sounds")
local Victoire = require("Win")
local Commandes = require("Commandes")

-- ETATS DU JEU
GameState = {}
GameState.Menu = "MENU"
GameState.level1 = "LEVEL1"
GameState.Pause = "PAUSE"
GameState.Commandes = "Commandes"
GameState.GameOver = "GAMEOVER"
GameState.Quit = "QUIT"
GameState.WIN = "WIN"

G_State = GameState.Menu

local backtomenu = 20
local timerbacktomenu = backtomenu

function love.load()
    love.window.setMode(1024, 1024)
    Menu.Load()
    Pause.Load()
    Cartes.Load()
    Sounds.Load()
    Player.Load()
    Ennemy.Load()
    Weapons.Load()
    Loot.Load()
    GUI.Load()
    GameOver.Load()
    Victoire.Load()
    Commandes.Load()
end

-- SECTION UPDATES
function UpdateMenu(dt)
    Menu.Update(dt)
end

function UpdateLevel1(dt)
    Cartes.Update(dt)

    Player.Move(dt)

    Weapons.Obus(dt)
    Weapons.EMI(dt)
    Ennemy.Update(dt)

    Weapons.Shield(dt)
    Weapons.Update(dt)
    Loot.Update(dt)
    Loot.GameWin()
end

function UpdatePause()
end

function UpdateCommandes()
end

function UpdateGameOver(dt)
    GameOver.Update(dt)
end

-- UPDATE GENERAL
function love.update(dt)
    if G_State == GameState.Menu then
        UpdateMenu(dt)
        Sd_Menu:play()
        Sd_WIN:stop()
    elseif G_State == GameState.level1 then
        UpdateLevel1(dt)
        Sd_Lvl1:play()
    elseif G_State == GameState.GameOver then
        Sd_Lvl1:stop()
        Sd_GAMEOVER:play()
        UpdateGameOver(dt)
    elseif G_State == GameState.WIN then
        Sd_Lvl1:stop()
        Sd_WIN:play()
        timerbacktomenu = timerbacktomenu - dt
        if timerbacktomenu <= 0 then
            G_State = GameState.Menu
        end
    end
end

-- SECTION DRAW

function DrawMenu()
    Menu.Draw()
end

function DrawLevel1()
    Cartes.Draw()
    Player.Draw()
    Ennemy.Draw()
    Loot.Draw()
    Weapons.Draw()
    GUI.Draw()
end

function DrawCommandes()
    Commandes.Draw()
end

function DrawWIN()
    Victoire.Draw()
end

function DrawGameOver()
    GameOver.Draw()
end

function DrawPause()
    Pause.Draw()
end

function love.draw()
    if G_State == GameState.Menu then
        DrawMenu()
    elseif G_State == GameState.level1 then
        DrawLevel1()
    elseif G_State == GameState.Pause then
        DrawLevel1()
        DrawPause()
    elseif G_State == GameState.Boss then
        DrawLevel1()
    elseif G_State == GameState.Commandes then
        DrawCommandes()
    elseif G_State == GameState.GameOver then
        DrawLevel1()
        DrawGameOver()
    elseif G_State == GameState.WIN then
        DrawWIN()
    end
end

-- SECTION ACTION/CLAVIER
function love.keypressed(key)
    -- actions possible sur le menu du jeu
    if G_State == GameState.Menu then
        -- actions possible pendant le premier niveau
        if key == "return" then
            -- initialisation du niveau 1
            Player.Start()
            Ennemy.Start()
            Weapons.Start()
            Loot.Start()
            GUI.Start()
            Sd_Menu:stop()
            G_State = GameState.level1
        end
        -- ajustement du volume general
        if key == "u" then
            Sounds.LevelUp()
        end
        if key == "i" then
            Sounds.LevelDown()
        end
    elseif G_State == GameState.level1 then
        -- actions possible dans le menu "Pause"
        if key == "p" then
            G_State = GameState.Pause
        end

        if key == "c" then
            G_State = GameState.Commandes
        end
        -- activation de l'IEM
        if key == "v" then
            Weapons.Type(W_Style.ATTACK)
        end
        -- activation du Bouclier
        if ShieldActiveWidth >= 50 then
            if key == "b" then
                Weapons.Type(W_Style.SHIELD)
                Sd_Shield:play()
            end
        end
        if key == "escape" then
            Sd_Lvl1:stop()
            G_State = GameState.Menu
        end
        if key == "u" then
            Sounds.LevelUp()
        end
        if key == "i" then
            Sounds.LevelDown()
        end
    elseif G_State == GameState.Pause then
        -- actions possible dans le menu "Commandes"
        if key == "p" then
            G_State = GameState.level1
        end
        if key == "u" then
            Sounds.LevelUp()
        end
        if key == "i" then
            Sounds.LevelDown()
        end
    elseif G_State == GameState.Commandes then
        -- actions possibles sur la page de "Game Over"
        if key == "c" then
            G_State = GameState.level1
        end
        if key == "u" then
            Sounds.LevelUp()
        end
        if key == "i" then
            Sounds.LevelDown()
        end
    elseif G_State == GameState.GameOver then
        -- action possibles sur la page de "Victoire"
        Sd_Lvl1:stop()
        if key == "return" then
            G_State = GameState.Menu
            Sd_GAMEOVER:stop()
        end
        if key == "u" then
            Sounds.LevelUp()
        end
        if key == "i" then
            Sounds.LevelDown()
        end
    elseif G_State == GameState.WIN then
        if key == "return" then
            G_State = GameState.Menu
            Sd_WIN:stop()
        end
        if key == "u" then
            Sounds.LevelUp()
        end
        if key == "i" then
            Sounds.LevelDown()
        end
    end
end

function love.mousepressed(x, y, button)
    if G_State == GameState.level1 then
        Player.mousepressed(x, y, button)
    end
end
