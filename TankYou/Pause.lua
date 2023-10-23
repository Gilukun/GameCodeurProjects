Pause = {}

function Pause.Load()
    Img_Pause = love.graphics.newImage("Images/pause.jpeg")
end

function Pause.Draw()
    love.graphics.setColor(1, 1, 1)
    love.graphics.draw(
        Img_Pause,
        love.graphics.getWidth() / 2,
        love.graphics.getHeight() / 2,
        0,
        1,
        1,
        Img_Pause:getWidth() / 2,
        Img_Pause:getHeight() / 2
    )
    love.graphics.setColor(1, 1, 1)
end

return Pause
