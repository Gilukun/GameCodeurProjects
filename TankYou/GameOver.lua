GameOver = {}

local flash = 2
local timer = flash
local flashGameOver = true

function GameOver.Load()
    Img_GameOverTop = love.graphics.newImage("Images/GameOver.png")
end

function GameOver.Update(dt)
    if timer > 1 then
        flashGameOver = true
        timer = timer - dt
    end
    if timer < 1 then
        timer = timer - dt
        flashGameOver = false
    end
    if timer <= 0 then
        timer = flash
    end
end

function GameOver.Draw()
    if flashGameOver == true then
        love.graphics.draw(
            Img_GameOverTop,
            love.graphics.getWidth() / 2,
            love.graphics.getHeight() / 2,
            0,
            1,
            1,
            Img_GameOverTop:getWidth() / 2,
            Img_GameOverTop:getHeight() / 2
        )
    end
end

return GameOver
