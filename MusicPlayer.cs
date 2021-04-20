using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;
using System;

namespace VFL_Party_Player
{
    class MusicPlayer
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwndCallback);
        public void open(string file)
        {
            string command = "open \"" + file + "\" type MPEGVideo alias MyMp3";
            mciSendString(command, null, 0, 0);
        }

        public void play()
        {
            string command = "play MyMp3";
            mciSendString(command, null, 0, 0);
        }

        public void stop()
        {
            string command = "stop MyMp3";
            mciSendString(command, null, 0, 0);

            command = "close MyMp3";
            mciSendString(command, null, 0, 0);
        }

        public int CalculateLength()
        {
            StringBuilder str = new StringBuilder(128);
            string command = "status MyMp3 length";
            mciSendString(command, str, 128, 0);
            if (str.Length == 0)
            {
                str.Append("100");
            }
            return int.Parse(str.ToString());
        }

        public void setVolume(int newVolume)
        {
            if (newVolume >= 0)
                mciSendString(string.Concat("setaudio MyMp3 volume to ", newVolume), null, 0, 0);
        }

        public void resetVolume()
        {
            mciSendString(string.Concat("setaudio MyMp3 volume to ", 1000), null, 0, 0);
        }

    }
}
