using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TicTacToe
{

    public partial class main : System.Web.UI.Page
    {
        TicTacToe[] _PlayBoard;
        int[] _BitValues = new int[] { 1, 2, 4, 8, 16, 32, 64, 128, 256 };
        int[] _WinningValues = new int[] { 7, 56, 448, 73, 146, 292, 273, 84 };


        protected void Page_Load(object sender, EventArgs e)
        {
            //FIRST TIME LOADING
            if (!IsPostBack)
            {
                ViewState["CurrentPlayer"] = Players.X;
                ViewState["PlayerOne"] = 0;
                ViewState["PlayerTwo"] = 0;
                ViewState["PlayerOneWins"] = 0;
                ViewState["PlayerTwoWins"] = 0;
            }
            //Making a Panel To Hold the Buttons                
            Panel Panel1 = new Panel();
            Panel1.ID = "GameBoard";
            Panel1.BackColor = System.Drawing.Color.LightGreen;
            Panel1.Style[HtmlTextWriterStyle.Left] = "500px";
            Panel1.Style[HtmlTextWriterStyle.Top] = "500px";
            Panel1.Style[HtmlTextWriterStyle.Width] = "180px";
            Panel1.Style[HtmlTextWriterStyle.Height] = "180px";
            //Creating the Buttons then Adding them into the Panel
            _PlayBoard = new TicTacToe[9];
            for (int i = 0; i < _PlayBoard.Length; i++)
            {
                _PlayBoard[i] = new TicTacToe(_BitValues[i]);
                _PlayBoard[i].Style[HtmlTextWriterStyle.Width] = "60px";
                _PlayBoard[i].Style[HtmlTextWriterStyle.Height] = "60px";
                _PlayBoard[i].Text = " ";
                _PlayBoard[i].Click += Main_Click;
                Panel1.Controls.Add(_PlayBoard[i]);
            }
            //Adding the Panel to the Page
            form1.Controls.Add(Panel1);
            //Showing Who is the Current Player
            Label currentPlayer = new Label();
            currentPlayer.Text = "The Current Player is: Player " + ViewState["CurrentPlayer"].ToString();
            currentPlayer.ID = "currentPlayer";
            form1.Controls.Add(currentPlayer);
            form1.Controls.Add(new LiteralControl("<br/>"));
            //Creating Labels for the Players to See the Points
            Label playerOne = new Label();
            playerOne.ID = "playerOne";
            playerOne.Text = "Player One Score: " + ViewState["PlayerOneWins"].ToString();
            form1.Controls.Add(playerOne);
            form1.Controls.Add(new LiteralControl("<br/>"));
            Label playerTwo = new Label();
            playerTwo.ID = "playerTwo";
            playerTwo.Text = "Player Two Score: " + ViewState["PlayerTwoWins"].ToString();
            form1.Controls.Add(playerTwo);
        }

        private void Main_Click(object sender, EventArgs e)
        {
            bool isGameOver = false;
            //TODO check to see if the game needs to be resete
            if ((Players)ViewState["CurrentPlayer"] == Players.X)
            {
                ((TicTacToe)sender).Text = "X";
                ViewState["PlayerOne"] = ((TicTacToe)sender).BitValue + (int)ViewState["PlayerOne"];
                isGameOver = CheckWinning((int)ViewState["PlayerOne"]);
                if (isGameOver == false)
                    ViewState["CurrentPlayer"] = Players.O;
            }
            else
            {
                ((TicTacToe)sender).Text = "O";
                ViewState["PlayerTwo"] = ((TicTacToe)sender).BitValue + (int)ViewState["PlayerTwo"];
                isGameOver = CheckWinning((int)ViewState["PlayerTwo"]);
                if (isGameOver == false)
                    ViewState["CurrentPlayer"] = Players.X;
            }
            //Reset the board state
            if (isGameOver)
                NewGame();
            else
                //Show the User whos turn it is
                ((Label)form1.FindControl("currentPlayer")).Text = "The Current Player is: Player " + ViewState["CurrentPlayer"].ToString();
        }

        private void NewGame()
        {
            //Reset the board by finding all the buttons and changing the text to nothing
            Panel tmp = (Panel)form1.FindControl("GameBoard");
            //Check to see if I found the panel
            if (tmp == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Error')", true);
                Response.Redirect(Request.RawUrl);
                return;
            }
            foreach (var item in tmp.Controls)
            {
                if (item as Button != null)
                    ((Button)item).Text = " ";
            }
            //Reset the point values and the default player back to x
            ViewState["CurrentPlayer"] = Players.X;
            ViewState["PlayerOne"] = 0;
            ViewState["PlayerTwo"] = 0;
            //Update the Labels for the Players
            Label playerOne = (Label)form1.FindControl("playerOne");
            Label playerTwo = (Label)form1.FindControl("playerTwo");
            Label currentPlayer = (Label)form1.FindControl("currentPlayer");
            playerOne.Text = "Player One Score: " + ViewState["PlayerOneWins"].ToString();
            playerTwo.Text = "Player Two Score: " + ViewState["PlayerTwoWins"].ToString();
            currentPlayer.Text = "The Current Player is: Player " + ViewState["CurrentPlayer"].ToString();
        }

        private bool CheckWinning(int value)
        {
            //The value needs to be bit and to find the value
            for (int i = 0; i < _WinningValues.Length; i++)
            {
                int tmpValue = value;
                if ((tmpValue &= _WinningValues[i]) == _WinningValues[i])
                {
                    ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Player " + ViewState["CurrentPlayer"].ToString() + " has won')", true);
                    if ((Players)ViewState["CurrentPlayer"] == Players.O)
                        ViewState["PlayerTwoWins"] = (int)ViewState["PlayerTwoWins"] + 1;
                    else
                        ViewState["PlayerOneWins"] = (int)ViewState["PlayerOneWins"] + 1;
                    return true;
                }
            }
            //Check to see if the board is full -- Tie game
            //Reset the board by finding all the buttons 
            Panel tmp = (Panel)form1.FindControl("GameBoard");
            if (tmp == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Error')", true);
                Response.Redirect(Request.RawUrl);
                return false;
            }
            foreach (var item in tmp.Controls)
            {
                //If at least one has a space in it the game isn't over
                if (item as Button != null)
                    if (((Button)item).Text.Equals(" "))
                        return false;
            }
            //The game is tied with no winners
            ClientScript.RegisterStartupScript(GetType(), "alert", "alert('The Game has ended in a tie')", true);
            NewGame();
            return true;
        }
    }

    public class TicTacToe : Button
    {
        public int BitValue { get; }

        public TicTacToe(int bitValue)
        {
            BitValue = bitValue;
        }
    }

    public enum Players
    {
        X = 1,
        O
    }
}