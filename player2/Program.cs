using System;
using System.Net;
using System.Net.Sockets;

namespace player2
{
    class Player2
    {
        static void Main(string[] strings)
        {
            Console.Title = "Player 2";

            //tcp 
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 8080;
            TcpClient socketForPlayer1 = null;
            NetworkStream networkStream = null;
            StreamReader streamReader = null;
            StreamWriter streamWriter = null;


            try
            {
                Console.WriteLine("Connecting to player 1");
                socketForPlayer1 = new TcpClient(ip.ToString(), port);
                Console.Write("Connected");
                Thread.Sleep(1000);
                Console.Clear();
            }
            catch
            {
                Console.WriteLine("could not connect to player 1");
            }

            try
            {
                //asign player1 data
                networkStream = socketForPlayer1.GetStream();
                streamReader = new StreamReader(networkStream);
                streamWriter = new StreamWriter(networkStream);

                //entered numbers
                int player1Number = 0;
                int guessNumber = 0;

                //scores
                var p1score = 0;
                var p2score = 0;

                //game Ending bool
                bool gameEnd = false;
                //round ending bool
                bool roundEnd = false;


                while (!gameEnd)
                {
                    Console.Clear();
                    Console.WriteLine($"{p2score} : {p1score}");


                    Console.WriteLine("player 1 entering a number...");
                    player1Number = int.Parse(streamReader.ReadLine());

                    //if p2 could guess the number guessed = true
                    bool guessed = false;
                    for (int chance = 0; chance < 3; chance++)
                    {
                        Console.Write("Guess the number: ");

                        if (int.TryParse(Console.ReadLine(), out guessNumber))
                        {

                            //checking guessed number and number that player 1 entered 
                            if (guessNumber == player1Number)
                            {
                                Console.WriteLine("you won");
                                Thread.Sleep(2000);
                                p2score++;
                                guessed = true;
                                break;
                            }
                            else if (guessNumber > player1Number)
                            {
                                Console.WriteLine("The number is smaller");
                            }
                            else if (guessNumber < player1Number)
                            {
                                Console.WriteLine("The number is bigger");
                            }

                        }
                        //if the entered number isnt in correct form
                        else
                        {
                            Console.WriteLine("pls enter a number!!!");
                            chance--;
                        }

                    }
                    //sending to player 1 the answer we got
                    streamWriter.WriteLine(guessed);
                    streamWriter.Flush();
                    //if we could not guess number
                    if (!guessed)
                    {
                        Console.WriteLine("You Lost!");
                        Console.WriteLine("The number was {0}", player1Number);
                        Thread.Sleep(2000);

                        p1score++;
                    }


                    //check if someone won all 5 rounds
                    if (p2score == 5)
                    {
                        Console.Clear();
                        Console.WriteLine($"{p1score} : {p2score}");
                        Console.WriteLine("*** YOU WON *** \n CONGRATS !!!");
                        roundEnd = true;

                    }
                    else if (p1score == 5)
                    {
                        Console.Clear();
                        Console.WriteLine($"{p1score} : {p2score}");
                        Console.WriteLine("*** PLAYER 1 WON *** \n YOU LOST :( ");
                        roundEnd = true;
                    }

                    //check if players want to continue or exit
                    if (roundEnd)
                    {
                        Console.WriteLine("********************* \n Press <enter> to play again \n Or enter \"exit\" to finish \n*******************");
                        string enteredStr = Console.ReadLine();
                        if (String.IsNullOrEmpty(Console.ReadLine()))
                        {
                            p1score = 0;
                            p2score = 0;
                            roundEnd = false;
                        }
                        else if (enteredStr == "exit")
                        {
                            streamReader.Close();
                            streamWriter.Close();
                            gameEnd = true;

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            socketForPlayer1.Close();
            networkStream.Close();
            streamReader.Close();
            streamWriter.Close();

        }


    }
}
