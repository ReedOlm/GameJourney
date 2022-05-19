# GameJourney - My Monogame and Aseprite Learning Repo

## Table of Contents
[Day Zero, ECS and Basics](#ECS)</br>
[Day One, Pong](#Pong)</br>
[Day Two, Snake](#Snake)</br>

## Day Zero - Basic Monogame and my ECS Template <a name="ECS"></a> - <a href="https://github.com/ReedOlm/CS4843/tree/main/basicCICD">Link to Repository</a>
### Learning basic ECS project format and creating a template
<ul>
  <li>Created a template project with basic ECS implemented</li>
  <li>Core contains an abstract component class, a Data class, MainGame class, and a simple SaveData class</li>
  <li>Managers contains a game-state-manager</li>
  <li>Scenes contains a Main menu, a game scene, and a settings scene</li>
  <li>Also implements a RenderTarget to target 1080p, need to learn more about how this works and scales textures, especially with mouse inputs</li>
  <li>This should also be targeting 144fps, and should not be linking physics to frame rate</li>
</ul>

## Day One - Pong <a name="Pong"></a> - <a href="https://github.com/ReedOlm/CS4843/tree/main/KittiesOfTheWeek">Link to Repository</a>
### My first game, a mostly complete 2 player pong
<ul>
  <li>First attempt at implementing a pause menu, edited Data/GamestateManager to implement</li>
  <li>Basic collision detection on rectangles and ball, basic clamping on paddles</li>
  <li>Score keeping using a sprite sheet and the texture changes to the correct number, this is the basis for future animation</li>
  <li>My object placement is awful and obtuse</li>
  <li>My GameScene needs to be split up into more methods</li>
  <li>Game ends at 7 points, nothing fancy, no real victor decalred</li>
</ul>

## Day Two - Snake <a name="Snake"></a> - <a href="https://github.com/ReedOlm/CS4843/tree/main/CloudFormationDeployment">Link to Repository</a>
### My future second game
<!---
<ul>
  <li>Using a bash script and the AWS cli, deployed 3 different stacks</li>
  <li>Deployed a scalable network framework using a YAML template, that split the us-east-1 servers into private/public subnets using CIDR</li>
  <li>This network framework included 2 VPCs, an InternetGateway, the aforementioned Subnets, a NAT with elastic ip's, and routing tables.</li>
  <li>Deployed a loadbalancing private webserver using a YAML template, with a configurable JSON parameter file to the 2 previously created us-east-1 private subnets using my own AMI/key values</li>
  <li>Created a final EC2 instance as a public Jumpbox inside of the VPC created for the network, and passed it the required keys to allow my personal computer's IP address to SSH into the jumpbox, then was able to ssh into both of my private EC2 servers</li>
  <li>Here is my drawing of what this system essentially looks like when fully deployed. (Has been taken down to avoid being charged by Amazon.):</li>
  
  ![architectureDiagram](/CloudFormationDeployment/architectureDiagram.PNG)

</ul>

## Assignment 3 - Final Project: Google Dataflow and Google Big Query Data Manipulation <a name="Assignment3"></a> -<a href="https://github.com/ReedOlm/CS4843/tree/main/FinalProject_DataflowBigQuery">Link to Repository</a>
### Creation of a Dataflow and Big Query Pipeline to Manipulate Data (Line Counting)
<ul>
  <li>Using Cloud terminal we plugged in and executed our Java functions (files found in repository for reference)</li>
  <li><a href="https://drive.google.com/drive/folders/1J596Fjr2qEkI7WR1pLcxc1a0G233ykyX?usp=sharing">Video Demonstration Link</a></li>
  <li>Images of our Dataflow data pipeline charts setup:</li>
  
  ![architectureDiagram1](/FinalProject_DataflowBigQuery/Image1.png)
  
  ![architectureDiagram2](/FinalProject_DataflowBigQuery/Image2.png)
  
  ![architectureDiagram3](/FinalProject_DataflowBigQuery/Image3.png)
  
  <li><a href="https://console.cloud.google.com/storage/browser/_details/dataflow-cloudcomputingdataflow/linecount-00000-of-00001;tab=live_object?project=cloudcomputingdataflow">Google Cloud Storage Link, displaying data AFTER data is piped through Dataflow</a></li>
  <li><a href="https://storage.cloud.google.com/dataflow-cloudcomputingdataflow/linecount-00000-of-00001?_ga=2.228040859.-720083893.1649035167&_gac=1.258673528.1649045030.CjwKCAjwi6WSBhA-EiwA6Niok6GATVCoGJBljVJ8VtvwJfeyLIj5qKI0BZwgwkA3wEPyMWkrgm4RLhoC4RIQAvD_BwE">Verification that Google Dataflow successfully uploaded our data passed in from our Java script</a></li>
  <li>NOTE!! The link above will take users to a webpage containing an output stored in our Google Cloud Storage. The output should read 5525, indicating that our Java function has worked correctly, piped the output through Google Dataflow and successfully stored it in our Cloud Storage. The Google Cloud project will be deleted on June 15 as not overuse data on service.</li>

</ul>
-->
