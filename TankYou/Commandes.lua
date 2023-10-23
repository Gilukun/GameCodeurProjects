Commandes = {}

function Commandes.Load()
    Img_Commandebck = love.graphics.newImage("Images/CommandeBck.png")
    largeurImgCommandebck = Img_Commandebck:getWidth()
    hauteurImgCommandebck = Img_Commandebck:getHeight()
    Img_Keys = love.graphics.newImage("Images/Commandes.png")
    largeurImg_Keys = Img_Keys:getWidth()
    hauteurImg_Keys = Img_Keys:getHeight()
end

function Commandes.Update(dt)
end

function Commandes.Draw()
    love.graphics.draw(
        Img_Commandebck,
        love.graphics.getWidth() / 2,
        love.graphics.getHeight() / 2,
        0,
        1,
        1,
        largeurImgCommandebck / 2,
        hauteurImgCommandebck / 2
    )
    love.graphics.draw(
        Img_Keys,
        love.graphics.getWidth() / 2,
        love.graphics.getHeight() / 2,
        0,
        1,
        1,
        largeurImg_Keys / 2,
        hauteurImg_Keys / 2
    )
end

return Commandes
