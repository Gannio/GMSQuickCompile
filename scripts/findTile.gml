/// findTile(x,y,[depth])
//Finds a tile at a specific x and y position. Is *very* inefficient for all tiles and should probably be improved.

var mX = argument[0];
var mY = argument[1];
var tiles = -1;
if (argument_count > 2)
{
    tiles = tile_get_ids_at_depth(argument[2]);
}
else
{
    tiles = tile_get_ids();
}
var size = array_length_1d(tiles);
if (size > 0 && tile_exists(tiles[0]))
{
    for (var i = size-1; i >= 0; i--)//Go backwards so we get highest tile order (last drawn and tf frontmost tile) selected first.
    {
        var X = tile_get_x(tiles[i]);
        var Y = tile_get_y(tiles[i]);
        var W = tile_get_width(tiles[i]);
        var H = tile_get_height(tiles[i]);
        if (point_in_rectangle(mX,mY,X,Y,X+W,Y+H))
        {
            return tiles[i];
        }
    }
}
return noone;
