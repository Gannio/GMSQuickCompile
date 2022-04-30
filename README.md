# GMSQuickCompile
This is a program meant for launching GMS1.x projects without the need to launch the program within GMS itself, which causes large loading times due to how giant GMS itself is.

It works by executing a process in a game, using a DLL made specifically for this purpose to do so. Apparently, because the game is connected to GMS via the Runner, it considers commands from said game valid in regards to the compiler's DRM (Even if the shell is made seperate).

For the same reason, this program will *not* work if you try to compile the code to an exe, as then it's seperated from GMS. As such, there is no release build as it'd be pointless.
