# GameJourney - My Monogame and Aseprite Learning Repo

## Table of Contents
[Day Zero, ECS and Basics](#ECS)</br>
[Day One, Pong](#Pong)</br>
[Day Two, Snake](#Snake)</br>

## Day Zero - Basic Monogame and my ECS Template <a name="ECS"></a> - <a href="https://github.com/ReedOlm/GameJourney/tree/main/ECSTemplate">Link to Repository</a>
### Learning basic ECS project format and creating a template
<ul>
  <li>Created a template project with basic ECS implemented</li>
  <li>Core contains an abstract component class, a Data class, MainGame class, and a simple SaveData class</li>
  <li>Managers contains a game-state-manager</li>
  <li>Scenes contains a Main menu, a game scene, and a settings scene</li>
  <li>Also implements a RenderTarget to target 1080p, need to learn more about how this works and scales textures, especially with mouse inputs</li>
  <li>This should also be targeting 144fps, and should not be linking physics to frame rate</li>
</ul>

## Day One - Pong <a name="Pong"></a> - <a href="https://github.com/ReedOlm/GameJourney/tree/main/Pong">Link to Repository</a>
### My first game, a mostly complete 2 player pong
<ul>
  <li>First attempt at implementing a pause menu, edited Data/GamestateManager to implement</li>
  <li>Basic collision detection on rectangles and ball, basic clamping on paddles</li>
  <li>Score keeping using a sprite sheet and the texture changes to the correct number, this is the basis for future animation</li>
  <li>My object placement is awful and obtuse</li>
  <li>My GameScene needs to be split up into more methods</li>
  <li>Game ends at 7 points, nothing fancy, no real victor decalred</li>
</ul>

## Day Two - Snake <a name="Snake"></a> - <a href="https://github.com/ReedOlm/GameJourney/tree/main/Snake">Link to Repository</a>
### My second game, goals include better art, learning sound, more functional victory/loss screen, persistent high scores, and render targeting understanding
<ul>
  <li>Created art/sounds/music for snake! Sprite sheet usage to display correct snake directions</li>
  <li>Basic collision detection on walls and snake parts</li>
  <li>Persistent high score keeping</li>
  <li>Much better at placing objects around a screen</li>
  <li>Need to follow an ECS format better, using characters that are a collection of components rather than what I did</li>
  <li>Victory screen with my first attempt at resetting the game.</li>
  <li>Functional resolution swapping!</li>
  <li>Just realized there is no way back to the menu once you start the game</li>
</ul>
