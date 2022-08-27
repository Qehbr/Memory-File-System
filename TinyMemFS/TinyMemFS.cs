using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class TinyMemFSConsole
    {
        internal class MyFile
        {

            public string fileName { get; }
            public long fileSize { get; }
            public DateTime dateTime { get; }
            public byte[] data { get; set; }

            public MyFile(string fileName, long fileSize, DateTime dateTime, byte[] data)
            {
                this.fileName = fileName;
                this.fileSize = fileSize;
                this.dateTime = dateTime;
                this.data = data;
            }
        }
        private List<MyFile> files;
        private bool isEncrypted;
        private byte encryptionByte;
        public TinyMemFSConsole()
        {
            // constructor
            files = new List<MyFile>();
            isEncrypted = false;
            encryptionByte = 0;
        }
        public bool add(String fileName, String fileToAdd)
        {
            // fileName - The name of the file to be added to the file system
            // fileToAdd - The file path on the computer that we add to the system
            // return false if operation failed for any reason
            // Example:
            // add("name1.pdf", "C:\\Users\\user\Desktop\\report.pdf")
            // note that fileToAdd isn't the same as the fileName
            if (!File.Exists(fileToAdd))
            {
                return false;
            }

            FileInfo fi = new FileInfo(fileToAdd);
            files.Add(new MyFile(fileName, fi.Length, fi.CreationTime, File.ReadAllBytes(fileToAdd)));
            return true;
        }
        public bool remove(String fileName)
        {
            // fileName - remove fileName from the system
            // this operation releases all allocated memory for this file
            // return false if operation failed for any reason
            // Example:
            // remove("name1.pdf")
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].fileName == fileName)
                {
                    files.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public List<String> listFiles()
        {
            // The function returns a list of strings with the file information in the system
            // Each string holds details of one file as following: "fileName,size,creation time"
            // Example:{
            // "report.pdf,630KB,Friday, ‎May ‎13, ‎2022, ‏‎12:16:32 PM",
            // "table1.csv,220KB,Monday, ‎February ‎14, ‎2022, ‏‎8:38:24 PM" }
            // You can use any format for the creation time and date
            
            List<String> files = new List<string>();
            for (int i = 0; i < this.files.Count; i++)
            {
                string kilobytes = (((double)this.files[i].fileSize)/1000).ToString().Replace(",", ".");
                files.Add(this.files[i].fileName+","+ kilobytes + "KB,"+ this.files[i].dateTime);
            }
            return files;
        }
        public bool save(String fileName, String fileToAdd)
        {
            // this function saves file from the TinyMemFS file system into a file in the physical disk
            // fileName - file name from TinyMemFS to save in the computer
            // fileToAdd - The file path to be saved on the computer
            // return false if operation failed for any reason
            // Example:
            // save("name1.pdf", "C:\\tmp\\fileName.pdf")

            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].fileName == fileName)
                {
                    File.WriteAllBytes(fileToAdd, files[i].data);
                    return true;
                }
            }
            return false;
        }
        public bool encrypt(String key)
        {
            // key - Encryption key to encrypt the contents of all files in the system 
            // You can use an encryption algorithm of your choice
            // return false if operation failed for any reason
            // Example:
            // encrypt("myFSpassword")

            if(isEncrypted || files.Count==0)
            {
                return false;
            }

            for (int i = 0; i < Encoding.ASCII.GetBytes(key).Length; i++)
            {
                encryptionByte += Encoding.ASCII.GetBytes(key)[i];
            }
            
            for(int i = 0; i < this.files.Count; i++)
            {
                for(int j = 0; j < this.files[i].fileSize; j++)
                {
                    files[i].data[j] += encryptionByte;
                }
            }
            isEncrypted = true;    
            return true;
        }

        public bool decrypt(String key)
        {
            // fileName - Decryption key to decrypt the contents of all files in the system 
            // return false if operation failed for any reason
            // Example:
            // decrypt("myFSpassword")
            if (!isEncrypted)
            {
                return false;
            }

            byte keyByte = 0;
            for (int i = 0; i < Encoding.ASCII.GetBytes(key).Length; i++)
            {
                keyByte += Encoding.ASCII.GetBytes(key)[i];
            }

            if(keyByte != encryptionByte)
            {
                return false;
            }

            for (int i = 0; i < this.files.Count; i++)
            {
                for (int j = 0; j < this.files[i].fileSize; j++)
                {
                    files[i].data[j] -= keyByte;
                }
            }
            encryptionByte = 0;
            isEncrypted = false;
            return true;
        }

    }
