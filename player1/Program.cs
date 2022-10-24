using System;
using System.Net;
using System.Net.Sockets;

namespace player1
{
    class Player1
    {
        static void Main(string[] strings)
        {

            Console.Title = "player 1";
            var ip = IPAddress.Parse("127.0.0.1");
            int port = 8080;
            bool Player1Status = true;
            Socket SocketForClient = null;
            //player 2 data
            NetworkStream networkStream = null;
            StreamReader streamReader = null;
            StreamWriter streamWriter = null;


            //creating listener
            try
            {
                TcpListener tcpListener = new TcpListener(ip, port);
                tcpListener.Start();

                Console.WriteLine("Server started");
                Thread.Sleep(1000);
                Console.WriteLine("Trying to find second player...");

                //find and accpet player 2
                SocketForClient = tcpListener.AcceptSocket();
                Console.WriteLine("player 2 is found!");
                Thread.Sleep(1000);
                Console.Clear();


            }
            catch
            {
                Console.WriteLine("Could not start listeneing");
            }



            int player1Number = 0;

            //scores
            int p1Score = 0;
            int p2Score = 0;

            //if p2 could guess the number then guessed = true
            bool guessed = false;

            //game ending bool
            bool gameEnd = false;

            //round ending bool
            bool roundEnd = false;


            try
            {
                //assign player 2 data
                networkStream = new NetworkStream(SocketForClient);
                //receiving messages from player 2
                streamReader = new StreamReader(networkStream);
                //sending messages to player2
                streamWriter = new StreamWriter(networkStream);
                while (!gameEnd)
                {
                    Console.Clear();
                    Console.WriteLine($"{p1Score} : {p2Score}");

                    while (true)
                    {
                        Console.Write("Enter your number (1...10): ");

                        bool correctForm = int.TryParse(Console.ReadLine(), out player1Number);
                        if (correctForm && player1Number <= 10 && player1Number > 0)
                        {
                            break;
                        }
                        Console.WriteLine("Pls enter a number in a correct form!!!");
                    }
                    streamWriter.WriteLine(player1Number);
                    streamWriter.Flush();

                    Console.WriteLine("player 2 is guessing your number");

                    guessed = bool.Parse(streamReader.ReadLine());
                    //check if number is guessed by p2
                    if (guessed)
                    {
                        Console.WriteLine("player 2 guessed!");
                        Thread.Sleep(2000);
                        p2Score++;

                    }
                    else
                    {
                        Console.WriteLine("Player 2 could not guess");
                        Thread.Sleep(2000);
                        p1Score++;
                    }

                    //check if someone won all 5 rounds
                    if (p1Score == 5)
                    {
                        Console.Clear();
                        Console.WriteLine($"{p1Score} : {p2Score}");
                        Console.WriteLine("*** YOU WON *** \n CONGRATS !!!");
                        roundEnd = true;

                    }
                    else if (p2Score == 5)
                    {
                        Console.Clear();
                        Console.WriteLine($"{p1Score} : {p2Score}");
                        Console.WriteLine("*** PLAYER 2 WON *** \n YOU LOST :( ");
                        roundEnd = true;
                    }

                    //finish the game or continue
                    if (roundEnd)
                    {
                        Console.WriteLine("********************* \n Press <enter> to play again \n Or enter \"exit\" to finish \n *************************");
                        string enteredStr = Console.ReadLine();
                        if (String.IsNullOrEmpty(Console.ReadLine()))
                        {
                            p1Score = 0;
                            p2Score = 0;
                            roundEnd = false;
                        }
                        else if (enteredStr == "exit")
                        {
                            gameEnd = true;
                            streamReader.Close();
                            streamWriter.Close();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            networkStream.Close();
            streamReader.Close();
            streamWriter.Close();
            SocketForClient.Close();

        }

    }
}