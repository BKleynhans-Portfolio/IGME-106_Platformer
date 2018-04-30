Hi Ash,

The project you want to run is in this folder

team-project\Game1



If the project complains about anything it would be the paths (using relative paths) relating to reading and writing levels.  This configuration is located in Game1.cs which you can update in the following lines

You can update where the levels are located by updating the "ReadLevelFile" method.  The lines you want to update are 763 and 765.

You can also update where the auto level generator is writing to in the "CreateTempLevels" method.  The line you are interested in is 1156.