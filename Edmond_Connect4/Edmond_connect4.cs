using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edmond_Connect4
{
    

    public partial class Form1 : Form
    {
        //m stands for rows
        //n stands for columns
        //m comes first
        //n comes second

        int[,] board = new int[11, 12];//A matrix array that represents the game board **board starts at [1,1] and ends at [6,7]**
                                     //the matrix is longer than I need it to be. I do this to take care of out of bounds calls *I know!! Messy...*
        int[,] boardx = new int[11, 12];//for editing

        int turn = 1;//set it to no ones turn let it be someones turn
        bool ready = true;//to stop the player from playing
        int m = 1;
        int n = 1;//for the second co-odinate
        bool Aturn = false;//is it the autonumuses turn?
        bool Amode = false;//are we in autonumus mode?

        Random rand = new Random();//usesd to pick turns
        int place;

        string p1;
        string p2;
        string mode;

        public Form1(string pOne, string pTwo, string Mode)
        {
            InitializeComponent();

            //setup the international values
            p1 = pOne;
            p2 = pTwo;
            mode = Mode;

            //set the board to zero
            board = Zero(board);

            //setup who goes first
            place = rand.Next(0, 4);


            if (mode == "Single Player")//starts the AI if it is needed
            {
                Amode = true;

                //set p1 and p2
                labelP1.Text = p1;
                labelP2.Text = "Computer";

                //used to set up turns
                if (place % 2 == 1)//P1 Red, P2 blue
                {
                    labelP1.BackColor = Color.Blue;
                    labelP2.BackColor = Color.Red;
                    //ai plays
                    humanDone();
                    
                }
                else//P1 Blue, P2 Red
                {
                    labelP1.BackColor = Color.Red;
                    labelP2.BackColor = Color.Blue;

                    Aturn = true;
                }
            }
            else 
            {

                //set p1 and p2
                labelP1.Text = p1;
                labelP2.Text = p2;

                //used to set up turns
                if (place % 2 == 1)//P2 Blue, P1 Red
                {
                    labelP1.BackColor = Color.Blue;
                    labelP2.BackColor = Color.Red;

                    MessageBox.Show(p2 + " goes first");
                }
                else//P1 Blue, P2 Red
                {
                    labelP1.BackColor = Color.Red;
                    labelP2.BackColor = Color.Blue;

                    MessageBox.Show(p1 + " goes first");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {   
            /*
            //debuging
            board[6, n] = 1;
            board[6, n + 1] = 1;
            board[6, n + 2] = 1;
            board[6, n + 3] = 1;
            Show(board);
             */
        }

        private int[,] Zero(int[,] board)//populate an array with zero's
        {
            for (int m = 0; m <= 6; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 0; n <= 7; n++)//go through all the second spaces in the matrix (7)
                {
                    board[m, n] = 0;//make the board element at that space [m,n] equal zero 
                }
            }

            return board;//return the value of the edited array
        }

        private void Show(int[,] board)//change the picture depending on the array value
        {
            //List<PictureBox> boxes = new List<PictureBox>();//create a new picturebox list
            Control[] matches;

            for (int m = 1; m <= 6; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 7; n++)//go through all the second spaces in the matrix (7)
                {
                    matches = this.Controls.Find("p" + (m).ToString() + (n).ToString(), true);

                    if ((matches[0] is PictureBox) && (board[m,n] == 1 ))//if its turn one 
                    {
                        matches[0].BackgroundImage = Properties.Resources.Ball_Red;
                    }

                    if ((matches[0] is PictureBox) && (board[m, n] == 2))//if its turn two
                    {
                        matches[0].BackgroundImage = Properties.Resources.Ball_Blue;
                    }

                    if ((matches[0] is PictureBox) && (board[m, n] == 0))//if its turn two
                    {
                        matches[0].BackgroundImage = null;
                    }
                }
            }
        }

        private int switchTurn(int turn)//switch the players turn between one an zero
        {
            if (turn == 1)//*Make pictureboxes change color*
            {
                turn = 2;
            }
            else if (turn == 2)
            {
                turn = 1;
            }

            return turn;
        }

        private void p15_Click(object sender, EventArgs e)//when u want to play on the fifth one
        {
            n = 5;//for the fifth part

             if (ready)//if the player can play
            {
                TimerFall.Start();
            }
            
        }

        public void moveDown(int n)//move down 
        {
           
                ready = false;//u cant click after this

                if (board[m, n] == 0)
                {

                    board[m - 1, n] = 0;//make the previous space equal zero
                    board[m, n] = turn;//if there is space on that point then put the thing there 
                    m += 1;//set up for the next round

                }
                if(board[m,n] != 0)// if there is a value here
                {
                    TimerFall.Stop();//stop the timer
                    ready = true;//u can click now
                    turn = switchTurn(turn);
                    m = 1;//setup for next time

                    //check if someone won
                    Won();

                    //* turns With the AI
                    if ((Aturn == true) && (Amode == true))//if it's the AI's turn and it can play
                    {
                        humanDone();//the human is done
                        Aturn = false;//it will be the human's turn next
                    }
                    else if((Aturn == false) && (Amode == true))//if it's not the AI's turn and it can play 
                    {
                        Aturn = true;//it will be the Ai's turn next turn
                    }
                }

                else if (m == 7)//if it has hit the bottom limit then   
                {
                    TimerFall.Stop();//stop the falling timer
                    ready = true;//u can click now
                    turn = switchTurn(turn);
                    m = 1;//set up for next time

                    //check if someone won
                    Won();

                    //* turns With the AI
                    if ((Aturn == true) && (Amode == true))//if it's the AI's turn and it can play
                    {
                        humanDone();//the human is done
                        Aturn = false;//it will be the human's turn next
                        
                    }
                    else if ((Aturn == false) && (Amode == true))//if it's not the AI's turn and it can play 
                    {
                        Aturn = true;//it will be the Ai's turn next turn
                    }
                }

        }

        private void Won()//small win function
        {
            if (WinCheck(board) == 1)
            {
                MessageBox.Show("Red wins");
                ready = false;
            }
            if (WinCheck(board) == 2)
            {
                MessageBox.Show("Blue wins");
                ready = false;
            }
            if (WinCheck(board) == 3)
            {
                MessageBox.Show("Draw");
                ready = false;
            }
           // MessageBox.Show(WinCheck().ToString());//debug
        }

        private void TimerFall_Tick(object sender, EventArgs e)//movement timer
        {
           // try
           // {
                moveDown(n);//call the move down function
                Show(board);//show the effects on the board
           // }
           // catch(Exception error) 
           // {
           //     MessageBox.Show(error.Message);
           // }
        }

        private int WinCheck(int[,] board)//*Bigger win function* checks if someone has won **1*p1*,2*p2*,3*draw* or 0*None**
        {
            int retVal = 3;//make this value equal a full array until proven false

            for (int m = 1; m <= 6; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 7; n++)//go through all the second spaces in the matrix (7)*limitied array to 4*
                {
                    //column sides check if there is a win on one of the columns
                    if ((board[m, n] == board[m, n + 1]) && (board[m, n] == board[m, n + 2]) && (board[m, n] == board[m, n + 3]) && (board[m, n] != 0))
                    {
                         return board[m, n];//send back who won
                    }

                    //vertical row check if there is a win on one of the rows 
                    else if ((board[m, n] == board[m + 1, n]) && (board[m, n] == board[m + 2, n]) && (board[m, n] == board[m + 3, n]) && (board[m, n] != 0))
                    {
                        return board[m, n];//send back who won
                    }

                    //diagonal row check for diagonals going downwards
                    else if ((board[m, n] == board[m + 1, n + 1]) && (board[m, n] == board[m + 2, n + 2]) && (board[m, n] == board[m + 3, n + 3]) && (board[m, n] != 0))
                    {
                        return board[m, n];//send back who won
                    }

                        //diagonal row check for diagonals going upwards
                    else if (((m - 1) > 0) && ((m - 2) > 0 ) && ((m - 3) > 0))//if it goes out of bounds
                    {
                        //*Treating out of bounds of the array possibilities*
                        if ((board[m, n] == board[m - 1, n + 1]) && (board[m, n] == board[m - 2, n + 2]) && (board[m, n] == board[m - 3, n + 3]) && (board[m, n] != 0))//if it becomes out of bounds
                        {
                            return board[m, n];
                        }
                    }

                    else if(board[m,n] == 0)//if there is at least one space that is zero then the board is not filled 
                    {
                        retVal = 0;//not full and no one has won
                    }
                    
                }
            }
            //at the end of the whole loop
            return retVal;// send back the result
        }

        private void humanDone()//when the human player is done playing
        {
            ready = false;//the human player cannot play yet 
            
            
            //Ask the AI where it wants to play
            n = MastBrain(turn);//

            //plays at that spot
            TimerFall.Start();
        }
        
        

        //Autonomus Master Brain *Calls on the opinon of smaller brains to make choices*
        private int MastBrain(int me)//returns a single interger that shows where it wants to play
        //it needs to know what turn it's going at.
        {
            //If it can win, then win
            //the horizontal threats
            int[] threatP1Hor = ThreatP1Hor();
            int[] threatP2Hor = ThreatP2Hor();

            //The vertical threats
            int[] threatP1Ver = ThreatP1Ver();
            int[] threatP2Ver = ThreatP2Ver();

            //the diagonal down threats 
            int[] threatP1Dwn = ThreatP1Dwn();
            int[] threatP2Dwn = ThreatP2Dwn();

            //The diagonal up threats 
            int[] threatP1Up = ThreatP1Up();
            int[] threatP2Up = ThreatP2Up();

            //Pre threats 
            //horizontal pre threats
            int[] PrethreatP1Hor = PreThreatP1Hor();
            int[] PrethreatP2Hor = PreThreatP2Hor();

            //Vertical pre threats
            int[] PrethreatP1Ver = PreThreatP1Ver();
            int[] PrethreatP2Ver = PreThreatP2Ver();

            //diagonally down pre threats
            int[] PrethreatP1Dwn = PreThreatP1Dwn();
            int[] PrethreatP2Dwn = PreThreatP2Dwn();

            //diagonally Up pre threats
            int[] PrethreatP1Up = PreThreatP1Up();
            int[] PrethreatP2Up = PreThreatP2Up();

            //*Heirchy* 
            //Wining is the main priority
            if ((threatP1Hor[1] == turn))//if the threat is from me
            {
                return threatP1Hor[0];//return the n
            }
            else if(threatP2Hor[1] == turn)//if the other threat is from me
            {
                return threatP2Hor[0];//return the n
            }
            else if(threatP1Ver[1] == turn)//vertical threats
            {
               // MessageBox.Show("Ver | For me");
                return threatP1Ver[0];//return the n
            }
            else if (threatP2Ver[1] == turn)//vertical threats
            {
               // MessageBox.Show("Ver | For me");
                return threatP2Ver[0];//return the n
            }
            else if (threatP1Dwn[1] == turn)//diagonal dwn threats
            {
                return threatP1Dwn[0];//return the n
            }
            else if (threatP2Dwn[1] == turn)//diagonal dwn threats
            {
                return threatP2Dwn[0];//return the n
            }
            else if (threatP1Up[1] == turn)//diagonal up threats
            {
               // MessageBox.Show("Up | For me");
                return threatP1Up[0];//return the n
            }
            else if (threatP2Up[1] == turn)//diagonal up threats
            {
               // MessageBox.Show("Up | For me");
                return threatP2Up[0];//return the n
            }

            //Then Blocking
            else if (threatP1Hor[1] != 3)//else if its from the opponent
            {
                return threatP1Hor[0];//return the n
            }
            else if (threatP2Hor[1] != 3)//else if there is another threat that is from the opponent
            {
                return threatP2Hor[0];//return the n 
            }
            else if (threatP1Ver[1] != 3)//vertical threats
            {
                //MessageBox.Show("Ver | For you");
                return threatP1Ver[0];//return the n
            }
            else if (threatP2Ver[1] != 3)//vertical threats
            {
                //MessageBox.Show("Ver | For you");
                return threatP2Ver[0];//return the n
            }
            else if (threatP1Dwn[1] != 3)//diagonal dwn threats
            {
                return threatP1Dwn[0];//return the n
            }
            else if (threatP2Dwn[1] != 3)//diagonal dwn threats
            {
                return threatP2Dwn[0];//return the n
            }
            else if (threatP1Up[1] != 3)//diagonal up threats
            {
                //MessageBox.Show("Up | For you");
                return threatP1Up[0];//return the n
            }
            else if (threatP2Up[1] != 3)//diagonal up threats
            {
               // MessageBox.Show("Up | For you");
                return threatP2Up[0];//return the n
            }
             

            //PreThreats
            //for me
            else if(PrethreatP1Hor[1] == turn)//horizontal pre threats
            {
                
                return PrethreatP1Hor[0];//return the n
            }
            else if (PrethreatP2Hor[1] == turn)//horizontal pre threats
            {
                
                return PrethreatP2Hor[0];//return the n
            }
            else if (PrethreatP1Ver[1] == turn)//ver pre threats
            {
                
                return PrethreatP1Ver[0];//return the n
            }
            else if (PrethreatP2Ver[1] == turn)//ver pre threats
            {
                
                return PrethreatP2Ver[0];//return the n
            }
            else if (PrethreatP1Dwn[1] == turn)//diag down pre threats
            {
               
                return PrethreatP1Dwn[0];//retuen n
            }
            else if (PrethreatP2Dwn[1] == turn)//diag down pre threats
            {
                
                return PrethreatP2Dwn[0];//retuen n
            }
            else if (PrethreatP1Up[1] == turn)//diag up pre threats
            {
                
                return PrethreatP1Up[0];//retuen n
            }
            else if (PrethreatP2Up[1] == turn)//diag up pre threats
            {
               
                return PrethreatP2Up[0];//retuen n
            }

            //against me
            else if (PrethreatP1Hor[1] != 3)//horizontal pre threats
            {
                
                return PrethreatP1Hor[0];//return the n
            }
            else if (PrethreatP2Hor[1] != 3)//horizontal pre threats
            {
               
                return PrethreatP2Hor[0];//return the n
            }
            else if (PrethreatP1Ver[1] != 3)//ver pre threats
            {
                
                return PrethreatP1Ver[0];//return the n
            }
            else if (PrethreatP2Ver[1] != 3)//ver pre threats
            {
                
                return PrethreatP2Hor[0];//return the n
            }
            else if (PrethreatP1Dwn[1] != 3)//diag down pre threats
            {
                
                return PrethreatP1Dwn[0];//return the n
            }
            else if (PrethreatP2Dwn[1] != 3)//diag down pre threats
            {
                
                return PrethreatP2Dwn[0];//return the n
            }
            else if (PrethreatP1Up[1] != 3)//diag Up pre threats
            {
                
                return PrethreatP1Up[0];//return the n
            }
            else if (PrethreatP2Up[1] != 3)//diag Up pre threats
            {
                
                return PrethreatP2Up[0];//return the n
            }


            //in the end
            else
            {
                //play a random position
                Random play = new Random();
                return play.Next(1, 8);//return
            }
        }

        //identifies a possible win 
        //returns two values in an array *Who is about to win* and *where it is goint to happen*
        private int[] ThreatP1Hor()//method that finds threats for p1 Horizontally
        {
            //  Himanshi: "Himanshi is the best person in the world" * Edmond: "LOOOL... No Comment"*

            for (int m = 1; m <= 6; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 4; n++)//go through all the second spaces in the matrix (7 but we only need to get to 4)
                {
                    //check if there is a the threat from p1 
                    if (((board[m, n] + board[m, n + 1] + board[m, n + 2] + board[m, n + 3]) == 3) //checks if the sum of that point and three points around it are equal to 3
                    && 
                    ((board[m, n] != 2) && (board[m, n + 1] !=2) && (board[m, n + 2] != 2) && (board[m, n + 3] != 2)))//and none of them are equal to 2
                    {
                        for(int i = 0; i <=3; i++)//go through all 4 spaces to find one that is equal to zero
                        {
                            if ((board[m, n + i] == 0) && ((board[m +1,n + i] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                            {
                                int[] retval = { n + i, 1 };//make a return value with the wining position and who is going to win
                                return retval;
                            }
                        }
                        
                        
                    }
          }
         }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] ThreatP2Hor()//method that finds threats for p2 Horizontally
        {
            for (int m = 1; m <= 6; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 4; n++)//go through all the second spaces in the matrix (7 but we only need to get to 4)
                {
                    //check if there is a the threat from p1 
                    if (((board[m, n] + board[m, n + 1] + board[m, n + 2] + board[m, n + 3]) == 6) //checks if the sum of that point and three points around it are equal to 3
                    &&
                    ((board[m, n] != 1) && (board[m, n + 1] != 1) && (board[m, n + 2] != 1) && (board[m, n + 3] != 1)))//and none of them are equal to 2
                    {
                        for (int i = 0; i <= 3; i++)//go through all 4 spaces to find one that is equal to zero
                        {
                            if ((board[m, n + i] == 0) && ((board[m + 1, n + i] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                            {
                                int[] retval = { n + i, 1 };//make a return value with the wining position and who is going to win
                                return retval;
                            }
                        }


                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] PreThreatP1Hor()//method that finds Pre threats for p1 Horizontally
        {
            //  Himanshi: "Himanshi is the best person in the world" * Edmond: "LOOOL... No Comment"*

            for (int m = 1; m <= 6; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 5; n++)//go through all the second spaces in the matrix (7 but we only need to get to 5)
                {
                    //check if there is a the threat from p1 
                    if (((board[m, n] + board[m, n + 1] + board[m, n + 2] == 2) //checks if the sum of that point and two points around it are equal to 2, 1 for the full ones and zero for the empty
                    &&
                    ((board[m, n] != 2) && (board[m, n + 1] != 2) && (board[m, n + 2] != 2))))//and none of them are equal to 2
                    {
                        for (int i = 0; i <= 2; i++)//go through all 3 spaces to find one that is equal to zero
                        {
                            if ((board[m, n + i] == 0) && ((board[m + 1, n + i] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                            {
                                int[] retval = { n + i, m };//make a return value with the wining position and who is going to win
                                return GoodFuture(retval);//check if this action is favourable
                            }
                        }


                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] PreThreatP2Hor()//method that finds Pre threats for p2 Horizontally
        {
            for (int m = 1; m <= 6; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 5; n++)//go through all the second spaces in the matrix (7 but we only need to get to 5)
                {
                    //check if there is a the threat from p1 
                    if (((board[m, n] + board[m, n + 1] + board[m, n + 2] == 4) //checks if the sum of that point and two points around it are equal to 2, 1 for the full ones and zero for the empty
                    &&
                    ((board[m, n] != 1) && (board[m, n + 1] != 1) && (board[m, n + 2] != 1))))//and none of them are equal to 2
                    {
                        for (int i = 0; i <= 2; i++)//go through all 3 spaces to find one that is equal to zero
                        {
                            if ((board[m, n + i] == 0) && ((board[m + 1, n + i] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                            {
                                int[] retval = { n + i, m };//make a return value with the wining position and who is going to win
                                return GoodFuture(retval);//check if this action is favourable
                            }
                        }


                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] ThreatP1Ver()//method that finds threats for p1 Vertically
        {
            for (int m = 1; m <= 3; m++)//go through all the first spaces in the matrix (6) *Stop at three for M*
            {
                for (int n = 1; n <= 7; n++)//go through all the second spaces in the matrix (7)
                {
                    //check if there is a threat from p1 
                    if (((board[m, n] + board[m + 1, n] + board[m + 2, n] + board[m + 3, n]) == 3) //checks if the sum of that point and three points around it are equal to 3
                    && 
                    ((board[m, n] != 2) && (board[m + 1, n] !=2) && (board[m + 2, n] != 2) && (board[m + 3, n] != 2))//and none of them are equal to 2
                    &&
                    (m > 0))//make sure that it can't play above the board
                    {
                                int[] retval = { n, 1 };//make a return value with the wining position and who is going to win
                                return retval;
                    }
          }
         }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] ThreatP2Ver()//method that finds threats for p2 Vertically
        {
            for (int m = 1; m <= 3; m++)//go through all the first spaces in the matrix (6) *Stop at three for M*
            {
                for (int n = 1; n <= 7; n++)//go through all the second spaces in the matrix (7)
                {
                    //check if there is a threat from p1 
                    if (((board[m, n] + board[m + 1, n] + board[m + 2, n] + board[m + 3, n]) == 6) //checks if the sum of that point and three points around it are equal to 3
                    &&
                    ((board[m, n] != 1) && (board[m + 1, n] != 1) && (board[m + 2, n] != 1) && (board[m + 3, n] != 1))//and none of them are equal to 2
                    &&
                    (m > 0))//make sure that it can't play above the board
                    {
                        int[] retval = { n, 2 };//make a return value with the wining position and who is going to win
                        return retval;
                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] PreThreatP1Ver()//method that finds Pre threats for p1 Vertically
        {
            for (int m = 1; m <= 4; m++)//go through all the first spaces in the matrix (6) *Stop at three for M*
            {
                for (int n = 1; n <= 7; n++)//go through all the second spaces in the matrix (7)
                {
                    //check if there is a threat from p1 
                    if (((board[m, n] + board[m + 1, n] + board[m + 2, n]) == 2) //checks if the sum of that point and 2 points around it are equal to 2
                    &&
                    (board[m, n] != 2) && (board[m + 1, n] != 2) && (board[m + 2, n] != 2)//and none of them are equal to 2
                    &&
                    (m > 0))//make sure that it can't play above the board 
                    {
                        for (int i = 0; i <= 2; i++)//go through all 3 spaces to find one that is equal to zero
                        {

                            if ((board[m - i, n] == 0) && ((board[(m - i) + 1, n] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                            {
                                int[] retval = { n, m - i};//make a return value with the wining position and who is going to win
                                return GoodFuture(retval);//check if action is favourable
                            }
                        }
                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] PreThreatP2Ver()//method that finds Pre threats for p2 Vertically
        {
            for (int m = 1; m <= 4; m++)//go through all the first spaces in the matrix (6) *Stop at 4 for pre M*
            {
                for (int n = 1; n <= 7; n++)//go through all the second spaces in the matrix (7)
                {
                    //check if there is a threat from p2 
                    if (((board[m, n] + board[m + 1, n] + board[m + 2, n]) == 4) //checks if the sum of that point and 2 points around it are equal to 4
                    &&
                    ((board[m, n] != 1) && (board[m + 1, n] != 1) && (board[m + 2, n] != 1))//and none of them are equal to 1
                    &&
                    (m > 0))//make sure that it can't play above the board
                    {
                        for (int i = 0; i <= 2; i++)//go through all 3 spaces to find one that is equal to zero
                        {

                            if ((board[m - i, n] == 0) && ((board[(m - i) + 1, n] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                            {
                                int[] retval = { n, m - i };//make a return value with the wining position and who is going to win
                                return GoodFuture(retval);//check if action is favourable
                            }
                        }
                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] ThreatP1Dwn()//method that finds threats for p1 diagonally down
        {
            for (int m = 1; m <= 3; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 4; n++)//go through all the second spaces in the matrix (7)
                {
                    //check if there is a the threat from p1 
                    if (((board[m, n] + board[m + 1, n + 1] + board[m + 2, n + 2] + board[m + 3, n + 3]) == 3) //checks if the sum of that point and three points around it are equal to 3
                    &&
                    ((board[m, n] != 2) && (board[m + 1, n + 1] != 2) && (board[m + 2, n + 2] != 2) && (board[m + 3, n + 3] != 2)))//and none of them are equal to 2
                    {
                        for (int i = 0; i <= 3; i++)//go through all 4 spaces to find one that is equal to zero
                        {
                            if ((board[m + i, n + i] == 0) && ((board[(m + i) + 1, n + i] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                            {
                                int[] retval = { n + i, 1 };//make a return value with the wining position and who is going to win
                                return retval;
                            }
                        }


                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] ThreatP2Dwn()//method that finds threats for p2 diagonally down
        {
            for (int m = 1; m <= 3; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 4; n++)//go through all the second spaces in the matrix (7)
                {
                    //check if there is a the threat from p1 
                    if (((board[m, n] + board[m + 1, n + 1] + board[m + 2, n + 2] + board[m + 3, n + 3]) == 6) //checks if the sum of that point and three points around it are equal to 3
                    &&
                    ((board[m, n] != 1) && (board[m + 1, n + 1] != 1) && (board[m + 2, n + 2] != 1) && (board[m + 3, n + 3] != 1)))//and none of them are equal to 2
                    {
                        for (int i = 0; i <= 3; i++)//go through all 4 spaces to find one that is equal to zero
                        {
                            if ((board[m + i, n + i] == 0) && ((board[(m + i) + 1, n + i] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                            {
                                int[] retval = { n + i, 1 };//make a return value with the wining position and who is going to win
                                return retval;
                            }
                        }


                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] PreThreatP1Dwn()//method that finds Pre threats for p1 diagonally down
        {
            for (int m = 1; m <= 4; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 5; n++)//go through all the second spaces in the matrix (7)
                {
                    //check if there is a the threat from p1 
                    if (((board[m, n] + board[m + 1, n + 1] + board[m + 2, n + 2] == 2)) //checks if the sum of that point and two other points around it are equal to 2
                    &&
                    ((board[m, n] != 2) && (board[m + 1, n + 1] != 2) && (board[m + 2, n + 2] != 2)))//and none of them are equal to 2
                    {
                        for (int i = 0; i <= 2; i++)//go through all 3 spaces to find one that is equal to zero
                        {
                            if ((board[m + i, n + i] == 0) && ((board[(m + i) + 1, n + i] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                            {
                                int[] retval = { n + i, m + i};//make a return value with the wining position and who is going to win
                                return GoodFuture(retval);//check if action is favourable
                            }
                        }


                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] PreThreatP2Dwn()//method that finds threats for p2 diagonally down
        {
            for (int m = 1; m <= 4; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 5; n++)//go through all the second spaces in the matrix (7)
                {
                    //check if there is a the threat from p2 
                    if (((board[m, n] + board[m + 1, n + 1] + board[m + 2, n + 2] == 4) //checks if the sum of that point and two other points around it are equal to 4
                    &&
                    ((board[m, n] != 1) && (board[m + 1, n + 1] != 1) && (board[m + 2, n + 2] != 1))))//and none of them are equal to 1
                    {
                        for (int i = 0; i <= 2; i++)//go through all 3 spaces to find one that is equal to zero
                        {
                            if ((board[m + i, n + i] == 0) && ((board[(m + i) + 1, n + i] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                            {
                                int[] retval = { n + i, m + i};//make a return value with the wining position and who is going to win
                                return GoodFuture(retval);//check if action is favourable
                            }
                        }


                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] ThreatP1Up()//method that finds threats for p1 diagonally Up
        {
            for (int m = 4; m <= 6; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 4; n++)//go through all the second spaces in the matrix (7)
                {
                    //if (((m - 1) > 0) && ((m - 2) > 0) && ((m - 3) > 0))
                    //{
                        //check if there is a the threat from p1 
                        if (((board[m, n] + board[m - 1, n + 1] + board[m - 2, n + 2] + board[m - 3, n + 3]) == 3) //checks if the sum of that point and three points around it are equal to 3
                        &&
                        ((board[m, n] != 2) && (board[m - 1, n + 1] != 2) && (board[m - 2, n + 2] != 2) && (board[m - 3, n + 3] != 2)))//and none of them are equal to 2
                        {
                            for (int i = 0; i <= 3; i++)//go through all 4 spaces to find one that is equal to zero
                            {
                                if ((board[m - i, n + i] == 0) && ((board[(m - i) + 1, n + i] != 0)))//of that space is empty and the one below it is full then play it
                                {
                                    int[] retval = { n + i, 1 };//make a return value with the wining position and who is going to win
                                    //MessageBox.Show((n + i).ToString() + "," + (m - i).ToString());//debug
                                    return retval;
                                }
                            }


                       // }
                    }
                 }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] ThreatP2Up()//method that finds threats for p2 diagonally Up
        {
            for (int m = 4; m <= 6; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 4; n++)//go through all the second spaces in the matrix (7)
                {
                    //if (((m - 1) > 0) && ((m - 2) > 0) && ((m - 3) > 0))
                   // {
                        //check if there is a the threat from p1 
                        if (((board[m, n] + board[m - 1, n + 1] + board[m - 2, n + 2] + board[m - 3, n + 3]) == 6) //checks if the sum of that point and three points around it are equal to 3
                        &&
                        ((board[m, n] != 1) && (board[m - 1, n + 1] != 1) && (board[m - 2, n + 2] != 1) && (board[m - 3, n + 3] != 1)))//and none of them are equal to 2
                        {
                            for (int i = 0; i <= 3; i++)//go through all 4 spaces to find one that is equal to zero
                            {
                                if ((board[m - i, n + i] == 0) && ((board[(m - i) + 1, n + i] != 0)))//of that space is empty and the one below it is full then play it
                                {
                                    int[] retval = { n + i, 1 };//make a return value with the wining position and who is going to win
                                    //MessageBox.Show((n+i).ToString() + "," + (m - i).ToString());//debug
                                    return retval;
                                }
                            }


                        //}
                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] PreThreatP1Up()//method that finds threats for p1 diagonally Up
        {
            for (int m = 4; m <= 6; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 5; n++)//go through all the second spaces in the matrix (7)
                {
                    //if (((m - 1) > 0) && ((m - 2) > 0) && ((m - 3) > 0))
                    //{
                        //check if there is a the threat from p1 
                        if (((board[m, n] + board[m - 1, n + 1] + board[m - 2, n + 2] == 2) //checks if the sum of that point and two other points around it are equal to 2
                        &&
                        ((board[m, n] != 2) && (board[m - 1, n + 1] != 2) && (board[m - 2, n + 2] != 2))))//and none of them are equal to 2
                        {
                            for (int i = 0; i <= 2; i++)//go through all 3 spaces to find one that is equal to zero
                            {
                                if ((board[m - i, n + i] == 0) && ((board[(m - i) + 1, n + i] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                                {
                                    int[] retval = { n + i, m - i};//make a return value with the wining position and who is going to win
                                    return GoodFuture(retval);//check if action is favourable
                                }
                            }


                       // }
                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }

        private int[] PreThreatP2Up()//method that finds threats for p2 diagonally Up
        {
            for (int m = 4; m <= 6; m++)//go through all the first spaces in the matrix (6)
            {
                for (int n = 1; n <= 5; n++)//go through all the second spaces in the matrix (7)
                {
                   // if (((m - 1) > 0) && ((m - 2) > 0) && ((m - 3) > 0))
                   // {
                        //check if there is a the threat from p2 
                        if (((board[m, n] + board[m - 1, n + 1] + board[m - 2, n + 2] == 4) //checks if the sum of that point and two other points around it are equal to 4
                        &&
                        ((board[m, n] != 1) && (board[m - 1, n + 1] != 1) && (board[m - 2, n + 2] != 1))))//and none of them are equal to 1
                        {
                            for (int i = 0; i <= 2; i++)//go through all 3 spaces to find one that is equal to zero
                            {
                                if ((board[m - i, n + i] == 0) && ((board[(m - i) + 1, n + i] != 0) || (m == 6)))//of that space is empty and the one below it is full then play it
                                {
                                    int[] retval = { n + i, m - i };//make a return value with the wining position and who is going to win
                                    return GoodFuture(retval);//check if action is favourable
                                }
                            }


                     //   }
                    }
                }
            }
            int[] retvol = { 3, 3 };
            return retvol;
        }
    
        
        private int[] GoodFuture(int[] coord)//if the future outcome is good it returns the original coordinates, else it returns (3,3) 
                                                         //gets the the current board and the position for a new point to be added. 
                                                          //usuall for pre threats
        {
            //because progamming is wierd having an array just equal another array makes a reference :(
            int[,] boardx = (int[,]) board.Clone();//clones a previous array into this new one
            int[,] boardy = (int[,])board.Clone();//clones a previous array into this new one

           //get the m and n coordinates
            //values switched
           int n = coord[0];
           int m = coord[1];

            int altTurn;

            //used this to get the other turn
            if (turn == 1)//1 or 2
            {
                altTurn = 2;
            }
            else
            {
                altTurn = 1;
            }
            
           //simulate what would happen when the computer played  
           //board[m, n] = turn; *no need for this code*
           
           //put the opponents thing on the other board
            boardx[m - 1, n] = altTurn;//put the opponents ball in the space above the board we are aiming for
            boardy[m - 1, n] = turn;//put my ball in the space above the board we are aiming for


           //unfavourable outcomes
           //if the opponent wins after we play
            if (WinCheck(boardx) == altTurn)
            {
                int[] val = { 3, 3 };//not favourable
                return val;
            }
            else if(WinCheck(boardy) == turn)//if i have a chance to win after i play, then dont play
            {
                int[] val = { 3, 3 };//not favourable
                return val;
            }
            else
            {
                coord[1] = 1;//if nothing is wrong then return the coorinate that we put in
                return coord;
            }
        }

        //mouse functions for all the first controls
        private void p15_MouseEnter(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 5] == 0)//if there is nothing there
            {
                if (turn == 1)
                {
                    p15.BackgroundImage = Properties.Resources.Ball_Red;
                }
                else if (turn == 2)
                {
                    p15.BackgroundImage = Properties.Resources.Ball_Blue;
                }
            }
            else
            {
                Show(board);
            }
        }

        private void p15_MouseLeave(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 5] == 0)//if there is nothing there
            {
                p15.BackgroundImage = null;
            }
            else
            {
                Show(board);
            }
        }

        private void p16_Click(object sender, EventArgs e)
        {
           

            if (ready)//if the player can play
            {
                n = 6;//for the fifth part
                TimerFall.Start();
            }
        }

        private void p16_MouseEnter_1(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 6] == 0)//if there is nothing there
            {
                if (turn == 1)
                {
                    p16.BackgroundImage = Properties.Resources.Ball_Red;
                }
                else if (turn == 2)
                {
                    p16.BackgroundImage = Properties.Resources.Ball_Blue;
                }
            }
            else
            {
                Show(board);
            }
        }

        private void p16_MouseLeave_1(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 6] == 0)//if there is nothing there
            {
                p16.BackgroundImage = null;
            }
            else
            {
                Show(board);
            }
        }

        private void p17_Click(object sender, EventArgs e)
        {
            

            if (ready)//if the player can play
            {
                n = 7;//for the fifth part
                TimerFall.Start();
            }
            
        }

        private void p17_MouseEnter(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 7] == 0)//if there is nothing there
            {
                if (turn == 1)
                {
                    p17.BackgroundImage = Properties.Resources.Ball_Red;
                }
                else if (turn == 2)
                {
                    p17.BackgroundImage = Properties.Resources.Ball_Blue;
                }
            }
            else
            {
                Show(board);
            }
        }

        private void p17_MouseLeave(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 7] == 0)//if there is nothing there
            {
                p17.BackgroundImage = null;
            }
            else
            {
                Show(board);
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            

            if (ready)//if the player can play
            {
                n = 4;//for the fifth part
                TimerFall.Start();
            }
        }

        private void p14_MouseEnter(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 4] == 0)//if there is nothing there
            {
                if (turn == 1)
                {
                    p14.BackgroundImage = Properties.Resources.Ball_Red;
                }
                else if (turn == 2)
                {
                    p14.BackgroundImage = Properties.Resources.Ball_Blue;
                }
            }
            else
            {
                Show(board);
            }
        }

        private void p14_MouseLeave(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 4] == 0)//if there is nothing there
            {
                p14.BackgroundImage = null;
            }
            else
            {
                Show(board);
            }
        }

        private void p13_Click(object sender, EventArgs e)
        {
            

            if (ready)//if the player can play
            {
                n = 3;//for the fifth part
                TimerFall.Start();
            }
            
        }

        private void p13_MouseEnter(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 3] == 0)//if there is nothing there
            {
                if (turn == 1)
                {
                    p13.BackgroundImage = Properties.Resources.Ball_Red;
                }
                else if (turn == 2)
                {
                    p13.BackgroundImage = Properties.Resources.Ball_Blue;
                }
            }
            else
            {
                Show(board);
            }
        }

        private void p13_MouseLeave(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 3] == 0)//if there is nothing there
            {
                p13.BackgroundImage = null;
            }
            else
            {
                Show(board);
            }
        }

        private void p12_Click(object sender, EventArgs e)
        {
            

            if (ready)//if the player can play
            {
                n = 2;//for the fifth part
                TimerFall.Start();
            }
            
        }

        private void p12_MouseEnter(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 2] == 0)//if there is nothing there
            {
                if (turn == 1)
                {
                    p12.BackgroundImage = Properties.Resources.Ball_Red;
                }
                else if (turn == 2)
                {
                    p12.BackgroundImage = Properties.Resources.Ball_Blue;
                }
            }
            else
            {
                Show(board);
            }
        }

        private void p12_MouseLeave(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 2] == 0)//if there is nothing there
            {
                p12.BackgroundImage = null;
            }
            else
            {
                Show(board);
            }
        }

        private void p11_Click(object sender, EventArgs e)
        {
            

            if (ready)//if the player can play
            {
                n = 1;//for the fifth part
                TimerFall.Start();
            }
        }

        private void p11_MouseEnter(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 1] == 0)//if there is nothing there
            {
                if (turn == 1)
                {
                    p11.BackgroundImage = Properties.Resources.Ball_Red;
                }
                else if (turn == 2)
                {
                    p11.BackgroundImage = Properties.Resources.Ball_Blue;
                }
            }
            else
            {
                Show(board);
            }
        }

        private void p11_MouseLeave(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 1] == 0)//if there is nothing there
            {
                p11.BackgroundImage = null;
            }
            else
            {
                Show(board);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            board = Zero(board);//set the board to zero 
            Show(board);//show the board
            ready = true;

            //setup who goes first
            place = rand.Next(0, 4);

            turn = 1;

            TimerFall.Stop();


            if (mode == "Single Player")//starts the AI if it is needed
            {
                Amode = true;
                Aturn = false;

                //set p1 and p2
                labelP1.Text = p1;
                labelP2.Text = "Computer";

                //used to set up turns
                if (place % 2 == 1)//P1 Red, P2 blue
                {
                    labelP1.BackColor = Color.Blue;
                    labelP2.BackColor = Color.Red;
                    //ai plays
                    humanDone();

                }
                else//P1 Blue, P2 Red
                {
                    labelP1.BackColor = Color.Red;
                    labelP2.BackColor = Color.Blue;

                    Aturn = true;
                }
            }
            else
            {

                //set p1 and p2
                labelP1.Text = p1;
                labelP2.Text = p2;

                //used to set up turns
                if (place % 2 == 1)//P2 Blue, P1 Red
                {
                    labelP1.BackColor = Color.Blue;
                    labelP2.BackColor = Color.Red;

                    MessageBox.Show(p2 + " goes first");
                }
                else//P1 Blue, P2 Red
                {
                    labelP1.BackColor = Color.Red;
                    labelP2.BackColor = Color.Blue;

                    MessageBox.Show(p1 + " goes first");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)//turn Ai mode on and off
        {
            /*
            if (Amode == false)//if mode is off
            {
                Amode = true;
                Aturn = true;
                button2.Text = "Turn AI Off";
            }
            else //if mode is on
            {
                Amode = false;
                button2.Text = "Turn AI On";
            }
             */

            //quit
           // Application.Exit();
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 1] == 0)//if there is nothing there
            {
                if (turn == 1)
                {
                    p11.BackgroundImage = Properties.Resources.Ball_Red;
                }
                else if (turn == 2)
                {
                    p11.BackgroundImage = Properties.Resources.Ball_Blue;
                }
            }
            else
            {
                Show(board);
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            //show who is going next
            if (board[1, 1] == 0)//if there is nothing there
            {
                p11.BackgroundImage = null;
            }
            else
            {
                Show(board);
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {


            if (ready)//if the player can play
            {
                n = 1;//for the fifth part
                TimerFall.Start();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
