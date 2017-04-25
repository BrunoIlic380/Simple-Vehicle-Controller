using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

//gets input from the game controller and outputs destination and speed
class VehicleController //rename to "class Player" if using the script for "https://www.codingame.com/ide/puzzle/coders-strike-back"
{
    static void Main(string[] args) //rename to something else if using the script in a standalone game
    {
        string[] inputs;
        bool canBoost = true;
        double enemyDist = 19000; //max is 18357
        double enemyDistLastTurn = 0;

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int x = int.Parse(inputs[0]);
            int y = int.Parse(inputs[1]);
            int nextCheckpointX = int.Parse(inputs[2]); // x position of the next checkpoint
            int nextCheckpointY = int.Parse(inputs[3]); // y position of the next checkpoint
            int nextCheckpointDist = int.Parse(inputs[4]); // distance to the next checkpoint
            int nextCheckpointAngle = int.Parse(inputs[5]); // angle between your pod orientation and the direction of the next checkpoint
            inputs = Console.ReadLine().Split(' ');
            int opponentX = int.Parse(inputs[0]);
            int opponentY = int.Parse(inputs[1]);

            int thrust = 100;
            int absAngle = (Math.Abs(nextCheckpointAngle));

            enemyDistLastTurn = enemyDist;
            enemyDist = Math.Sqrt((opponentX - x) * (opponentX - x) + (opponentY - y) * (opponentY - y));
            //corrects thrust based on where the the nose is pointing
            int correctedThrust = (int)(100f - 0.64 * absAngle); //default 0.64
            thrust = correctedThrust;                           //smaller - more speed, wider turns
            if (thrust < 0) thrust = 0;                         //bigger - tighter turns, less speed
            if (thrust >= 98) thrust = 100;



            if (nextCheckpointAngle < 15 && nextCheckpointAngle > -15 && nextCheckpointDist > 5000 && canBoost) //will use boost only when it is advantageous
            {
                Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " + "BOOST");
                canBoost = false;
            }
            else if (nextCheckpointAngle < 15 && nextCheckpointAngle > -15)
            {
                Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " + thrust);
            }
            else if (enemyDist <= 900 && enemyDist < enemyDistLastTurn) //if collision is unavoidable, will turn on shields
            {
                Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " + "SHIELD");
            }
            else
            {
                Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " + thrust);   //if it can't boost and won't collide in the next turn, it will go towards the next checkpoint
            }



            /*Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " +thrust);*/   
        }
    }
}