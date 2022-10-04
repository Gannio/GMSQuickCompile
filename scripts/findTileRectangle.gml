/// findTile(left,top,right,bottom,criteria,[depth])
//Finds a set of tiles in a rectangle. Is *very* inefficient for all tiles and should probably be improved.

var mX = argument[0];
var mY = argument[1];
var mR = argument[2];
var mB = argument[3];
var tiles = -1;
if (argument_count > 5)
{
    tiles = tile_get_ids_at_depth(argument[5]);
}
else
{
    tiles = tile_get_ids();
}
var returnTiles = array_create(0);
var a = 0;
for (var i = array_length_1d(tiles)-1; i >= 0; i--)//Go backwards so we get highest tile order (last drawn and tf frontmost tile) selected first.
{
    var X = tile_get_x(tiles[i]);
    var Y = tile_get_y(tiles[i]);
    var W = tile_get_width(tiles[i]);
    var H = tile_get_height(tiles[i]);
    if (rectangle_in_rectangle(X,Y,X+W,Y+H,mX,mY,mR,mB) == argument[4])//point_in_rectangle(mX,mY,X,Y,X+W,Y+H))
    {
        returnTiles[a++] = tiles[i];//return tiles[i];
    }
}
return returnTiles;