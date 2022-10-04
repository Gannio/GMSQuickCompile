/// editorCreateMenu(menuName,dir,sidePos,width,height,color, outlineColor)
var inst = instance_create(x,y,objEditorMenu);
with (inst)
{
    name = argument[0];
    dir = argument[1];
    width = argument[3];
    height = argument[4];
    if (dir == "right")
    {
        yPos = argument[2]-height/2;
        xPos = view_wview-hangMargin;
    }
    else if (dir == "left")
    {
        yPos = argument[2]-height/2;
        xPos = -width+hangMargin;
    }
    else
    {
        xPos = argument[2]-width/2;
        yPos = view_hview-hangMargin;
    }
    parent = other;
    menuColor = argument[5];
    menuOutlineColor = argument[6];
    event_user(0);
}
return inst;
