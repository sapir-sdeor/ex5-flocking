
<h3 align="center">  
  Exercise #5 - Flocking - Sapir Sdeor and Roi Shacham
</h3>

## Obstacles
<p align="center">
<img src="/Screenshots/Flock Racing.png" width="507" height="304"/>
</p>
  
## General

The first step for our process was to implement the flocking Cohesion and Alignment features.
We tried to follow the logic of the separation feature that was already implemented for us, and after some trials and errors we got ourselves a decent flocking.

Our next step was to find an interesting experience we can create that revolves around flocking.
We liked the idea of collecting peeps to your group, so after some brainstorming we decided to create a 2 player race game.
Each player will be a leader of a group, and reach the finish line before is opponent. The twist is that one can only pass the finish line if his group has 10 or more peeps.
To make the experience more interesting, we decided to implement some power-ups and obstacles(As described below).
Few hours after we came up with the idea we already got ourselves a working prototype, and from that point forward we spent the next 2 weeks to make the experience more reach and interesting.


## Obstacles
<p align="center">
<img src="/Screenshots/Obstacles.gif" width="400" height="100"/>
</p>

1. Rotating obstacle - If it touches your group peeps, they are removed from your group.

2. Moving sideways obstacle - If it touches your group peeps, they are removed from your group..

3. Sparks obstacle - When any of your peeps comes close to it, they are separating from you for a given time instead of following you.


## Power-Ups
<p align="center">
<img src="/Screenshots/Power-Ups.gif" width="400" height="100"/>
</p>

1. Protect - When you take it, your peeps are protected for 7 seconds from obstacles.

2. Steal - When you take it, one of the peeps from your rivals team is moves into your team.


## Shaders
<p align="center">
<img src="/Screenshots/Shaders.png" width="350" height="175"/>
</p>

Peeps can be "protected" from obstacles at times, either when a player takes a "Protect" power up, or when he collects a new peep(That peep will be protected for a short period of time). We wanted to give the players a visual indication to when they or their team peeps are protected, so we used a ghost-looking shader that's appearing on a peep that is protected.

## What we would like to achieve given more time?
With more time wo would probably make diverse - have a vast range of obstacles instead of 3 three, more levels, possibly even randomly-generated levels.

We would like to add more juice to our game - sounds, shaders, special-effects etc.

Possibly even think of some other type of races - for example one when your goal is to reach the finish line, and the flocking peeps are used not as team-mates but as obstacles.
