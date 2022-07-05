/// parseAsset(assetType, assetName)
var assetType = argument[0];
var assetName = argument[1];
switch (assetType)
{
    case "object":
        if (!ds_map_exists(project_objects,assetName))
        {
            
            var sprData = RET_GetObjectSpriteData(global.projectLocation + "*" + assetName + "*" + game_save_id);
            show_debug_message(string(sprData));
            if (sprData != "*error" && sprData != "*nosprite")
            {
                sprData = stringSplit(sprData,"*",false);
                show_debug_message(sprData);
                parseAsset("sprite",sprData[0],sprData[1],sprData[2])
            }
            else
            {
                sprData = makeArray("",0,0,0);
            }
            ds_map_add(project_objects,assetName,makeArray(sprData[0],sprData[3]));
            
            
        }
    break;
    case "sprite":
        if (!ds_map_exists(project_sprites,assetName))
        {
            
            ds_map_add(project_sprites,assetName,sprite_add("sprites/images/" + assetName + "_0.png",1,false,false,argument[2],argument[3]));
            
        }
    break;
    case "tile":
    case "background"://Includes both types.
        if (!ds_map_exists(project_backgrounds,assetName))
        {
            var backData = RET_GetBackground(global.projectLocation + "*" + assetName + "*" + game_save_id);
            if (backData != "*error")
            {
                //show_debug_message(backData);
                backData = stringSplit(backData,"*",false);
                ds_map_add(project_backgrounds,backData[0],background_add("background/images/" + assetName + ".png",false,false));
                ds_map_add(project_backgrounds_tileData,backData[0],makeArray(backData[1],backData[2],backData[3],backData[4],backData[5],backData[6]));
                ds_map_add(project_backgrounds_reverseLookup,string(project_backgrounds[? backData[0]]), backData[0]);
            }
        }
    break;
    
    
    
}
