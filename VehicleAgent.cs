using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;


//last modified 27/04/2017
class Player
{


    static void Main(string[] args)
    {
        string[] inputs;
        bool canBoost = true;
        bool firstTurn = true;
        double enemyDist = 19000; //max is 18357
        double enemyDistLastTurn = 0;
        double checkpointDist = 0;
        double checkpointDistLastTurn = 0;

        double angleBetween = 0;

        int x = 0;
        int y = 0;
        int enemyX = 0;
        int enemyY = 0;

        int enemyXLastTurn;
        int enemyYLastTurn;
        int enemySpeedX = 0;
        int enemySpeedY = 0;

        int ownXLastTurn;
        int ownYLastTurn;
        int ownSpeedX = 0;
        int ownSpeedY = 0;

        // game loop
        while (true)
        {
            enemyXLastTurn = enemyX;
            enemyYLastTurn = enemyY;
            ownXLastTurn = x;
            ownYLastTurn = y;
            enemyDistLastTurn = enemyDist;

            inputs = Console.ReadLine().Split(' ');
            x = int.Parse(inputs[0]);
            y = int.Parse(inputs[1]);
            int nextCheckpointX = int.Parse(inputs[2]); // x position of the next check point
            int nextCheckpointY = int.Parse(inputs[3]); // y position of the next check point
            int nextCheckpointDist = int.Parse(inputs[4]); // distance to the next checkpoint
            int nextCheckpointAngle = int.Parse(inputs[5]); // angle between your pod orientation and the direction of the next checkpoint
            inputs = Console.ReadLine().Split(' ');
            enemyX = int.Parse(inputs[0]);
            enemyY = int.Parse(inputs[1]);

            int thrust = 100;
            int absAngle = (Math.Abs(nextCheckpointAngle));


            enemyDist = Math.Sqrt((enemyX - x) * (enemyX - x) + (enemyY - y) * (enemyY - y));

            if (firstTurn == false)
            {
                enemySpeedX = enemyX - enemyXLastTurn;
                enemySpeedY = enemyY - enemyYLastTurn;
                ownSpeedX = x - ownXLastTurn;
                ownSpeedY = y - ownYLastTurn;
                angleBetween = (180.0 / Math.PI) * Math.Acos((ownSpeedX * enemySpeedX + ownSpeedY * enemySpeedY) / (Math.Sqrt((ownSpeedX * ownSpeedX) + (ownSpeedY * ownSpeedY)) * Math.Sqrt((enemySpeedX * enemySpeedX) + (enemySpeedY * enemySpeedY))));


            }
            else
            {
                enemySpeedX = 0;
                enemySpeedY = 0;
                ownSpeedX = 0;
                ownSpeedY = 0;
                angleBetween = 0;
            }



            firstTurn = false;


            /*angleBetween = (180.0 / Math.PI)*Math.Acos((ownSpeedX*enemySpeedX + ownSpeedY*enemySpeedY)/(Math.Sqrt((ownSpeedX*ownSpeedX)+(ownSpeedY*ownSpeedY))*Math.Sqrt((enemySpeedX*enemySpeedX)+(enemySpeedY*enemySpeedY))));*/

            Console.Error.WriteLine(angleBetween);

            checkpointDistLastTurn = checkpointDist;
            checkpointDist = Math.Sqrt((nextCheckpointX - x) * (nextCheckpointX - x) + (nextCheckpointY - y) * (nextCheckpointY - y));

            if (checkpointDist >= 3 * checkpointDistLastTurn && checkpointDistLastTurn != 0)
            {
                thrust = 0;
            }
            else
            {
                //corrects thrust based on where the the nose is pointing
                int correctedThrust = (int)(100f - 0.70 * absAngle); //default 0.67
                thrust = correctedThrust;                           //smaller - more speed, wider turns
                if (thrust < 0) thrust = 0;                         //bigger - tighter turns, less speed
                if (thrust >= 98) thrust = 100;
                if (checkpointDist <= 1500 && thrust > 40) //default 1500, 40
                {
                    thrust = 40;
                }
            }
            //checks if the pods are on collision course
            if (Math.Sqrt(((enemyX + enemySpeedX) - (x + ownSpeedX)) * ((enemyX + enemySpeedX) - (x + ownSpeedX)) + ((enemyY + enemySpeedY) - (y + ownSpeedY)) * ((enemyY + enemySpeedY) - (y + ownSpeedY))) <= 800 && nextCheckpointDist < 1200 && Math.Abs(angleBetween) <= 5) //800 is collider diameter for both pods
            {
                /*Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " +0);*/
                /*Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " +"SHIELD");*/
            }
            else if (absAngle < 15 && nextCheckpointDist > 6000 && canBoost) //will use boost only when it is advantageous
            {
                Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " + "BOOST");
                canBoost = false;
            }
            else
            {
                Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " + thrust);   //if it can't boost and won't collide in the next turn, it will go towards the next checkpoint
            }



            /*Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " +thrust);*/
        }
    }
}