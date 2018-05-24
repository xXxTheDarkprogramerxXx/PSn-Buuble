using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PSn_Buuble
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            fill();
        }
        
        
        private void fill()
        {
            DirectoryInfo dinfo = new DirectoryInfo(Application.StartupPath);

            FileInfo[] Files = dinfo.GetFiles("*.pkg");
            
            foreach (FileInfo file in Files)
            {
                checkedListBox1.Items.Add(file.Name );
            }
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            
 
            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {

                CreatePDBFiles(itemChecked.ToString());
            }

        }

        private void CreatePDBFiles(string pkg_name) 
        {
            int file_number = 80000000;
            string file_path = "game_pkg/"+ file_number;
            while (Directory.Exists(file_path))
            {
            file_number = file_number +1;
            file_path = "game_pkg/" + file_number;
            }
           

            long pkg_size = 0;
            string path = pkg_name;
           FileInfo pkg = new FileInfo(path);

           if (pkg.Exists)
           {
               pkg_size = pkg.Length;
           }
		
	
		// ------------------------------------------------------------------------
		// write - d0.pdb
		//
           string outFile = "game_pkg/" + file_number + "/d0.pdb";
           Directory.CreateDirectory("game_pkg/" + file_number);
           FileStream o = File.Open(outFile, FileMode.Create);
		// 00000000 - Header
		byte[] header = new byte[4]; 
            header[0] = 0x00; 
            header[1] = 0x00; 
            header[2] = 0x00; 
            header[3] = 0x00;
            o.Write(header, 0, 4);
	
		// 00000065 - Unknown
		byte[] unknown1 = new byte[16];
            unknown1[0] = 0x00;
            unknown1[1] = 0x00;
            unknown1[2] = 0x00;
            unknown1[3] = 0x65;
            unknown1[4] = 0x00;
            unknown1[5] = 0x00;
            unknown1[6] = 0x00;
            unknown1[7] = 0x04;
            unknown1[8] = 0x00;
            unknown1[9] = 0x00;
            unknown1[10] = 0x00;
            unknown1[11] = 0x04;
            unknown1[12] = 0x00;
            unknown1[13] = 0x00;
            unknown1[14] = 0x00;
            unknown1[15] = 0x00;
            o.Write(unknown1, 0, 16);
	
		// 0000006B - Unknown
		byte[] unknown2 = new byte[16];
              unknown2[0] = 0x00;
              unknown2[1] = 0x00;
              unknown2[2] = 0x00;
              unknown2[3] = 0x6B;
              unknown2[4] = 0x00;
              unknown2[5] = 0x00;
              unknown2[6] = 0x00;
              unknown2[7] = 0x04;
              unknown2[8] = 0x00;
              unknown2[9] = 0x00;
              unknown2[10] = 0x00;
              unknown2[11] = 0x04;
              unknown2[12] = 0x00;
              unknown2[13] = 0x00;
              unknown2[14] = 0x00;
              unknown2[15] = 0x03;    
              o.Write(unknown2, 0, 16);
	
		// 00000068 - Status of download
		byte[] dl_status = new byte[16];
              dl_status[0] = 0x00;
              dl_status[1] = 0x00;
              dl_status[2] = 0x00;
              dl_status[3] = 0x68;
              dl_status[4] = 0x00;
              dl_status[5] = 0x00;
              dl_status[6] = 0x00;
              dl_status[7] = 0x04;
              dl_status[8] = 0x00;
              dl_status[9] = 0x00;
              dl_status[10] = 0x00;
              dl_status[11] = 0x04;
              dl_status[12] = 0x00;
              dl_status[13] = 0x00;
              dl_status[14] = 0x00;
              dl_status[15] = 0x00;
              o.Write(dl_status, 0, 16);
	
		// 000000D0 - Download current size (in bytes)
		byte[] dl_progress = new byte[12];
              dl_progress[0] = 0x00;
              dl_progress[1] = 0x00;
              dl_progress[2] = 0x00;
              dl_progress[3] = 0xD0;
              dl_progress[4] = 0x00;
              dl_progress[5] = 0x00;
              dl_progress[6] = 0x00;
              dl_progress[7] = 0x08;
              dl_progress[8] = 0x00;
              dl_progress[9] = 0x00;
              dl_progress[10] = 0x00;
              dl_progress[11] = 0x08;
              o.Write(dl_progress, 0, 12);

            //pkg size
              byte[] ps = BitConverter.GetBytes(pkg_size);
              Array.Reverse(ps);
              o.Write(ps, 0, 8);
	
		// 000000CE - Download expected size (in bytes)
		byte[] dl_size = new byte[12];
              dl_size[0] = 0x00;
              dl_size[1] = 0x00;
              dl_size[2] = 0x00;
              dl_size[3] = 0xCE;
              dl_size[4] = 0x00;
              dl_size[5] = 0x00;
              dl_size[6] = 0x00;
              dl_size[7] = 0x08;
              dl_size[8] = 0x00;
              dl_size[9] = 0x00;
              dl_size[10] = 0x00;
              dl_size[11] = 0x08;
              o.Write(dl_size, 0, 12);

            //pkg size
              o.Write(ps, 0, 8);
	
		// 00000069 - Display title
		byte[] title = new byte[4]; 
            title[0] = 0x00;
            title[1] = 0x00;
            title[2] = 0x00; 
            title[3] = 0x69;
            o.Write(title, 0, 4);

		string title_str = "";
		title_str = string.Format("\xE2\x98\x85 Install \x22{0}\x22", pkg_name);
	
		int title_len = title_str.Length + 1;
        byte[] t = BitConverter.GetBytes(title_len);
        Array.Reverse(t);
        o.Write(t, 0, 4);
        o.Write(t, 0, 4);
        byte[] string_title = new byte[title_len];
        string_title = charsToByte((title_str).ToCharArray());
        o.Write(string_title, 0, string_title.Length);
        byte[] fill = new byte[1];
        fill[0] = 0x00;
        o.Write(fill, 0, 1);
	
		// 000000CB - PKG file name
		byte[] filename = new byte[4];
            filename[0] = 0x00;
            filename[1] = 0x00;
            filename[2] = 0x00;
            filename[3] = 0xCB;
            o.Write(filename, 0, 4);

            int filename_len = pkg_name.Length + 1;
            byte[] f = BitConverter.GetBytes(filename_len);
            Array.Reverse(f);
            o.Write(f, 0, 4);
            o.Write(f, 0, 4);
            byte[] string_name = new byte[filename_len];
            string_name = charsToByte((pkg_name).ToCharArray());
            o.Write(string_name, 0, string_name.Length);
            
            o.Write(fill, 0, 1);
	
		// 0000006A - Icon location / path (PNG w/o extension) 
		byte[] iconpath = new byte[4];
            iconpath[0] = 0x00;
            iconpath[1] = 0x00;
            iconpath[2] = 0x00; 
            iconpath[3] = 0x6A;
            o.Write(iconpath, 0, 4);
	
            byte[] iconpath_len = new byte[8];
            iconpath_len[0] = 0x00; 
            iconpath_len[1] = 0x00;
            iconpath_len[2] = 0x00; 
            iconpath_len[3] = 0x2A; 
            iconpath_len[4] = 0x00; 
            iconpath_len[5] = 0x00; 
            iconpath_len[6] = 0x00; 
            iconpath_len[7] = 0x2A;
            o.Write(iconpath_len, 0, 8);
		
	    string icon_path = "/dev_hdd0/vsh/game_pkg/"+file_number+"/ICON_FILE";
        byte[] string_path = new byte[0x2A];
        string_path = charsToByte((icon_path).ToCharArray());
        o.Write(string_path, 0, string_path.Length);
       ;
        o.Write(fill, 0, 1);
        o.Close();
        File.Copy(path, file_path + "/" + pkg_name);
		// ------------------------------------------------------------------------
		// write - ICON_FILE
		//

        File.Copy("ICON_FILE", "game_pkg/" + file_number + "/ICON_FILE");
        }




        public static byte[] charsToByte(char[] b)
        {
            byte[] c = new byte[b.Length];
            for (int i = 0; i < b.Length; i++)
                c[i] = (byte)b[i];
            return c;
        }



    }
}
