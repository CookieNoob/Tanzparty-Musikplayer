using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;




namespace VFL_Party_Player
{

    public partial class MainWindow : Form
    {
        public List<dances> ListOfDances = new List<dances>();
        public List<List<string>> ListOfSongs = new List<List<string>>();
        public int iteratorDance = 0, iteratorRound = 1;
        public int currentDance = 0, fadeLength = 140000;
        public int currentVolume = 1000;
        public string CurrentPath = Environment.CurrentDirectory;


        MusicPlayer player = new MusicPlayer();

        public MainWindow()
        {
            InitializeComponent();
            ReadDances();
            for (int i = 0; i < ListOfDances.Count(); i++)
                ReadSongs(i);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigWindow Confwnd = new ConfigWindow();
            Confwnd.parentwnd = this;
            Confwnd.setTextboxText(fadeLength/1000);
            Confwnd.Show();
            this.Visible = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Start")
            {
                for(int i=0; i<ListOfDances.Count(); i++)
                {
                    if (ListOfDances[i].order == 0)
                    {
                        currentDance = i;
                    }
                }
            }
            button2.Text = "Nächster Song";
            string SongName = NextSongForDance(currentDance);
            int counter = 0;
            while (SongName == "-1" || counter > ListOfDances.Count())
            {
                currentDance= NextDance(ref iteratorDance, ref iteratorRound);
                SongName = NextSongForDance(currentDance);
                counter++;
                if (counter == ListOfDances.Count())
                {
                    string message = "Keine Songs gefunden!";
                    string caption = "Error!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, caption, buttons);
                    return;
                }
            }
            PlayNextSong(SongName);
            timer1.Enabled = true;
            
        }


        public void ReadDances()
        {            
            int NumberOfDances = 0;
            foreach (var dir in Directory.EnumerateDirectories(CurrentPath))
            {
                dances newDance = new dances();
                newDance.name = dir.Remove(0, CurrentPath.Length+1);
                newDance.count = 0;
                newDance.ansage = "";
                newDance.ansagevorhanden = false;
                newDance.frequency = 1;
                newDance.order = NumberOfDances;
                NumberOfDances++;
                newDance.lastPlayed = "";
                ListOfDances.Add(newDance);
                ListOfSongs.Add(new List<string>());
                comboBox1.Items.Add(newDance.name);
            }
        }

        public void ReadSongs(int n)
        {
            string DancePath = CurrentPath + "\\" + ListOfDances[n].name;

            var SongFiles = Directory.EnumerateFiles(DancePath, "*.mp3");

            ListOfDances[n].count = 0;

            foreach (string currentFile in SongFiles)
            {
                if (!currentFile.Contains("Ansage.mp3"))
                {
                    ListOfSongs[n].Add(currentFile);
                    ListOfDances[n].count++;
                }
                else
                {
                    ListOfDances[n].ansage = currentFile;
                    ListOfDances[n].ansagevorhanden = true;
                }
            }
        }


        public string NextSongForDance(int DanceType)
        {
            if (ListOfDances[DanceType].count == 0)
                return "-1";
            if (ListOfSongs[DanceType].Count() == 0)
                ReadSongs(DanceType);
            int maxSongNumber = ListOfSongs[DanceType].Count();
            Random random = new Random();
            int randomSong = random.Next(0, maxSongNumber-1);

            //check if the song was played the round before (only relevant if the list of songs is refreshed)
            if ((ListOfSongs[DanceType])[randomSong] == ListOfDances[DanceType].lastPlayed && ListOfDances[DanceType].count > 1)
            {
                ListOfSongs[DanceType].RemoveAt(randomSong);
                randomSong = random.Next(0, maxSongNumber - 2);
            }

            ListOfDances[DanceType].lastPlayed = (ListOfSongs[DanceType])[randomSong];
            ListOfSongs[DanceType].RemoveAt(randomSong);
            return ListOfDances[DanceType].lastPlayed;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            player.stop();
            button2.Text = "Start";
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            string SongName = NextSongForDance(currentDance);
            PlayNextSong(SongName);
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                timer2.Enabled = false;
                return;
            }
            if (timer2.Interval > 50)
                timer2.Interval = 50;
            currentVolume = currentVolume - 20;
            player.setVolume(currentVolume);
            if (currentVolume <= 0)
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                string SongName = NextSongForDance(currentDance);
                PlayNextSong(SongName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Wollen Sie das Programm beenden?", "Achtung!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void JumpToDance(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                for (int i = 0; i < ListOfDances.Count(); i++)
                {
                    if (comboBox1.Text == ListOfDances[i].name)
                    {
                        string SongName = NextSongForDance(i);
                        if (SongName != "-1")
                        {
                            currentDance = i;
                            PlayNextSong(SongName);
                        }
                    }
                }
            }
        }

