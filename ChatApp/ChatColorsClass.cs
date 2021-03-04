using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ChatApp
{
    class ChatColorsClass
    {
        public static int PrintGetColorInfoFromUser()
        {
            string[] colorNames = {"Red","Green","Blue","Yellow","White"};                              //We make a array of colors for the user to choose from (the order is important to the second func)

            bool parseCheck = false;                                                                    //at the start parse is always false and it will only be true if we successfully get a correct input from the user
            int colorNumber = 0;
            Console.WriteLine("Choose your chat color! Enter the corresponding number!");               

            while (true)
            {
                for (int i = 0; i < colorNames.Length; i++)                                             //print all the colors we have
                {
                    Console.WriteLine((i + 1) + ": " + colorNames[i]);
                }

                parseCheck = int.TryParse(Console.ReadLine(), out colorNumber);                         //tryparse waits for the users input here they will type the number that correspondes to the color

                if (parseCheck == false)                                                                //if tryparse failes that means the input is invalid so we will go back to the start of the while loop
                {
                    Console.WriteLine("Your input is invalid! Please enter a valid input!");
                    Console.WriteLine("Type only the number corresponding to the color you want!");     
                }
                else                                                                                    //else tryparse has succeeded & we will need to do one more check 
                {
                    if (colorNumber < colorNames.Length - 1)                                            //we check if the color number is in the length of colors we provided at the top 
                    {
                        return colorNumber;                                                             // if it  is within the length of colors we return the color number they chose
                    }
                    else                                                                                // else we give them a error msg
                    {
                        Console.WriteLine("The number you input is not within the bounds of the list of provided colors!");
                        Console.WriteLine("Type only the number corresponding to the color you want!");
                    }
                }
            }
        }

        public static ConsoleColor GetIndexOfColor(int colorIndex)                                      // just a function that returns the color index accoridng to the list provided at the top both of these must be in order!
        {
            switch (colorIndex)
            {
                case 1:
                    return ConsoleColor.Red;
                case 2:
                    return ConsoleColor.Green;
                case 3:
                    return ConsoleColor.Blue;
                case 4:
                    return ConsoleColor.Yellow;
                case 5:
                    return ConsoleColor.White;
                default:
                    return ConsoleColor.Cyan;
            }
        }
    }
}
