Win = {}

function Win.Load()
    Img_bck = love.graphics.newImage("Images/CommandeBck.png")
    largeurImg_bck = Img_bck:getWidth()
    hauteurImg_bck = Img_bck:getHeight()
    Img_WIN = love.graphics.newImage("Images/Victory.png")
end

function Win.Update(dt)
end

function Win.Draw()
    love.graphics.draw(
        Img_bck,
        love.graphics.getWidth() / 2,
        love.graphics.getHeight() / 2,
        0,
        1,
        1,
        largeurImg_bck / 2,
        hauteurImg_bck / 2
    )

    love.graphics.draw(
        Img_WIN,
        love.graphics.getWidth() / 2,
        love.graphics.getHeight() / 2,
        0,
        1 / 2,
        1 / 2,
        Img_WIN:getWidth() / 2,
        Img_WIN:getHeight() / 2
    )
end

return Win
