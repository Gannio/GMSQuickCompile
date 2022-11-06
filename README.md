# GMSQuickCompile
This is a program meant for launching GMS1.x projects without the need to launch the program within GMS itself, which causes large loading times for large projects.

It works by executing a process in a game, using a DLL made specifically for this purpose to do so. Apparently, because the game is connected to GMS via the Runner, it considers commands from said game valid in regards to the compiler's DRM (Even if the shell is made seperate).

For the same reason, this program will *not* work if you try to compile the code to an exe, as then it's seperated from GMS. As such, there is no release build as it'd be pointless.

The program also includes a 'run last compile' option after compiling at least once. As what seems to be a quirk of executing commands from within GMS, the FMOD extension used by a number of games will misidentify the local location as the Runner.exe's location if ran in the same manner. As a workaround, a .bat is created with each file to execute the code seperately from the GMS's own control. This also allows re-launching a compiled build even after the program shuts down, as the user can simply start said .bat file.

The runner also allows you to redirect into older compiler EXE's (e.g. You can compile a game as though it was compiled for 1.4.1804, even if the program you're using to run QuickCompile is 1.4.9999).
