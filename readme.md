# Immortal Snail Locator

The program current displays a map of the world with features such as sea, land, rivers, lakes, country borders, and state/province borders. A cyan dot, representing you, should position itself on the map based on your IRL position. A red dot, representing an immortal snail, is positioned on the map, and it navigates towards you, at super sonic speeds (1,000,000.0 m/s), taking earth's curvature into consideration. 

## Instructions for Build and Use

Steps to build and/or run the software:

1. Open the project in Godot.
2. Open main_scene.tscn in Godot's File system, double click to open.
3. Once the main_scene is loaded, press the "Run Current Scene" button near the top right corner of the editor.

Instructions for using the software:

1. You move IRL as the program runs and your dot will go to your location on the map, you cannot outrun the snail.
2. Click the x button on the debug window to close it and end the program.

## Development Environment

To recreate the development environment, you need the following software and/or libraries with the specified versions:

* Windows 11
* .Net 8
* VSCode 1.116
* Godot 4.6.stable.mono .Net

## Useful Websites to Learn More

I found these websites useful in developing this software:

* Gemini (https://gemini.google.com/)
* Natural Earth (https://www.naturalearthdata.com/)
* Map Shaper (https://mapshaper.org/)
* Microsoft Learn - Geolocator Class (https://learn.microsoft.com/en-us/)
* Movable Type Scripts (https://www.movable-type.co.uk/scripts/latlong.html)
* Wikipedia (https://en.wikipedia.org/wiki/Earth_radius)

Honorable mention

* Github - WolfBearGames (https://github.com/WolfBearGames)

## Future Work?

The following items I can hypothetically fix, improve, and/or add to this project in the future:

* [ ] I can probably have the program wait to get your position before displaying the map (and you on the coast of Africa)
* [ ] I can make the snail slower, but that is less funny
* [ ] I can add a start screen, and a lose screen if the snail touches you
* [ ] I can either have a save file that stores and loads game state when you leave/rejoin, or I can find a way to have the app continue to run in the background.
* [ ] Have better file management
* [ ] Get GPS for Android and IOS to work