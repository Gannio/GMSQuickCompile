/// editorPerformEditingActions(forceShift,disableCreation,overrideX,overrideY)
var forceShift = false;
var mouseX = mouse_x;
var mouseY = mouse_y;
var disableCreation = false;
if (argument_count >= 1)
{
    forceShift = argument[0];
    disableCreation = argument[1];
    mouseX = argument[2];
    mouseY = argument[3];
}

switch (editingMode)
{
    case editModes.OBJECTS:

        var addedNewObject = false;
        var foundObject = collision_point(mouseX,mouseY,objEditorObject,false,false)
        var isOnEdge = -1;
        if (foundObject)
        {
            if (ds_list_size(selectedObjects) <= 1 && ds_list_find_value(selectedObjects,0) == foundObject)
            {
                with (foundObject)
                {
                    var range = 4;
                    var right = rectangle_in_rectangle(mouseX-range,mouseY-range,mouseX+range,mouseY+range,bbox_right,bbox_top,bbox_right,bbox_bottom) > 0//Right box.
                    var up = rectangle_in_rectangle(mouseX-range,mouseY-range,mouseX+range,mouseY+range,bbox_left,bbox_top,bbox_right,bbox_top) > 0//Top box.
                    var left = rectangle_in_rectangle(mouseX-range,mouseY-range,mouseX+range,mouseY+range,bbox_left,bbox_top,bbox_left,bbox_bottom) > 0//Left.
                    var down = rectangle_in_rectangle(mouseX-range,mouseY-range,mouseX+range,mouseY+range,bbox_left,bbox_bottom,bbox_right,bbox_bottom) > 0//Bottom.
                    
                    var X = right+-1*left;
                    var Y = down+-1*up;
                    if !(X == 0 && Y == 0)
                    {
                        isOnEdge = round(point_direction(0,0,X,Y)/45)%8;
                        show_debug_message(X);
                        show_debug_message(Y);
                        
                    }
                    show_debug_message(isOnEdge);
                }
                
                
                
            }
        }
        if (isOnEdge >= 0)
        {
            var cursors = makeArray(cr_size_we,cr_size_nesw,cr_size_ns,cr_size_nwse,cr_size_we,cr_size_nesw,cr_size_ns,cr_size_nwse);
            
            window_set_cursor(cursors[isOnEdge]);
            
        }
        else if (window_get_cursor() != cr_default)
        {
            window_set_cursor(cr_default);
        }
        
        if (global.mouseLeftPressed)
        {
            //if (selectedObject == noone)
            //{
            //selectedObject = collision_point(mouseX,mouseY,objEditorObject,false,false)
            //}
            
            if (foundObject == noone)
            {
                if (!disableCreation)
                {
                    show_debug_message("Todo: Create");
                }
            }
            else
            {
                
                
                if !(keyboard_check(vk_shift) || forceShift)
                {
                    ds_list_clear(selectedObjects);
                    ds_list_clear(selectedObjects_XOffset);
                    ds_list_clear(selectedObjects_YOffset);
                    
                    
                    holdingType = isOnEdge+1;
                    holding_ScaleX = foundObject.image_xscale;
                    holding_ScaleY = foundObject.image_yscale;
                    
                }
                
                holdingObject = true;
                addedNewObject = true;
                for (var i = 0; i < ds_list_size(selectedObjects); i++)
                {
                    var o = ds_list_find_value(selectedObjects,i);
                    ds_list_set(selectedObjects_XOffset,i,o.x-mouseX);
                    ds_list_set(selectedObjects_YOffset,i,o.y-mouseY);
                }
                var index = ds_list_find_index(selectedObjects,foundObject);
                if (index < 0)
                {
                    ds_list_add(selectedObjects,foundObject);
                    ds_list_add(selectedObjects_XOffset, foundObject.x-mouseX);
                    ds_list_add(selectedObjects_YOffset, foundObject.y-mouseY);
                    
                }
                else if (keyboard_check(vk_shift) || forceShift)
                {
                    ds_list_delete(selectedObjects,index);
                    ds_list_delete(selectedObjects_XOffset,index);
                    ds_list_delete(selectedObjects_YOffset,index);
                }
                
            }
        }
        else if (global.mouseLeftReleased)
        {
            holdingObject = false;//selectedObject = noone;
            holdingType = 0;
        }
        else if (global.mouseLeft)
        {
            for (var i = 0; i < ds_list_size(selectedObjects); i++)
            {
                var selectedObject = ds_list_find_value(selectedObjects,i);
                var xOffset = ds_list_find_value(selectedObjects_XOffset,i);
                var yOffset = ds_list_find_value(selectedObjects_YOffset,i);
                if (instance_exists(selectedObject) && holdingObject)
                {
                    switch (holdingType)
                    {
                        case 0:
                            selectedObject.x = mouseX+xOffset;
                            selectedObject.y = mouseY+yOffset;
                            with (selectedObject)
                            {
                                move_snap(gridsnap_X,gridsnap_Y);
                            }
                        break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            var ht = holdingType;
                            with (selectedObject)
                            {
                                
                                
                                
                                
                                var xs = 0;
                                var ys = 0;
                                if (ht < 3 || ht > 7)
                                {
                                    xs = round(((mouseX+xOffset)-(x - sprite_get_xoffset(sprite_index)))/(sprite_get_width(sprite_index))) + other.holding_ScaleX;
                                }
                                else if (ht > 3 && ht < 7)
                                {
                                    xs = -1*round(((mouseX+xOffset)-(x + sprite_get_xoffset(sprite_index)))/(sprite_get_width(sprite_index))) + other.holding_ScaleX;
                                }
                                
                                if (ht > 1 && ht < 5)
                                {
                                    ys = -1*round(((mouseY+yOffset)-(y + sprite_get_yoffset(sprite_index)))/(sprite_get_height(sprite_index))) + other.holding_ScaleY;
                                }
                                else if (ht > 5)
                                {
                                    //show_debug_message("V");
                                    ys = round(((mouseY+yOffset)-(y - sprite_get_yoffset(sprite_index)))/(sprite_get_height(sprite_index))) + other.holding_ScaleY;
                                }
                                //var ys = 
                                if (sign(xs) != 0 && ht != 3 && ht != 7)
                                {
                                    image_xscale = xs;
                                }
                                if (sign(ys) != 0 && ht != 1 && ht != 5)
                                {
                                    image_yscale = ys;
                                }
                            }
                        break;
                    }
                }
            }
        }
    break;
    case editModes.TILES:
        //show_debug_message("TILES");
        var addedNewObject = false;
        
        var foundTile = findTile(mouseX,mouseY,tileSelector_Depth);//collision_point(mouseX,mouseY,objEditorObject,false,false)
        var isOnEdge = -1;
        if (foundTile >= 0)
        {
            if (ds_list_size(selectedObjects) <= 1 && ds_list_find_value(selectedObjects,0) == foundTile)
            {
                //with (foundTile)
                //{
                
                var range = 4;
                
                var bb_left = tile_get_x(foundTile);
                var bb_top = tile_get_x(foundTile);
                var bb_right = bb_left+tile_get_width(foundTile);
                var bb_bottom = bb_top+tile_get_height(foundTile);
                
                var right = rectangle_in_rectangle(mouseX-range,mouseY-range,mouseX+range,mouseY+range,bb_right,bb_top,bb_right,bb_bottom) > 0//Right box.
                var up = rectangle_in_rectangle(mouseX-range,mouseY-range,mouseX+range,mouseY+range,bb_left,bb_top,bb_right,bb_top) > 0//Top box.
                var left = rectangle_in_rectangle(mouseX-range,mouseY-range,mouseX+range,mouseY+range,bb_left,bb_top,bb_left,bb_bottom) > 0//Left.
                var down = rectangle_in_rectangle(mouseX-range,mouseY-range,mouseX+range,mouseY+range,bb_left,bb_bottom,bb_right,bb_bottom) > 0//Bottom.
                
                var X = right+-1*left;
                var Y = down+-1*up;
                if !(X == 0 && Y == 0)
                {
                    isOnEdge = round(point_direction(0,0,X,Y)/45)%8;
                    show_debug_message(X);
                    show_debug_message(Y);
                    
                }
                show_debug_message(isOnEdge);
                //}
                
                
                
            }
        }
        if (isOnEdge >= 0)
        {
            var cursors = makeArray(cr_size_we,cr_size_nesw,cr_size_ns,cr_size_nwse,cr_size_we,cr_size_nesw,cr_size_ns,cr_size_nwse);
            
            window_set_cursor(cursors[isOnEdge]);
            
        }
        else if (window_get_cursor() != cr_default)
        {
            window_set_cursor(cr_default);
        }
        
        if (global.mouseLeftPressed)
        {
            //if (selectedObject == noone)
            //{
            //selectedObject = collision_point(mouseX,mouseY,objEditorObject,false,false)
            //}
            
            if (foundTile == noone)
            {
                if (!disableCreation)
                {
                    editorAttemptTileAtPoint(mouseX,mouseY);//show_debug_message("Todo: Create");
                }
            }
            else
            {
                
                
                if !(keyboard_check(vk_shift) || forceShift)
                {
                    ds_list_clear(selectedObjects);
                    ds_list_clear(selectedObjects_XOffset);
                    ds_list_clear(selectedObjects_YOffset);
                    
                    
                    holdingType = isOnEdge+1;
                    holding_ScaleX = tile_get_xscale(foundTile);//foundTile.image_xscale;
                    holding_ScaleY = tile_get_yscale(foundTile);//foundTile.image_yscale;
                    
                }
                
                holdingObject = true;
                addedNewObject = true;
                for (var i = 0; i < ds_list_size(selectedObjects); i++)
                {
                    var o = ds_list_find_value(selectedObjects,i);
                    ds_list_set(selectedObjects_XOffset,i,tile_get_x(o)-mouseX);
                    ds_list_set(selectedObjects_YOffset,i,tile_get_y(o)-mouseY);
                }
                var index = ds_list_find_index(selectedObjects,foundTile);
                if (index < 0)
                {
                    ds_list_add(selectedObjects,foundTile);
                    ds_list_add(selectedObjects_XOffset, tile_get_x(foundTile)-mouseX);
                    ds_list_add(selectedObjects_YOffset, tile_get_y(foundTile)-mouseY);
                    
                }
                else if (keyboard_check(vk_shift) || forceShift)
                {
                    ds_list_delete(selectedObjects,index);
                    ds_list_delete(selectedObjects_XOffset,index);
                    ds_list_delete(selectedObjects_YOffset,index);
                }
                
            }
        }
        else if (global.mouseLeftReleased)
        {
            holdingObject = false;//selectedObject = noone;
            holdingType = 0;
        }
        else if (global.mouseLeft)
        {
            var size = ds_list_size(selectedObjects);
            if (size > 0)
            {
                for (var i = 0; i < size; i++)
                {
                    var selectedObject = ds_list_find_value(selectedObjects,i);
                    var xOffset = ds_list_find_value(selectedObjects_XOffset,i);
                    var yOffset = ds_list_find_value(selectedObjects_YOffset,i);
                    if (tile_exists(selectedObject) && holdingObject)
                    {
                        switch (holdingType)
                        {
                            case 0:
                                
                                with (instance_create((mouseX+xOffset),(mouseY+yOffset),objDummy))
                                {
                                    move_snap(gridsnap_X,gridsnap_Y);
                                    tile_set_position(selectedObject,x,y);
                                    instance_destroy();
                                }
                                //with (selectedObject)
                                //{
                                //    
                                //}
                            break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                                var ht = holdingType;
                                
                                
                                var X = tile_get_x(selectedObject);
                                var Y = tile_get_y(selectedObject);
                                var W = tile_get_width(selectedObject);
                                var H = tile_get_height(selectedObject);
                                
                                var xs = 0;
                                var ys = 0;
                                if (ht < 3 || ht > 7)
                                {
                                    xs = round(((mouseX+xOffset)-(X))/W) + holding_ScaleX;
                                }
                                else if (ht > 3 && ht < 7)
                                {
                                    xs = -1*round(((mouseX+xOffset)-(X))/W) + holding_ScaleX;
                                }
                                
                                if (ht > 1 && ht < 5)
                                {
                                    ys = -1*round(((mouseY+yOffset)-(Y))/H) + holding_ScaleY;
                                }
                                else if (ht > 5)
                                {
                                    //show_debug_message("V");
                                    ys = round(((mouseY+yOffset)-(Y))/H) + holding_ScaleY;
                                }
                                //var ys = 
                                if (sign(xs) != 0 && ht != 3 && ht != 7)
                                {
                                    tile_set_scale(selectedObject,xs,tile_get_yscale(selectedObject));
                                }
                                if (sign(ys) != 0 && ht != 1 && ht != 5)
                                {
                                    tile_set_scale(selectedObject,tile_get_xscale(selectedObject),ys);//image_yscale = ys;
                                }
                                
                            break;
                        }
                    }
                }
            }
            else
            {
                if (keyboard_check(vk_shift) && !disableCreation)
                {
                    editorAttemptTileAtPoint(mouseX,mouseY);
                }
            }
        }
    break;
}
