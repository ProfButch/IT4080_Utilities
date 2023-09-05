# IT4080_Utilities

# Install
1.  [Download the zip](https://github.com/ProfButch/IT4080_Utilities/archive/refs/heads/main.zip).
1.  Extract the zip.
1.  Open the extracted folder.
1.  Copy the `Assets/IT4080` folder from the extracted zip folder to your `Assets` folder.

# IT4080 Menu
This menu contains utilities to make building your game and running multiple instances of the game much easier.  Once you set your build path, you can quickly create a new build and run multiple instances.  Each instance gets its very own log file.

This menu works for Windows and Mac.  Linux support would be great, PRs welcomed.

__WARNING__:  Log files are named `<build path>/<build name>_<x>.log` and are reused each time you launch an instance of your game.  If you use any of the `Run` commands while instances are still running, they will write to the same log file.  For example, if you click `Run->1`, then `Run->1` again (without closing the first instance launched), then both instances will be writing to the same log.  The contents of `<build name>_1.log` will be total chaos.

## Menu Items
* `Set Build Path` - Sets the build path and build name that will be used for all other build commands that are executed.  Log files will also be placed in this directory.  No more dialogs when building.  Just click build and it builds to the last place, no questions asked.  This path is saved in the `EditorPrefs` so you never have to set it again if you don't want to.<br/>For example, setting it to `/users/you/temp/TheBuild` on a Mac will result in the following files being made when you use `Build and Run -> 2`:
    * `/users/you/temp/TheBuild.app`
    * `/users/you/temp/TheBuild_1.log`
    * `/users/you/temp/TheBuild_2.log`
* `Build` - Builds the project to the path set by Build Path.
* `Build Current Scene` - This will build the project the same as Build but it will use the current scene as the first scene run by the build.  This only affects the build that is generated.  It does not change the order of the scenes in BuildSettings.  Use the `Run <x>` menus to run this build (as `Build and Run` always does a normal build)
* `Build and Run <x>` - Performs a build then will launch 1-4 instances of the built project.  See `Run <x>` for more info.
* `Run <x>` - Runs 1-4 instances of the last build.  This will create a logfile in the same directory as the build.  The logfile name will be `<build name>_<x>.log`, where x is 1-4.  Logs are rewritten each time a run occurs.  Logs are not deleted.
* `Show Files` - This will open up the `Build Path` in Finder or Explorer.
* `About` - It prints info.  It uses `Debug.Log`.  It doesn't print much.  That's what it's all about.
