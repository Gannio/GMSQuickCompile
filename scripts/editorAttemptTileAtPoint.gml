/// editorAttemptTileAtPoint
var tX = floor(argument[0]/16)*16;
var tY = floor(argument[1]/16)*16;
/*with (instance_create((argument[0]),(argument[1]),objDummy))
{
    move_snap(16,16);
    tX = x;
    tY = y;
    instance_destroy();
}*/
if (findTile(tX+8,tY+8,tileSelector_Depth) == noone)
{
    tile_add(tileSelector_BG,tileSelector_Left,tileSelector_Top,tileSelector_Width,tileSelector_Height,tX,tY,tileSelector_Depth);
}