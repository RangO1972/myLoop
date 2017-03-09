using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace myLoop {
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell {


        private string _CommentChars = "//";
        public string CommentChars
        {
            get { return _CommentChars; }
            set
            {
                _CommentChars = value;
                NotifyOfPropertyChange(() => CommentChars);
            }
        }

        private string _Repetition = "10";
        public string Repetition
        {
            get { return _Repetition; }
            set
            {
                _Repetition = value;
                NotifyOfPropertyChange(() => Repetition);
            }
        }



        private string _Src = "# // test{0}\n#L 12345 // just first repeat\nT DATO{0}\nT RET{0} // Item{1}\n#T BACKUP // Backup value just first repeat\n!NEW\n# // second loop\n# L VALUE\nT Item{0:00}.Value";
        public string Src
        {
            get { return _Src; }
            set
            {
                _Src = value;
                NotifyOfPropertyChange(() => Src);
            }
        }

        private string _Dst = "";
        public string Dst
        {
            get { return _Dst; }
            set
            {
                _Dst = value;
                NotifyOfPropertyChange(() => Dst);
            }
        }


        public void New() { Src = ""; }
        public void Open() {
            // Legge un file di testo in src
            OpenFileDialog dialog = new OpenFileDialog() {
                Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
            };
            if (dialog.ShowDialog() == true)
            {
                Src = File.ReadAllText(dialog.FileName);
            }
        }
        public void Save() {
            // Salva il contenuto del risultato in un file

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, Dst);
            }


        }

        public void SrcCopy() { Clipboard.SetText(Src); }
        public void SrcPaste() { Src += "\n" + Clipboard.GetText();}
        public void DstCopy() { Clipboard.SetText(Dst); }
        public void Logo() { System.Diagnostics.Process.Start("http://bit.ly/2n3diwv"); }

        public void Go()
        {
            //
            int n = int.Parse(Repetition);
            List<List<string>> xxx = new List<List<string>>();
            List<string> mySRC = new List<string>();
            TextReader myReader = new StringReader(Src);
            string line;
            Dst = "";  // RESET DST
            Dst += "//-----------------------\n";
            Dst += "// Templete for RdC LOOP\n";
            Dst += "//-----------------------\n";

            while ((line = myReader.ReadLine()) != null)
            {
                Dst += "//" + line + "\n";
                if (line == "!NEW")
                {
                    xxx.Add(mySRC);
                    mySRC = new List<string>(); ;
                }
                else
                {
                    mySRC.Add(line);
                }

            }
            Dst += "//-----------------------\n";

            xxx.Add(mySRC);

            myReader = null;


            foreach (var items in xxx)
            {
                for (int i = 0; i < n; i++)
                {
                    foreach (var item in items)
                    {
                        string myStr;
                        // Verifica se il primo carattere e' un #
                        // lo scrivo solo alla prima interazione
                        if ((item[0] != '#') | (i == 0))
                        {

                            if (item[0] == '#')
                            {
                                myStr = item.Remove(0, 1);
                            }
                            else
                            {
                                myStr = item;
                            }

                            // to do
                            // il carattere si dovrebbe togliere completamente
                            Dst += string.Format(myStr, i, i + 1) + "\n";
                        }

                    }
                }

            }
        }



    }





}