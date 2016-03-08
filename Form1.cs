﻿using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace MultipleJoysticks
{
    public partial class Form1 : Form
    {
        bool[] AutonomousMode = { true, true, true, true, true, true };
        bool[] TeleOp = { false, false, false, false, false, false };
        bool[] FinshedScoring = { false, false, false, false, false, false };

        //Arrays that hold the values for the made and attempt numbers of the frisbee scoring
        // 1 point frisbees
        int[] displayLowFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] lowFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] displayLowFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] lowFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayLowFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] autoLowFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayLowFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] autoLowFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };

        // 2 point frisbees
        int[] displayMidFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] midFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] displayMidFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] midFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayMidFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] autoMidFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayMidFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] autoMidFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };

        // 3 point frisbees
        int[] displayDefense2Cross = { 0, 0, 0, 0, 0, 0 };
        int[] defense2Cross = { 0, 0, 0, 0, 0, 0 };
        int[] displayDefense2Att = { 0, 0, 0, 0, 0, 0 };
        int[] defense2Att = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayDefense2Cross = { 0, 0, 0, 0, 0, 0 };
        int[] autoDefense2Cross = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayDefense2Reach = { 0, 0, 0, 0, 0, 0 };
        int[] autoDefense2Reach = { 0, 0, 0, 0, 0, 0 };

        // Pyramid Frisbees
        int[] displayPyramidFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] pyramidFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] displayPyramidFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] pyramidFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };

        // Robot Climbing
        int[] climb = { 0, 0, 0, 0, 0, 0 };
        int[] challengeScale = { 0, 0, 0, 0, 0, 0 };

        // Total Points
        int[] teleOpTotalPoints = { 0, 0, 0, 0, 0, 0 };
        int[] autoTotalPoints = { 0, 0, 0, 0, 0, 0 };

        // Defense Ratings
        int[] defenseRating = { 0, 0, 0, 0, 0, 0 };
        int[] displayDefenseRating = { 0, 0, 0, 0, 0, 0 };

        // Declaration of auto-filled team numbers.
        static int autoTeams = 0;
        int[] AutoTeamNo1;
        int[] AutoTeamNo2;
        int[] AutoTeamNo3;
        int[] AutoTeamNo4;
        int[] AutoTeamNo5;
        int[] AutoTeamNo6;

        //Keeps track of the match.
        static int match = 1;

        String fileName = "test1";
        String[] teamsNotePad;
        String x = ",";
        bool SavePromptActive = false;

        struct GameCommands
        {
            public const int TeleOp = 0;
            public const int Autonomous = 1;
            public const int scoreHigh = 2;
            public const int scoreLow = 3;
            public const int PyramidFrisbeesMadeMinus = 4;
            public const int PyramidFrisbeesMadePlus = 5;
            public const int PyramidFrisbeesAttMinus = 6;
            public const int PyramidFrisbeesAttPlus = 7;
            public const int Defense2CrossMinus = 8;
            public const int Defense2CrossPlus = 9;
            public const int Defense2AttMinus = 10;
            public const int Defense2AttPlus = 11;
            public const int MidFrisbeesMadeMinus = 12;
            public const int MidFrisbeesMadePlus = 13;
            public const int MidFrisbeesAttMinus = 14;
            public const int MidFrisbeesAttPlus = 15;
            public const int LowFrisbeesMadeMinus = 16;
            public const int LowFrisbeesMadePlus = 17;
            public const int LowFrisbeesAttMinus = 18;
            public const int LowFrisbeesAttPlus = 19;
            public const int ChallengeScalePlus = 20;
            public const int FinishedScoring = 21;
        }

        String[,] ControllerCommands = new String[6, 22];
        String[] LastButtonPattern = new String[6];
        Label[] displayButtons;

        // --- INITIALIZATION
        public Form1()
        {
            InitializeComponent();
            displayButtons = new[] { lbldisplayButtons1, lbldisplayButtons2, lbldisplayButtons3, lbldisplayButtons4, lbldisplayButtons5, lbldisplayButtons6 };

            var gameInput = new GameInput();
            var sticks = gameInput.GetSticks(this);
            if (sticks > 0)
            {
                timer1.Enabled = true;
                timer1.Tick += gameInput.timer1_Tick;
            }
            else
            {
                MessageBox.Show("No Joysticks found!", "Warning", MessageBoxButtons.OK);
            }
        }

        public void SetControllerCommands(int controllernumber, string[] Command, string buttons)
        {
            switch (Command[0].ToUpper())  //In this section, case names should be all uppercase
            {
                case "TELEOP":
                    ControllerCommands[controllernumber, Form1.GameCommands.TeleOp] = buttons;
                    break;
                case "AUTONOMOUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.Autonomous] = buttons;
                    break;
                case "DEFENSIVERATINGPLUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.scoreHigh] = buttons;
                    break;
                case "DEFENSIVERATINGMINUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.scoreLow] = buttons;
                    break;
                case "PYRAMIDFRISBEESMADEMINUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.PyramidFrisbeesMadeMinus] = buttons;
                    break;
                case "PYRAMIDFRISBEESMADEPLUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.PyramidFrisbeesMadePlus] = buttons;
                    break;
                case "PYRAMIDFRISBEESATTMINUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.PyramidFrisbeesAttMinus] = buttons;
                    break;
                case "PYRAMIDFRISBEESATTPLUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.PyramidFrisbeesAttPlus] = buttons;
                    break;
                case "DEFENSE2CROSSMINUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.Defense2CrossMinus] = buttons;
                    break;
                case "DEFENSE2CROSSPLUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.Defense2CrossPlus] = buttons;
                    break;
                case "DEFENSE2ATTMINUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.Defense2AttMinus] = buttons;
                    break;
                case "DEFENSE2ATTPLUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.Defense2AttPlus] = buttons;
                    break;
                case "MIDFRISBEESMADEMINUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.MidFrisbeesMadeMinus] = buttons;
                    break;
                case "MIDFRISBEESMADEPLUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.MidFrisbeesMadePlus] = buttons;
                    break;
                case "MIDFRISBEESATTMINUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.MidFrisbeesAttMinus] = buttons;
                    break;
                case "MIDFRISBEESATTPLUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.MidFrisbeesAttPlus] = buttons;
                    break;
                case "LOWFRISBEESMADEMINUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.LowFrisbeesMadeMinus] = buttons;
                    break;
                case "LOWFRISBEESMADEPLUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.LowFrisbeesMadePlus] = buttons;
                    break;
                case "LOWFRISBEESATTMINUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.LowFrisbeesAttMinus] = buttons;
                    break;
                case "LOWFRISBEESATTPLUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.LowFrisbeesAttPlus] = buttons;
                    break;
                case "CHALLENGESCALEPLUS":
                    ControllerCommands[controllernumber, Form1.GameCommands.ChallengeScalePlus] = buttons;
                    break;
                case "FINISHEDSCORING":
                    ControllerCommands[controllernumber, Form1.GameCommands.FinishedScoring] = buttons;
                    break;
            }
        }

        // --- OPPERATION

        public void UseButtonMap(int id, string strButtonMap)
        {
            bool FinshedScoringNeedtoSave = true;

            if (!strButtonMap.Equals(LastButtonPattern[id]))
            {
                tm1939ProcessButton(strButtonMap, id);
                LastButtonPattern[id] = strButtonMap;
                for (int i = 0; i < 6; i++)
                    if (FinshedScoring[i] == false)
                        FinshedScoringNeedtoSave = false;
                if (FinshedScoringNeedtoSave && SavePromptActive == false)
                {
                    SavePromptActive = true;
                    if (MessageBox.Show("Are you ready to save?", "Save", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        SaveDataBtn.PerformClick();
                    SavePromptActive = false;
                }
                UpdateScores(id);
                tm1939RefreshScreen(id);
            }
        }

        void UpdateScores(int id)
        {
            autoTotalPoints[id] = (autoDefense2Cross[id] * 6) +
                    (autoMidFrisbeesMade[id] * 4) +
                    (autoLowFrisbeesMade[id] * 2);
            teleOpTotalPoints[id] = (pyramidFrisbeesMade[id] * 5) +
                    (defense2Cross[id] * 3) +
                    (midFrisbeesMade[id] * 2) +
                    lowFrisbeesMade[id];

        }

        void tm1939RefreshScreen(int id)
        {
            switch (id)
            {
                case 0:
                    tm1939UpdateController0();
                    break;

                case 1:
                    tm1939UpdateController1();
                    break;

                case 2:
                    tm1939UpdateController2();
                    break;

                case 3:
                    tm1939UpdateController3();
                    break;

                case 4:
                    tm1939UpdateController4();
                    break;

                case 5:
                    tm1939UpdateController5();
                    break;
            }

        }
        void tm1939UpdateController0()
        {
            if (TeleOp[0])
            {
                lblAuto.Visible = false;
                lblTeleOp.Visible = true;
                lblAutoD2Reach.Visible = false;
                lblTeleOpD2Att.Visible = true;
                lblAutoD2Cross.Visible = false;
                lblTeleOpD2Cross.Visible = true;
                lblAutoMidAtt.Visible = false;
                lblTeleOpMidAtt.Visible = true;
                lblAutoMidMade.Visible = false;
                lblTeleOpMidMade.Visible = true;
                lblAutoLowAtt.Visible = false;
                lblTeleOpLowAtt.Visible = true;
                lblAutoLowMade.Visible = false;
                lblTeleOpLowMade.Visible = true;
                lblTeleOpPyramidAtt.Visible = true;
                lblTeleOpPyramidMade.Visible = true;
                lblChallengeScale.Visible = true;
            }
            if (AutonomousMode[0])
            {
                lblAuto.Visible = true;
                lblTeleOp.Visible = false;
                lblAutoD2Reach.Visible = true;
                lblTeleOpD2Att.Visible = false;
                lblAutoD2Cross.Visible = true;
                lblTeleOpD2Cross.Visible = false;
                lblAutoMidAtt.Visible = true;
                lblTeleOpMidAtt.Visible = false;
                lblAutoMidMade.Visible = true;
                lblTeleOpMidMade.Visible = false;
                lblAutoLowAtt.Visible = true;
                lblTeleOpLowAtt.Visible = false;
                lblAutoLowMade.Visible = true;
                lblTeleOpLowMade.Visible = false;
                lblTeleOpPyramidAtt.Visible = false;
                lblTeleOpPyramidMade.Visible = false;
                lblChallengeScale.Visible = false;
            }
            //Defense Rating
            lblDefense.Text = displayDefenseRating[0].ToString();

            // Pyramid Goals
            lblTeleOpPyramidMade.Text = displayPyramidFrisbeesMade[0].ToString();
            lblTeleOpPyramidAtt.Text = displayPyramidFrisbeesAtt[0].ToString();

            // High Goals
            lblTeleOpD2Cross.Text = displayDefense2Cross[0].ToString();
            lblTeleOpD2Att.Text = displayDefense2Att[0].ToString();
            lblAutoD2Cross.Text = autoDisplayDefense2Cross[0].ToString();
            lblAutoD2Reach.Text = autoDisplayDefense2Reach[0].ToString();

            // Mid Goals
            lblTeleOpMidMade.Text = displayMidFrisbeesMade[0].ToString();
            lblTeleOpMidAtt.Text = displayMidFrisbeesAtt[0].ToString();
            lblAutoMidMade.Text = autoDisplayMidFrisbeesMade[0].ToString();
            lblAutoMidAtt.Text = autoDisplayMidFrisbeesAtt[0].ToString();

            // Low Goals
            lblTeleOpLowMade.Text = displayLowFrisbeesMade[0].ToString();
            lblTeleOpLowAtt.Text = displayLowFrisbeesAtt[0].ToString();
            lblAutoLowMade.Text = autoDisplayLowFrisbeesMade[0].ToString();
            lblAutoLowAtt.Text = autoDisplayLowFrisbeesAtt[0].ToString();

            // Robot Climb
            lblChallengeScale.Text = challengeScale[0].ToString();

            lblTeleOpTotalPoints.Text = teleOpTotalPoints[0].ToString();
            lblAutoTotalPoints.Text = autoTotalPoints[0].ToString();
            lblTotalPoints.Text = (autoTotalPoints[0] + teleOpTotalPoints[0] + challengeScale[0]).ToString();
            if (FinshedScoring[0])
                lblTeleOp.ForeColor = Color.DarkGreen;
            else
                lblTeleOp.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(36)))));



        }
        void tm1939UpdateController1()
        {
            if (TeleOp[1])
            {
                lblAuto2.Visible = false;
                lblTeleOp2.Visible = true;
                lblAutoD2Reach2.Visible = false;
                lblTeleOpD2Att2.Visible = true;
                lblAutoD2Cross2.Visible = false;
                lblTeleOpD2Cross2.Visible = true;
                lblAutoMidAtt2.Visible = false;
                lblTeleOpMidAtt2.Visible = true;
                lblAutoMidMade2.Visible = false;
                lblTeleOpMidMade2.Visible = true;
                lblAutoLowAtt2.Visible = false;
                lblTeleOpLowAtt2.Visible = true;
                lblAutoLowMade2.Visible = false;
                lblTeleOpLowMade2.Visible = true;
                lblTeleOpPyramidAtt2.Visible = true;
                lblTeleOpPyramidMade2.Visible = true;
                lblChallengeScale2.Visible = true;
            }
            if (AutonomousMode[1])
            {
                lblAuto2.Visible = true;
                lblTeleOp2.Visible = false;
                lblAutoD2Reach2.Visible = true;
                lblTeleOpD2Att2.Visible = false;
                lblAutoD2Cross2.Visible = true;
                lblTeleOpD2Cross2.Visible = false;
                lblAutoMidAtt2.Visible = true;
                lblTeleOpMidAtt2.Visible = false;
                lblAutoMidMade2.Visible = true;
                lblTeleOpMidMade2.Visible = false;
                lblAutoLowAtt2.Visible = true;
                lblTeleOpLowAtt2.Visible = false;
                lblAutoLowMade2.Visible = true;
                lblTeleOpLowMade2.Visible = false;
                lblTeleOpPyramidAtt2.Visible = false;
                lblTeleOpPyramidMade2.Visible = false;
                lblChallengeScale2.Visible = false;
            }
            //Defense Rating
            lblDefense2.Text = displayDefenseRating[1].ToString();

            // Pyramid Goals
            lblTeleOpPyramidMade2.Text = displayPyramidFrisbeesMade[1].ToString();
            lblTeleOpPyramidAtt2.Text = displayPyramidFrisbeesAtt[1].ToString();

            // High Goals
            lblTeleOpD2Cross2.Text = displayDefense2Cross[1].ToString();
            lblTeleOpD2Att2.Text = displayDefense2Att[1].ToString();
            lblAutoD2Cross2.Text = autoDisplayDefense2Cross[1].ToString();
            lblAutoD2Reach2.Text = autoDisplayDefense2Reach[1].ToString();

            // Mid Goals
            lblTeleOpMidMade2.Text = displayMidFrisbeesMade[1].ToString();
            lblTeleOpMidAtt2.Text = displayMidFrisbeesAtt[1].ToString();
            lblAutoMidMade2.Text = autoDisplayMidFrisbeesMade[1].ToString();
            lblAutoMidAtt2.Text = autoDisplayMidFrisbeesAtt[1].ToString();

            // Low Goals
            lblTeleOpLowMade2.Text = displayLowFrisbeesMade[1].ToString();
            lblTeleOpLowAtt2.Text = displayLowFrisbeesAtt[1].ToString();
            lblAutoLowMade2.Text = autoDisplayLowFrisbeesMade[1].ToString();
            lblAutoLowAtt2.Text = autoDisplayLowFrisbeesAtt[1].ToString();

            // Robot Climb
            lblChallengeScale2.Text = challengeScale[1].ToString();

            lblTeleOpTotalPoints2.Text = teleOpTotalPoints[1].ToString();
            lblAutoTotalPoints2.Text = autoTotalPoints[1].ToString();
            lblTotalPoints2.Text = (autoTotalPoints[1] + teleOpTotalPoints[1] + challengeScale[1]).ToString();
            if (FinshedScoring[1])
                lblTeleOp2.ForeColor = Color.DarkGreen;
            else
                lblTeleOp2.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(36)))));


        }
        void tm1939UpdateController2()
        {
            if (TeleOp[2])
            {
                lblAuto3.Visible = false;
                lblTeleOp3.Visible = true;
                lblAutoD2Reach3.Visible = false;
                lblTeleOpD2Att3.Visible = true;
                lblAutoD2Cross3.Visible = false;
                lblTeleOpD2Cross3.Visible = true;
                lblAutoMidAtt3.Visible = false;
                lblTeleOpMidAtt3.Visible = true;
                lblAutoMidMade3.Visible = false;
                lblTeleOpMidMade3.Visible = true;
                lblAutoLowAtt3.Visible = false;
                lblTeleOpLowAtt3.Visible = true;
                lblAutoLowMade3.Visible = false;
                lblTeleOpLowMade3.Visible = true;
                lblTeleOpPyramidAtt3.Visible = true;
                lblTeleOpPyramidMade3.Visible = true;
                lblChallengeScale3.Visible = true;
            }
            if (AutonomousMode[2])
            {
                lblAuto3.Visible = true;
                lblTeleOp3.Visible = false;
                lblAutoD2Reach3.Visible = true;
                lblTeleOpD2Att3.Visible = false;
                lblAutoD2Cross3.Visible = true;
                lblTeleOpD2Cross3.Visible = false;
                lblAutoMidAtt3.Visible = true;
                lblTeleOpMidAtt3.Visible = false;
                lblAutoMidMade3.Visible = true;
                lblTeleOpMidMade3.Visible = false;
                lblAutoLowAtt3.Visible = true;
                lblTeleOpLowAtt3.Visible = false;
                lblAutoLowMade3.Visible = true;
                lblTeleOpLowMade3.Visible = false;
                lblTeleOpPyramidAtt3.Visible = false;
                lblTeleOpPyramidMade3.Visible = false;
                lblChallengeScale3.Visible = false;
            }
            //Defense Rating
            lblDefense3.Text = displayDefenseRating[2].ToString();

            // Pyramid Goals
            lblTeleOpPyramidMade3.Text = displayPyramidFrisbeesMade[2].ToString();
            lblTeleOpPyramidAtt3.Text = displayPyramidFrisbeesAtt[2].ToString();

            // High Goals
            lblTeleOpD2Cross3.Text = displayDefense2Cross[2].ToString();
            lblTeleOpD2Att3.Text = displayDefense2Att[2].ToString();
            lblAutoD2Cross3.Text = autoDisplayDefense2Cross[2].ToString();
            lblAutoD2Reach3.Text = autoDisplayDefense2Reach[2].ToString();

            // Mid Goals
            lblTeleOpMidMade3.Text = displayMidFrisbeesMade[2].ToString();
            lblTeleOpMidAtt3.Text = displayMidFrisbeesAtt[2].ToString();
            lblAutoMidMade3.Text = autoDisplayMidFrisbeesMade[2].ToString();
            lblAutoMidAtt3.Text = autoDisplayMidFrisbeesAtt[2].ToString();

            // Low Goals
            lblTeleOpLowMade3.Text = displayLowFrisbeesMade[2].ToString();
            lblTeleOpLowAtt3.Text = displayLowFrisbeesAtt[2].ToString();
            lblAutoLowMade3.Text = autoDisplayLowFrisbeesMade[2].ToString();
            lblAutoLowAtt3.Text = autoDisplayLowFrisbeesAtt[2].ToString();

            // Robot Climb
            lblChallengeScale3.Text = challengeScale[2].ToString();

            lblTeleOpTotalPoints3.Text = teleOpTotalPoints[2].ToString();
            lblAutoTotalPoints3.Text = autoTotalPoints[2].ToString();
            lblTotalPoints3.Text = (autoTotalPoints[2] + teleOpTotalPoints[2] + challengeScale[2]).ToString();
            if (FinshedScoring[2])
                lblTeleOp3.ForeColor = Color.DarkGreen;
            else
                lblTeleOp3.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(36)))));



        }
        void tm1939UpdateController3()
        {
            if (TeleOp[3])
            {
                lblAuto4.Visible = false;
                lblTeleOp4.Visible = true;
                lblAutoD2Reach4.Visible = false;
                lblTeleOpD2Att4.Visible = true;
                lblAutoD2Cross4.Visible = false;
                lblTeleOpD2Cross4.Visible = true;
                lblAutoMidAtt4.Visible = false;
                lblTeleOpMidAtt4.Visible = true;
                lblAutoMidMade4.Visible = false;
                lblTeleOpMidMade4.Visible = true;
                lblAutoLowAtt4.Visible = false;
                lblTeleOpLowAtt4.Visible = true;
                lblAutoLowMade4.Visible = false;
                lblTeleOpLowMade4.Visible = true;
                lblTeleOpPyramidAtt4.Visible = true;
                lblTeleOpPyramidMade4.Visible = true;
                lblChallengeScale4.Visible = true;
            }
            if (AutonomousMode[3])
            {
                lblAuto4.Visible = true;
                lblTeleOp4.Visible = false;
                lblAutoD2Reach4.Visible = true;
                lblTeleOpD2Att4.Visible = false;
                lblAutoD2Cross4.Visible = true;
                lblTeleOpD2Cross4.Visible = false;
                lblAutoMidAtt4.Visible = true;
                lblTeleOpMidAtt4.Visible = false;
                lblAutoMidMade4.Visible = true;
                lblTeleOpMidMade4.Visible = false;
                lblAutoLowAtt4.Visible = true;
                lblTeleOpLowAtt4.Visible = false;
                lblAutoLowMade4.Visible = true;
                lblTeleOpLowMade4.Visible = false;
                lblTeleOpPyramidAtt4.Visible = false;
                lblTeleOpPyramidMade4.Visible = false;
                lblChallengeScale4.Visible = false;
            }
            //Defense Rating
            lblDefense4.Text = displayDefenseRating[3].ToString();

            // Pyramid Goals
            lblTeleOpPyramidMade4.Text = displayPyramidFrisbeesMade[3].ToString();
            lblTeleOpPyramidAtt4.Text = displayPyramidFrisbeesAtt[3].ToString();

            // High Goals
            lblTeleOpD2Cross4.Text = displayDefense2Cross[3].ToString();
            lblTeleOpD2Att4.Text = displayDefense2Att[3].ToString();
            lblAutoD2Cross4.Text = autoDisplayDefense2Cross[3].ToString();
            lblAutoD2Reach4.Text = autoDisplayDefense2Reach[3].ToString();

            // Mid Goals
            lblTeleOpMidMade4.Text = displayMidFrisbeesMade[3].ToString();
            lblTeleOpMidAtt4.Text = displayMidFrisbeesAtt[3].ToString();
            lblAutoMidMade4.Text = autoDisplayMidFrisbeesMade[3].ToString();
            lblAutoMidAtt4.Text = autoDisplayMidFrisbeesAtt[3].ToString();

            // Low Goals
            lblTeleOpLowMade4.Text = displayLowFrisbeesMade[3].ToString();
            lblTeleOpLowAtt4.Text = displayLowFrisbeesAtt[3].ToString();
            lblAutoLowMade4.Text = autoDisplayLowFrisbeesMade[3].ToString();
            lblAutoLowAtt4.Text = autoDisplayLowFrisbeesAtt[3].ToString();

            // Robot Climb
            lblChallengeScale4.Text = challengeScale[3].ToString();

            lblTeleOpTotalPoints4.Text = teleOpTotalPoints[3].ToString();
            lblAutoTotalPoints4.Text = autoTotalPoints[3].ToString();
            lblTotalPoints4.Text = (autoTotalPoints[3] + teleOpTotalPoints[3] + challengeScale[3]).ToString();
            if (FinshedScoring[3])
                lblTeleOp4.ForeColor = Color.DarkGreen;
            else
                lblTeleOp4.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(36)))));



        }
        void tm1939UpdateController4()
        {
            if (TeleOp[4])
            {
                lblAuto5.Visible = false;
                lblTeleOp5.Visible = true;
                lblAutoD2Reach5.Visible = false;
                lblTeleOpD25.Visible = true;
                lblAutoD2Cross5.Visible = false;
                lblTeleOpD2Cross5.Visible = true;
                lblAutoMidAtt5.Visible = false;
                lblTeleOpMidAtt5.Visible = true;
                lblAutoMidMade5.Visible = false;
                lblTeleOpMidMade5.Visible = true;
                lblAutoLowAtt5.Visible = false;
                lblTeleOpLowAtt5.Visible = true;
                lblAutoLowMade5.Visible = false;
                lblTeleOpLowMade5.Visible = true;
                lblTeleOpPyramidAtt5.Visible = true;
                lblTeleOpPyramidMade5.Visible = true;
                lblChallengeScale5.Visible = true;
            }
            if (AutonomousMode[4])
            {
                lblAuto5.Visible = true;
                lblTeleOp5.Visible = false;
                lblAutoD2Reach5.Visible = true;
                lblTeleOpD25.Visible = false;
                lblAutoD2Cross5.Visible = true;
                lblTeleOpD2Cross5.Visible = false;
                lblAutoMidAtt5.Visible = true;
                lblTeleOpMidAtt5.Visible = false;
                lblAutoMidMade5.Visible = true;
                lblTeleOpMidMade5.Visible = false;
                lblAutoLowAtt5.Visible = true;
                lblTeleOpLowAtt5.Visible = false;
                lblAutoLowMade5.Visible = true;
                lblTeleOpLowMade5.Visible = false;
                lblTeleOpPyramidAtt5.Visible = false;
                lblTeleOpPyramidMade5.Visible = false;
                lblChallengeScale5.Visible = false;
            }
            //Defense Rating
            lblDefense5.Text = displayDefenseRating[4].ToString();

            // Pyramid Goals
            lblTeleOpPyramidMade5.Text = displayPyramidFrisbeesMade[4].ToString();
            lblTeleOpPyramidAtt5.Text = displayPyramidFrisbeesAtt[4].ToString();

            // High Goals
            lblTeleOpD2Cross5.Text = displayDefense2Cross[4].ToString();
            lblTeleOpD25.Text = displayDefense2Att[4].ToString();
            lblAutoD2Cross5.Text = autoDisplayDefense2Cross[4].ToString();
            lblAutoD2Reach5.Text = autoDisplayDefense2Reach[4].ToString();

            // Mid Goals
            lblTeleOpMidMade5.Text = displayMidFrisbeesMade[4].ToString();
            lblTeleOpMidAtt5.Text = displayMidFrisbeesAtt[4].ToString();
            lblAutoMidMade5.Text = autoDisplayMidFrisbeesMade[4].ToString();
            lblAutoMidAtt5.Text = autoDisplayMidFrisbeesAtt[4].ToString();

            // Low Goals
            lblTeleOpLowMade5.Text = displayLowFrisbeesMade[4].ToString();
            lblTeleOpLowAtt5.Text = displayLowFrisbeesAtt[4].ToString();
            lblAutoLowMade5.Text = autoDisplayLowFrisbeesMade[4].ToString();
            lblAutoLowAtt5.Text = autoDisplayLowFrisbeesAtt[4].ToString();

            // Robot Climb
            lblChallengeScale5.Text = challengeScale[4].ToString();

            lblTeleOpTotalPoints5.Text = teleOpTotalPoints[4].ToString();
            lblAutoTotalPoints5.Text = autoTotalPoints[4].ToString();
            lblTotalPoints5.Text = (autoTotalPoints[4] + teleOpTotalPoints[4] + challengeScale[4]).ToString();

            if (FinshedScoring[4])
                lblTeleOp5.ForeColor = Color.DarkGreen;
            else
                lblTeleOp5.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(36)))));


        }
        void tm1939UpdateController5()
        {
            if (TeleOp[5])
            {
                lblAuto6.Visible = false;
                lblTeleOp6.Visible = true;
                lblAutoD2Reach6.Visible = false;
                lblTeleOpD2Att6.Visible = true;
                lblAutoD2Cross6.Visible = false;
                lblTeleOpD2Cross6.Visible = true;
                lblAutoMidAtt6.Visible = false;
                lblTeleOpMidAtt6.Visible = true;
                lblAutoMidMade6.Visible = false;
                lblTeleOpMidMade6.Visible = true;
                lblAutoLowAtt6.Visible = false;
                lblTeleOpLowAtt6.Visible = true;
                lblAutoLowMade6.Visible = false;
                lblTeleOpLowMade6.Visible = true;
                lblTeleOpPyramidAtt6.Visible = true;
                lblTeleOpPyramidMade6.Visible = true;
                lblChallengeScale6.Visible = true;
            }
            if (AutonomousMode[5])
            {
                lblAuto6.Visible = true;
                lblTeleOp6.Visible = false;
                lblAutoD2Reach6.Visible = true;
                lblTeleOpD2Att6.Visible = false;
                lblAutoD2Cross6.Visible = true;
                lblTeleOpD2Cross6.Visible = false;
                lblAutoMidAtt6.Visible = true;
                lblTeleOpMidAtt6.Visible = false;
                lblAutoMidMade6.Visible = true;
                lblTeleOpMidMade6.Visible = false;
                lblAutoLowAtt6.Visible = true;
                lblTeleOpLowAtt6.Visible = false;
                lblAutoLowMade6.Visible = true;
                lblTeleOpLowMade6.Visible = false;
                lblTeleOpPyramidAtt6.Visible = false;
                lblTeleOpPyramidMade6.Visible = false;
                lblChallengeScale6.Visible = false;
            }
            //Defense Rating
            lblDefense6.Text = displayDefenseRating[5].ToString();

            // Pyramid Goals
            lblTeleOpPyramidMade6.Text = displayPyramidFrisbeesMade[5].ToString();
            lblTeleOpPyramidAtt6.Text = displayPyramidFrisbeesAtt[5].ToString();

            // High Goals
            lblTeleOpD2Cross6.Text = displayDefense2Cross[5].ToString();
            lblTeleOpD2Att6.Text = displayDefense2Att[5].ToString();
            lblAutoD2Cross6.Text = autoDisplayDefense2Cross[5].ToString();
            lblAutoD2Reach6.Text = autoDisplayDefense2Reach[5].ToString();

            // Mid Goals
            lblTeleOpMidMade6.Text = displayMidFrisbeesMade[5].ToString();
            lblTeleOpMidAtt6.Text = displayMidFrisbeesAtt[5].ToString();
            lblAutoMidMade6.Text = autoDisplayMidFrisbeesMade[5].ToString();
            lblAutoMidAtt6.Text = autoDisplayMidFrisbeesAtt[5].ToString();

            // Low Goals
            lblTeleOpLowMade6.Text = displayLowFrisbeesMade[5].ToString();
            lblTeleOpLowAtt6.Text = displayLowFrisbeesAtt[5].ToString();
            lblAutoLowMade6.Text = autoDisplayLowFrisbeesMade[5].ToString();
            lblAutoLowAtt6.Text = autoDisplayLowFrisbeesAtt[5].ToString();

            // Robot Climb
            lblChallengeScale6.Text = challengeScale[5].ToString();

            lblTeleOpTotalPoints6.Text = teleOpTotalPoints[5].ToString();
            lblAutoTotalPoints6.Text = autoTotalPoints[5].ToString();
            lblTotalPoints6.Text = (autoTotalPoints[5] + teleOpTotalPoints[5] + challengeScale[5]).ToString();

            if (FinshedScoring[5])
                lblTeleOp6.ForeColor = Color.DarkGreen;
            else
                lblTeleOp6.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(36)))));


        }
        void tm1939ProcessButton(string strButtonMap, int id)
        {
            int FoundAt;

            // Find where the button maps are equal to get the command
            for (FoundAt = 0; FoundAt < 22 && !strButtonMap.Equals(ControllerCommands[id, FoundAt]); FoundAt++)
            {
            }

            // Perform the appropriate function

            switch (FoundAt)
            {
                case (GameCommands.TeleOp):
                    AutonomousMode[id] = false;
                    TeleOp[id] = true;

                    break;

                case (GameCommands.Autonomous):
                    AutonomousMode[id] = true;
                    TeleOp[id] = false;
                    FinshedScoring[id] = false;
                    break;

                case (GameCommands.scoreHigh):
                    if (TeleOp[id] && defenseRating[id] > 0)
                    {
                        defenseRating[id]--;
                        displayDefenseRating[id] = defenseRating[id];
                    }
                    break;

                case (GameCommands.scoreLow):
                    if (TeleOp[id])
                    {
                        defenseRating[id]++;
                        displayDefenseRating[id] = defenseRating[id];
                        if (displayDefenseRating[id] > 10)
                        {
                            defenseRating[id] = 0;
                            displayDefenseRating[id] = 0;
                        }
                    }

                    break;

                case (GameCommands.Defense2AttMinus):
                    if (TeleOp[id])
                    {
                        if (defense2Att[id] > 0 && defense2Cross[id] < defense2Att[id])
                        {
                            defense2Att[id]--;
                            displayDefense2Att[id] = defense2Att[id];
                        }
                    }
                    if (AutonomousMode[id])
                    {
                        if (autoDefense2Reach[id] > 0 && autoDefense2Cross[id] < autoDefense2Reach[id])
                        {
                            autoDefense2Reach[id]--;
                            autoDisplayDefense2Reach[id] = autoDefense2Reach[id];
                        }
                    }

                    break;

                case (GameCommands.Defense2AttPlus):
                    if (TeleOp[id])
                    {
                        defense2Att[id]++;
                        displayDefense2Att[id] = defense2Att[id];
                    }
                    if (AutonomousMode[id])
                    {
                        autoDefense2Reach[id]++;
                        autoDisplayDefense2Reach[id] = autoDefense2Reach[id];
                    }

                    break;

                case (GameCommands.Defense2CrossMinus):
                    if (TeleOp[id])
                    {
                        if (defense2Cross[id] > 0)
                        {
                            defense2Cross[id]--;
                            displayDefense2Cross[id] = defense2Cross[id];
                        }
                    }
                    if (AutonomousMode[id])
                    {
                        if (autoDefense2Cross[id] > 0)
                        {
                            autoDefense2Cross[id]--;
                            autoDisplayDefense2Cross[id] = autoDefense2Cross[id];
                        }
                    }
                    break;

                case (GameCommands.Defense2CrossPlus):
                    if (TeleOp[id])
                    {
                        defense2Cross[id]++;
                        displayDefense2Cross[id] = defense2Cross[id];
                        defense2Att[id]++;
                        displayDefense2Att[id] = defense2Att[id];
                    }
                    if (AutonomousMode[id])
                    {
                        autoDefense2Cross[id]++;
                        autoDisplayDefense2Cross[id] = autoDefense2Cross[id];
                        autoDefense2Reach[id]++;
                        autoDisplayDefense2Reach[id] = autoDefense2Reach[id];
                    }
                    break;

                case (GameCommands.LowFrisbeesAttMinus):
                    if (TeleOp[id])
                    {
                        if (lowFrisbeesAtt[id] > 0 && lowFrisbeesMade[id] < lowFrisbeesAtt[id])
                        {
                            lowFrisbeesAtt[id]--;
                            displayLowFrisbeesAtt[id] = lowFrisbeesAtt[id];
                        }
                    }
                    if (AutonomousMode[id])
                    {
                        if (autoLowFrisbeesAtt[id] > 0 && autoLowFrisbeesMade[id] < autoLowFrisbeesAtt[id])
                        {
                            autoLowFrisbeesAtt[id]--;
                            autoDisplayLowFrisbeesAtt[id] = autoLowFrisbeesAtt[id];
                        }
                    }

                    break;

                case (GameCommands.LowFrisbeesAttPlus):
                    if (TeleOp[id])
                    {
                        lowFrisbeesAtt[id]++;
                        displayLowFrisbeesAtt[id] = lowFrisbeesAtt[id];
                    }
                    if (AutonomousMode[id])
                    {
                        autoLowFrisbeesAtt[id]++;
                        autoDisplayLowFrisbeesAtt[id] = autoLowFrisbeesAtt[id];
                    }


                    break;

                case (GameCommands.LowFrisbeesMadeMinus):
                    if (TeleOp[id])
                    {
                        if (lowFrisbeesMade[id] > 0)
                        {
                            lowFrisbeesMade[id]--;
                            displayLowFrisbeesMade[id] = lowFrisbeesMade[id];
                        }
                    }
                    if (AutonomousMode[id])
                    {
                        if (autoLowFrisbeesMade[id] > 0)
                        {
                            autoLowFrisbeesMade[id]--;
                            autoDisplayLowFrisbeesMade[id] = autoLowFrisbeesMade[id];
                        }
                    }

                    break;

                case (GameCommands.LowFrisbeesMadePlus):
                    if (TeleOp[id])
                    {
                        lowFrisbeesMade[id]++;
                        displayLowFrisbeesMade[id] = lowFrisbeesMade[id];
                        lowFrisbeesAtt[id]++;
                        displayLowFrisbeesAtt[id] = lowFrisbeesAtt[id];
                    }
                    if (AutonomousMode[id])
                    {
                        autoLowFrisbeesMade[id]++;
                        autoDisplayLowFrisbeesMade[id] = autoLowFrisbeesMade[id];
                        autoLowFrisbeesAtt[id]++;
                        autoDisplayLowFrisbeesAtt[id] = autoLowFrisbeesAtt[id];
                    }

                    break;

                case (GameCommands.MidFrisbeesAttMinus):
                    if (TeleOp[id])
                    {
                        if (midFrisbeesAtt[id] > 0 && midFrisbeesMade[id] < midFrisbeesAtt[id])
                        {
                            midFrisbeesAtt[id]--;
                            displayMidFrisbeesAtt[id] = midFrisbeesAtt[id];
                        }
                    }
                    if (AutonomousMode[id])
                    {
                        if (autoMidFrisbeesAtt[id] > 0 && autoMidFrisbeesMade[id] < autoMidFrisbeesAtt[id])
                        {
                            autoMidFrisbeesAtt[id]--;
                            autoDisplayMidFrisbeesAtt[id] = autoMidFrisbeesAtt[id];
                        }
                    }
                    break;

                case (GameCommands.MidFrisbeesAttPlus):
                    if (TeleOp[id])
                    {
                        midFrisbeesAtt[id]++;
                        displayMidFrisbeesAtt[id] = midFrisbeesAtt[id];
                    }
                    if (AutonomousMode[id])
                    {
                        autoMidFrisbeesAtt[id]++;
                        autoDisplayMidFrisbeesAtt[id] = autoMidFrisbeesAtt[id];
                    }


                    break;

                case (GameCommands.MidFrisbeesMadeMinus):
                    if (TeleOp[id])
                    {
                        if (midFrisbeesMade[id] > 0)
                        {
                            midFrisbeesMade[id]--;
                            displayMidFrisbeesMade[id] = midFrisbeesMade[id];
                        }
                    }
                    if (AutonomousMode[id])
                    {
                        if (autoMidFrisbeesMade[id] > 0)
                        {
                            autoMidFrisbeesMade[id]--;
                            autoDisplayMidFrisbeesMade[id] = autoMidFrisbeesMade[id];
                        }
                    }

                    break;

                case (GameCommands.MidFrisbeesMadePlus):
                    if (TeleOp[id])
                    {
                        midFrisbeesMade[id]++;
                        displayMidFrisbeesMade[id] = midFrisbeesMade[id];
                        midFrisbeesAtt[id]++;
                        displayMidFrisbeesAtt[id] = midFrisbeesAtt[id];
                    }
                    if (AutonomousMode[id])
                    {
                        autoMidFrisbeesMade[id]++;
                        autoDisplayMidFrisbeesMade[id] = autoMidFrisbeesMade[id];
                        autoMidFrisbeesAtt[id]++;
                        autoDisplayMidFrisbeesAtt[id] = autoMidFrisbeesAtt[id];
                    }

                    break;

                case (GameCommands.PyramidFrisbeesAttMinus):
                    if (pyramidFrisbeesAtt[id] > 0 && pyramidFrisbeesMade[id] < pyramidFrisbeesAtt[id])
                        pyramidFrisbeesAtt[id]--;
                    displayPyramidFrisbeesAtt[id] = pyramidFrisbeesAtt[id];

                    break;

                case (GameCommands.PyramidFrisbeesAttPlus):
                    pyramidFrisbeesAtt[id]++;
                    displayPyramidFrisbeesAtt[id] = pyramidFrisbeesAtt[id];
                    break;

                case (GameCommands.PyramidFrisbeesMadeMinus):
                    if (pyramidFrisbeesMade[id] > 0)
                        pyramidFrisbeesMade[id]--;
                    displayPyramidFrisbeesMade[id] = pyramidFrisbeesMade[id];
                    break;

                case (GameCommands.PyramidFrisbeesMadePlus):
                    pyramidFrisbeesMade[id]++;
                    displayPyramidFrisbeesMade[id] = pyramidFrisbeesMade[id];
                    // If they made it increase the attempts
                    pyramidFrisbeesAtt[id]++;
                    displayPyramidFrisbeesAtt[id] = pyramidFrisbeesAtt[id];

                    break;

                case (GameCommands.ChallengeScalePlus):
                    if (TeleOp[id])
                    {
                        climb[id]++;
                        challengeScale[id] = climb[id];

                        if (challengeScale[id] > 2) // 0 = not done, 1 = challenge, 2 = scale
                        {
                            challengeScale[id] = 0;
                            climb[id] = 0;
                        }

                        //This line multiplied by 10 for points, and is not relevant
                        //robotClimb[id] = robotClimb[id] * 10;
                    }
                    break;
                case (GameCommands.FinishedScoring):
                    FinshedScoring[id] = !FinshedScoring[id];

                    break;

                default:

                    break;
            }

            displayButtons[id].Text = GetButtonDisplay(strButtonMap);
        }

        // List buttons down for display
        private string GetButtonDisplay(string strButtonMap)
        {
            var result = "";
            for (int i = 0; i < strButtonMap.Length; i++)
            {
                if (strButtonMap[i] == 'T')
                {
                    result += i.ToString("00 ", CultureInfo.CurrentCulture);
                }
            }
            return result;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    MessageBox.Show(saveFileDialog1.FileName);
                    fileName = saveFileDialog1.FileName;
                    tm1939SaveFile(sw);
                    sw.Close();
                }
                //Increases match Number
                match++;
                lblmatch.Text = match.ToString();

                //Updates the team # automatically when a new match starts.
                lblAutoTeamNo1.Text = AutoTeamNo1[match - 1].ToString();
                lblAutoTeamNo2.Text = AutoTeamNo2[match - 1].ToString();
                lblAutoTeamNo3.Text = AutoTeamNo3[match - 1].ToString();
                lblAutoTeamNo4.Text = AutoTeamNo4[match - 1].ToString();
                lblAutoTeamNo5.Text = AutoTeamNo5[match - 1].ToString();
                lblAutoTeamNo6.Text = AutoTeamNo6[match - 1].ToString();

                for (int f = 0; f < 6; f++)
                {
                    displayLowFrisbeesMade[f] = 0;
                    displayLowFrisbeesAtt[f] = 0;
                    lowFrisbeesMade[f] = 0;
                    lowFrisbeesAtt[f] = 0;
                    autoDisplayLowFrisbeesMade[f] = 0;
                    autoLowFrisbeesMade[f] = 0;
                    autoDisplayLowFrisbeesAtt[f] = 0;
                    autoLowFrisbeesAtt[f] = 0;
                    displayMidFrisbeesMade[f] = 0;
                    midFrisbeesMade[f] = 0;
                    displayMidFrisbeesAtt[f] = 0;
                    midFrisbeesAtt[f] = 0;
                    autoDisplayMidFrisbeesMade[f] = 0;
                    autoMidFrisbeesMade[f] = 0;
                    autoDisplayMidFrisbeesAtt[f] = 0;
                    autoMidFrisbeesAtt[f] = 0;
                    displayDefense2Cross[f] = 0;
                    defense2Cross[f] = 0;
                    displayDefense2Att[f] = 0;
                    defense2Att[f] = 0;
                    autoDisplayDefense2Cross[f] = 0;
                    autoDefense2Cross[f] = 0;
                    autoDisplayDefense2Reach[f] = 0;
                    autoDefense2Reach[f] = 0;
                    displayPyramidFrisbeesMade[f] = 0;
                    pyramidFrisbeesMade[f] = 0;
                    displayPyramidFrisbeesAtt[f] = 0;
                    pyramidFrisbeesAtt[f] = 0;
                    climb[f] = 0;
                    challengeScale[f] = 0;
                    teleOpTotalPoints[f] = 0;
                    autoTotalPoints[f] = 0;
                    defenseRating[f] = 0;
                    displayDefenseRating[f] = 0;
                    lblAuto.Visible = true;
                    lblAuto2.Visible = true;
                    lblAuto3.Visible = true;
                    lblAuto4.Visible = true;
                    lblAuto5.Visible = true;
                    lblAuto6.Visible = true;
                    lblTeleOp.Visible = false;
                    lblTeleOp2.Visible = false;
                    lblTeleOp3.Visible = false;
                    lblTeleOp4.Visible = false;
                    lblTeleOp5.Visible = false;
                    lblTeleOp6.Visible = false;
                    lblChallengeScale.Visible = false;
                    lblChallengeScale2.Visible = false;
                    lblChallengeScale3.Visible = false;
                    lblChallengeScale4.Visible = false;
                    lblChallengeScale5.Visible = false;
                    lblChallengeScale6.Visible = false;
                    FinshedScoring[f] = false;
                    tm1939RefreshScreen(f);
                }

            }

        }

        //The following code updates the time and the date.
        private void tmrtime_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String myFileName = fileName;
            StreamWriter sww = File.AppendText(myFileName);
            tm1939SaveFile(sww);

            sww.Close();
            //Increases match Number
            match++;
            lblmatch.Text = match.ToString();

            //Updates the team # automatically when a new match starts.
            lblAutoTeamNo1.Text = AutoTeamNo1[match - 1].ToString();
            lblAutoTeamNo2.Text = AutoTeamNo2[match - 1].ToString();
            lblAutoTeamNo3.Text = AutoTeamNo3[match - 1].ToString();
            lblAutoTeamNo4.Text = AutoTeamNo4[match - 1].ToString();
            lblAutoTeamNo5.Text = AutoTeamNo5[match - 1].ToString();
            lblAutoTeamNo6.Text = AutoTeamNo6[match - 1].ToString();

            for (int f = 0; f < 6; f++)
            {
                displayLowFrisbeesMade[f] = 0;
                displayLowFrisbeesAtt[f] = 0;
                lowFrisbeesMade[f] = 0;
                lowFrisbeesAtt[f] = 0;
                autoDisplayLowFrisbeesMade[f] = 0;
                autoLowFrisbeesMade[f] = 0;
                autoDisplayLowFrisbeesAtt[f] = 0;
                autoLowFrisbeesAtt[f] = 0;
                displayMidFrisbeesMade[f] = 0;
                midFrisbeesMade[f] = 0;
                displayMidFrisbeesAtt[f] = 0;
                midFrisbeesAtt[f] = 0;
                autoDisplayMidFrisbeesMade[f] = 0;
                autoMidFrisbeesMade[f] = 0;
                autoDisplayMidFrisbeesAtt[f] = 0;
                autoMidFrisbeesAtt[f] = 0;
                displayDefense2Cross[f] = 0;
                defense2Cross[f] = 0;
                displayDefense2Att[f] = 0;
                defense2Att[f] = 0;
                autoDisplayDefense2Cross[f] = 0;
                autoDefense2Cross[f] = 0;
                autoDisplayDefense2Reach[f] = 0;
                autoDefense2Reach[f] = 0;
                displayPyramidFrisbeesMade[f] = 0;
                pyramidFrisbeesMade[f] = 0;
                displayPyramidFrisbeesAtt[f] = 0;
                pyramidFrisbeesAtt[f] = 0;
                climb[f] = 0;
                challengeScale[f] = 0;
                teleOpTotalPoints[f] = 0;
                autoTotalPoints[f] = 0;
                defenseRating[f] = 0;
                displayDefenseRating[f] = 0;
                FinshedScoring[f] = false;
                tm1939RefreshScreen(f);
            }
            lblAuto.Visible = true;
            lblAuto2.Visible = true;
            lblAuto3.Visible = true;
            lblAuto4.Visible = true;
            lblAuto5.Visible = true;
            lblAuto6.Visible = true;
            lblTeleOp.Visible = false;
            lblTeleOp2.Visible = false;
            lblTeleOp3.Visible = false;
            lblTeleOp4.Visible = false;
            lblTeleOp5.Visible = false;
            lblTeleOp6.Visible = false;
            lblChallengeScale.Visible = false;
            lblChallengeScale2.Visible = false;
            lblChallengeScale3.Visible = false;
            lblChallengeScale4.Visible = false;
            lblChallengeScale5.Visible = false;
            lblChallengeScale6.Visible = false;
        }

        private void tm1939SaveFile(StreamWriter outputstream)
        {
            // A single writeline section to handle both save buttons.
            // Added Match to the end of each record
            outputstream.WriteLine(lblAutoTeamNo1.Text + x + lblAutoD2Cross.Text + x + lblAutoD2Reach.Text + x + lblAutoMidMade.Text + x + lblAutoMidAtt.Text + x + lblAutoLowMade.Text + x + lblAutoLowAtt.Text + x + lblAutoTotalPoints.Text + x + lblTeleOpPyramidMade.Text + x + lblTeleOpPyramidAtt.Text + x + lblTeleOpD2Cross.Text + x + lblTeleOpD2Att.Text + x + lblTeleOpMidMade.Text + x + lblTeleOpMidAtt.Text + x + lblTeleOpLowMade.Text + x + lblTeleOpLowAtt.Text + x + lblChallengeScale.Text + x + lblTeleOpTotalPoints.Text + x + lblTotalPoints.Text + x + lblDefense.Text + x + lblmatch.Text);
            outputstream.WriteLine(lblAutoTeamNo2.Text + x + lblAutoD2Cross2.Text + x + lblAutoD2Reach2.Text + x + lblAutoMidMade2.Text + x + lblAutoMidAtt2.Text + x + lblAutoLowMade2.Text + x + lblAutoLowAtt2.Text + x + lblAutoTotalPoints2.Text + x + lblTeleOpPyramidMade2.Text + x + lblTeleOpPyramidAtt2.Text + x + lblTeleOpD2Cross2.Text + x + lblTeleOpD2Att2.Text + x + lblTeleOpMidMade2.Text + x + lblTeleOpMidAtt2.Text + x + lblTeleOpLowMade2.Text + x + lblTeleOpLowAtt2.Text + x + lblChallengeScale2.Text + x + lblTeleOpTotalPoints2.Text + x + lblTotalPoints2.Text + x + lblDefense2.Text + x + lblmatch.Text);
            outputstream.WriteLine(lblAutoTeamNo3.Text + x + lblAutoD2Cross3.Text + x + lblAutoD2Reach3.Text + x + lblAutoMidMade3.Text + x + lblAutoMidAtt3.Text + x + lblAutoLowMade3.Text + x + lblAutoLowAtt3.Text + x + lblAutoTotalPoints3.Text + x + lblTeleOpPyramidMade3.Text + x + lblTeleOpPyramidAtt3.Text + x + lblTeleOpD2Cross3.Text + x + lblTeleOpD2Att3.Text + x + lblTeleOpMidMade3.Text + x + lblTeleOpMidAtt3.Text + x + lblTeleOpLowMade3.Text + x + lblTeleOpLowAtt3.Text + x + lblChallengeScale3.Text + x + lblTeleOpTotalPoints3.Text + x + lblTotalPoints3.Text + x + lblDefense3.Text + x + lblmatch.Text);
            outputstream.WriteLine(lblAutoTeamNo4.Text + x + lblAutoD2Cross4.Text + x + lblAutoD2Reach4.Text + x + lblAutoMidMade4.Text + x + lblAutoMidAtt4.Text + x + lblAutoLowMade4.Text + x + lblAutoLowAtt4.Text + x + lblAutoTotalPoints4.Text + x + lblTeleOpPyramidMade4.Text + x + lblTeleOpPyramidAtt4.Text + x + lblTeleOpD2Cross4.Text + x + lblTeleOpD2Att4.Text + x + lblTeleOpMidMade4.Text + x + lblTeleOpMidAtt4.Text + x + lblTeleOpLowMade4.Text + x + lblTeleOpLowAtt4.Text + x + lblChallengeScale4.Text + x + lblTeleOpTotalPoints4.Text + x + lblTotalPoints4.Text + x + lblDefense4.Text + x + lblmatch.Text);
            outputstream.WriteLine(lblAutoTeamNo5.Text + x + lblAutoD2Cross5.Text + x + lblAutoD2Reach5.Text + x + lblAutoMidMade5.Text + x + lblAutoMidAtt5.Text + x + lblAutoLowMade5.Text + x + lblAutoLowAtt5.Text + x + lblAutoTotalPoints5.Text + x + lblTeleOpPyramidMade5.Text + x + lblTeleOpPyramidAtt5.Text + x + lblTeleOpD2Cross5.Text + x + lblTeleOpD25.Text + x + lblTeleOpMidMade5.Text + x + lblTeleOpMidAtt5.Text + x + lblTeleOpLowMade5.Text + x + lblTeleOpLowAtt5.Text + x + lblChallengeScale5.Text + x + lblTeleOpTotalPoints5.Text + x + lblTotalPoints5.Text + x + lblDefense5.Text + x + lblmatch.Text);
            outputstream.WriteLine(lblAutoTeamNo6.Text + x + lblAutoD2Cross6.Text + x + lblAutoD2Reach6.Text + x + lblAutoMidMade6.Text + x + lblAutoMidAtt6.Text + x + lblAutoLowMade6.Text + x + lblAutoLowAtt6.Text + x + lblAutoTotalPoints6.Text + x + lblTeleOpPyramidMade6.Text + x + lblTeleOpPyramidAtt6.Text + x + lblTeleOpD2Cross6.Text + x + lblTeleOpD2Att6.Text + x + lblTeleOpMidMade6.Text + x + lblTeleOpMidAtt6.Text + x + lblTeleOpLowMade6.Text + x + lblTeleOpLowAtt6.Text + x + lblChallengeScale6.Text + x + lblTeleOpTotalPoints6.Text + x + lblTotalPoints6.Text + x + lblDefense6.Text + x + lblmatch.Text);



        }

        private void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)

            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                String test = sr.ReadToEnd();
                String[] newTeams = test.Split(',');
                int countAgain = newTeams.Length;
                teamsNotePad = new string[countAgain];
                newTeams.CopyTo(teamsNotePad, 0);
                autoTeams = teamsNotePad.Length;
                int teamsDivide = teamsNotePad.Length / 6;

                DialogResult dialogResult = MessageBox.Show(teamsDivide.ToString() + "\n Is this the correct number of matches?", "Adding Teams", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    sr.Close();

                    AutoTeamNo1 = new int[autoTeams];
                    AutoTeamNo2 = new int[autoTeams];
                    AutoTeamNo3 = new int[autoTeams];
                    AutoTeamNo4 = new int[autoTeams];
                    AutoTeamNo5 = new int[autoTeams];
                    AutoTeamNo6 = new int[autoTeams];
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Check your text file with the schedule to make sure it is right!");
                    System.Environment.Exit(0);
                }
            }

            int count = 0;
            for (int j = 0; j < teamsNotePad.Length / 6; j++)
            {
                AutoTeamNo1[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
                AutoTeamNo2[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
                AutoTeamNo3[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
                AutoTeamNo4[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
                AutoTeamNo5[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
                AutoTeamNo6[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
            }

            lblAutoTeamNo1.Text = AutoTeamNo1[0].ToString();
            lblAutoTeamNo2.Text = AutoTeamNo2[0].ToString();
            lblAutoTeamNo3.Text = AutoTeamNo3[0].ToString();
            lblAutoTeamNo4.Text = AutoTeamNo4[0].ToString();
            lblAutoTeamNo5.Text = AutoTeamNo5[0].ToString();
            lblAutoTeamNo6.Text = AutoTeamNo6[0].ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lblEvent.Text = textBox1.Text;
            button4.Visible = false;
            btnSkip.Visible = true;
            textBox1.Clear();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            int skip = Convert.ToInt32(textBox1.Text);
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                fileName = open.FileName;
            }
            match = skip;
            lblmatch.Text = match.ToString();
            lblAutoTeamNo1.Text = AutoTeamNo1[match - 1].ToString();
            lblAutoTeamNo2.Text = AutoTeamNo2[match - 1].ToString();
            lblAutoTeamNo3.Text = AutoTeamNo3[match - 1].ToString();
            lblAutoTeamNo4.Text = AutoTeamNo4[match - 1].ToString();
            lblAutoTeamNo5.Text = AutoTeamNo5[match - 1].ToString();
            lblAutoTeamNo6.Text = AutoTeamNo6[match - 1].ToString();
        }

        private void btnScouter1_Click(object sender, EventArgs e)
        {
            lblScouter1.Text = textBoxScout1.Text;
            textBoxScout1.Visible = false;
            btnScouter1.Visible = false;
            lblScouter1.Visible = true;
        }

        private void btnScouter2_Click(object sender, EventArgs e)
        {
            lblScouter2.Text = textBoxScout2.Text;
            textBoxScout2.Visible = false;
            btnScouter2.Visible = false;
            lblScouter2.Visible = true;
        }

        private void btnScouter3_Click(object sender, EventArgs e)
        {
            lblScouter3.Text = textBoxScout3.Text;
            textBoxScout3.Visible = false;
            btnScouter3.Visible = false;
            lblScouter3.Visible = true;
        }

        private void btnScouter4_Click(object sender, EventArgs e)
        {
            lblScouter4.Text = textBoxScout4.Text;
            textBoxScout4.Visible = false;
            btnScouter4.Visible = false;
            lblScouter4.Visible = true;
        }

        private void btnScouter5_Click(object sender, EventArgs e)
        {
            lblScouter5.Text = textBoxScout5.Text;
            textBoxScout5.Visible = false;
            btnScouter5.Visible = false;
            lblScouter5.Visible = true;
        }

        private void btnScouter6_Click(object sender, EventArgs e)
        {
            lblScouter6.Text = textBoxScout6.Text;
            textBoxScout6.Visible = false;
            btnScouter6.Visible = false;
            lblScouter6.Visible = true;
        }

        private void lblAutoTotalPoints_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void lblTime_Click(object sender, EventArgs e)
        {

        }

        private void lblAutoTeamNo2_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void lblAutoTeamNo4_Click(object sender, EventArgs e)
        {

        }

        private void lblAutoTeamNo5_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}