        void PlayNextSong(string SongName)
        {
            player.stop();
            timer1.Enabled = false;
            timer2.Enabled = false;

            if (ListOfDances[currentDance].ansagevorhanden && checkBox1.Checked)
            {
                player.open(ListOfDances[currentDance].ansage);
                player.play();
                System.Threading.Thread.Sleep(player.CalculateLength());
                player.stop();
            }
            player.open(SongName);
            SongName = SongName.Remove(0, CurrentPath.Length + 1);
            SongName = SongName.Remove(SongName.Length - 4);

            int songlength = player.CalculateLength();

            if (songlength > 100)
            {
                listBox1.Items.Add(SongName);
                player.resetVolume();
                currentVolume = 1000;
                timer1.Interval = songlength;
                timer2.Interval = fadeLength - 2500;
                
                timer1.Enabled = true;
                timer2.Enabled = true;
                player.play();
            }
            else
            {
                Task.Run(() =>
                {
                    string message = "Das Lied " + SongName + " konnte nicht abgespielt werden :(\nWahrscheinlich liegt das an einer inkompatiblen mp3-Version. Es kann helfen das Lied nochmal als mp3 zu konvertieren.";
                    string caption = "Fehler!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, caption, buttons);
                });
            }
            currentDance = NextDance(ref iteratorDance, ref iteratorRound);
            button5.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string message = "Vorbereitung: Im Verzeichnis, in dem dieses Programm ist, jeweils einen Ordner pro Tanz erstellen und mit mp3" +
                             " Songs für die jeweiligen Tänze füllen. Das Programm erkennt automatisch die verfügbaren Tänze und liest die " +
                             "Lieder ein. Anschließend \"Konfiguration\" anklicken und die Abspieloptionen für die Häufigkeit und Reihenfolge " +
                             "der Tänze bestimmen. Änderungen bestätigen und mit dem Startknopf das Abspielen starten. \n\nHinweis: Wenn in " +
                             "einem Verzeichnis mit den Tänzen eine mp3 Datei mit dem Namen \"Ansage\" (zum Ankündigen der jeweiligen Tänze)" +
                             " verfügbar ist, dann kann diese vor dem jeweiligen Tanz gespielt werden (Haken setzen in der Checkbox). \n\nBei " +
                             "Fragen an Tobias wenden.";
            string caption = "Anleitung";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, caption, buttons);
        }

        private void disableHelpButton(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }



        private void enableHelpButton(object sender, EventArgs e)
        {
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
        }

        // Looks up the next dance. If all dances are used up for this round it increments the round and starts from the first dance again
        int NextDance(ref int iterator, ref int round)
        {
            iterator++;
            while (true)
            {
                for (int i = 0; i < ListOfDances.Count(); i++)
                {
                    if (ListOfDances[i].order == iterator)
                    {
                        if (ListOfDances[i].count > 0)
                        {
                            if (round % ListOfDances[i].frequency == 0)
                                return i;
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                if (iterator < ListOfDances.Count() - 1)
                    iterator++;
                else
                {
                    iterator = 0;
                    round++;
                }
            }
        }
    }
}



