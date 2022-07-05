/// editorCreateMenu(menuName,dir,sidePos,width,height)
var inst = instance_create(x,y,objEditorMenu);
with (inst)
{
    name = argument[0];
    dir = argument[1];
    width = argument[3];
    height = argument[4];
    if (dir == "left" || dir == "right")
    {
        yPos = argument[2]-height/2;
        xPos = view_wview-hangMargin;
    }
    else
    {
        xPos = argument[2]-width/2;
        yPos = view_hview-hangMargin;
    }
    parent = other;
    event_user(0);
}
return inst;
