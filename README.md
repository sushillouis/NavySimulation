# NavySimulation
A naval simulation for academic research. The simulation has several ship types, simple ship physics using desired speed and desired heading, keyboard controls, AI controls for move, follow, and intercept, with RTS style right mouse click commands.

Runs in and developed on Unity 2018.4.19, but should run on other versions with little trouble. (April 15, 2024: Updated to new UI and to run on 2022 LTS)
Uses some assets from the deprecated Unity Standard Assets package. Ship Models were brought on the Unity asset store or from TurboSquid.
Ships assume constant acceleration and constant turn rate.

Doing the tutorials at: https://docs.google.com/presentation/d/1dehTrM8pLemBWZd7-qXev9pUkZPpYvW-lXwi0k7SlkU/edit?usp=sharing (A google drive link) will help you understand the code.

### Commands
#### Keyboard
1.  Arrow keys to control desired speed and desired heading of ship
2.  WASD to move camera. QE to yaw camera. ZX to pitch camera. RF to raise and lower camera. C to switch to third person view from selected ship
3.  Tab key to select next ship

#### Mouse
1. Left mouse click select. Drag to group select
2. Right mouse click
    - Over open water --> move to clicked location
    - Over other ship --> follow that ship at a relative vector of (100, 0, 0) -  on the starboard side at 100 meters
    - CTRL-right-mouse click over other ship --> predictively intercept other ship (Collide)

The code and simulation architecture follow the assignments and notes from the CS381 Game Engine Architecture course at the University of Nevada, Reno
https://www.cse.unr.edu/~sushil/class/381.
