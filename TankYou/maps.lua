local Maps = {}

local tiled = require("Level1")

MAP_WIDTH = 32 -- nombre de colonne
MAP_HEIGHT = 20 -- nombre de ligne
TILE_WIDTH = 32 -- largeur de la tile
TILE_HEIGHT = 32 -- Hauteur de la tile

list_Layers = {
  background = tiled.layers[1].data,
  floor = tiled.layers[2].data,
  walls = tiled.layers[3].data,
  grass = tiled.layers[4].data,
  trees = tiled.layers[5].data,
  decor = tiled.layers[6].data
}

Tilesheet = {}
TileTextures = {}

TileTypes = {}
TileTypes[1] = "grass"
TileTypes[650] = "Wall"

local flash = 2
local timer = flash
local flashDecor = true

local flash2 = 3
local timer2 = flash2
local flashDecor3 = true

local flash3 = 1.5
local timer3 = flash3
local flashDecor3 = true

function Maps.Load()
  Tilesheet = love.graphics.newImage("Images/TileSet1.png")
  Largeurtilesheet = Tilesheet:getWidth()
  HauteurTilesheet = Tilesheet:getHeight()

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

function Maps.Update(dt)
  if timer > 1 then
    flashDecor = true
    timer = timer - dt
  elseif timer < 1 then
    timer = timer - dt
    flashDecor = false
  elseif timer <= 0 then
    timer = flash
  end
end

function Maps.AfficheLayers(Layer)
  local nbligne = #Layer / TILE_WIDTH
  local nbcol = TILE_HEIGHT
  local l, c
  for l = nbligne, 1, -1 do
    for c = 1, nbcol do
      local tuile = Layer[((l - 1) * TILE_WIDTH) + c]
      if tuile > 0 then
        love.graphics.draw(Tilesheet, TileTextures[tuile], (c - 1) * TILE_WIDTH, (l - 1) * TILE_HEIGHT)
      end
    end
  end
end

function Maps.CollisionsLayers(pX, pY, pWidth, pHeight)
  local nbligne = #list_Layers.walls / TILE_WIDTH
  local nbcol = TILE_HEIGHT
  local l, c
  for l = nbligne, 1, -1 do
    for c = 1, nbcol do
      local tuile = list_Layers.walls[((l - 1) * TILE_HEIGHT) + c]
      if tuile > 0 then
        if CheckCollision(pX, pY, pWidth, pHeight, c * TILE_WIDTH, l * TILE_HEIGHT, TILE_WIDTH, TILE_HEIGHT) then
          return true
        end
      end
    end
  end
  return false
end

function Maps.Draw()
  Maps.AfficheLayers(list_Layers.background)
  Maps.AfficheLayers(list_Layers.floor)
  Maps.AfficheLayers(list_Layers.walls)
  Maps.AfficheLayers(list_Layers.grass)
  Maps.AfficheLayers(list_Layers.trees)
  Maps.AfficheLayers(list_Layers.decor)
  -- local x = love.mouse.getX()
  ---local  y = love.mouse.getY()
  --- local col = math.floor(x / TILE_WIDTH) + 1
  --local lig = math.floor(y / TILE_HEIGHT) + 1
  -- if col > 0 and col<= MAP_WIDTH and lig> 0 and lig <= MAP_WIDTH then
  -- local id = Maps.background[lig][col]
  -- love.graphics.print("id=" .. tostring(Maps.TileTypes[id]))
  --- end
end

return Maps
