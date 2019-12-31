﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mucomDotNET.Driver
{
    public class MUBHeader
    {
        public uint magic = 0;
        public uint dataoffset = 0;
        public uint datasize = 0;
        public uint tagdata = 0;
        public uint tagsize = 0;
        public uint pcmdata = 0;
        public uint pcmsize = 0;
        public uint jumpcount = 0;
        public uint jumpline = 0;
        public uint ext_flags = 0;
        public uint ext_system = 0;
        public uint ext_target = 0;
        public uint ext_channel_num = 0;
        public uint ext_fmvoice_num = 0;
        public uint ext_player = 0;
        public uint pad1 = 0;
        public byte[] ext_fmvoice = new byte[32];
        private byte[] srcBuf = null;


        public MUBHeader(byte[] buf)
        {
            magic = Common.getLE32(buf, 0x0000);
            dataoffset = Common.getLE32(buf, 0x0004);
            datasize = Common.getLE32(buf, 0x0008);
            tagdata = Common.getLE32(buf, 0x000c);
            tagsize = Common.getLE32(buf, 0x0010);
            pcmdata = Common.getLE32(buf, 0x0014);
            pcmsize = Common.getLE32(buf, 0x0018);
            jumpcount = Common.getLE16(buf, 0x001c);
            jumpline = Common.getLE16(buf, 0x001e);

            if (magic == 0x3842554d) //'MUB8'
            {
                ext_flags = Common.getLE16(buf, 0x0020);
                ext_system = buf[0x0022];
                ext_target = buf[0x0023];
                ext_channel_num = Common.getLE16(buf, 0x0024);
                ext_fmvoice_num = Common.getLE16(buf, 0x0026);
                ext_player = Common.getLE32(buf, 0x0028);
                pad1 = Common.getLE32(buf, 0x002c);
                for (int i = 0; i < 32; i++)
                {
                    ext_fmvoice[i] = buf[0x0030 + i];
                }
            }
            srcBuf = buf;
        }

        public byte[] GetDATA()
        {
            try
            {
                if (dataoffset == 0) return null;
                if (srcBuf == null) return null;

                List<byte> lb = new List<byte>();
                for (int i = 0; i < datasize; i++)
                {
                    lb.Add(srcBuf[dataoffset + i]);
                }

                return lb.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public List<Tuple<string, string>> GetTags()
        {
            try
            {
                if (tagdata == 0) return null;
                if (srcBuf == null) return null;

                List<byte> lb = new List<byte>();
                for (int i = 0; i < tagsize; i++)
                {
                    lb.Add(srcBuf[tagdata + i]);
                }

                return GetTagsByteArray(lb.ToArray());
            }
            catch
            {
                return null;
            }
        }

        public byte[] GetPCM()
        {
            try
            {
                if (pcmdata == 0) return null;
                if (srcBuf == null) return null;

                List<byte> lb = new List<byte>();
                for (int i = 0; i < pcmsize; i++)
                {
                    lb.Add(srcBuf[pcmdata + i]);
                }

                return lb.ToArray();
            }
            catch
            {
                return null;
            }
        }

        private List<Tuple<string, string>> GetTagsByteArray(byte[] buf)
        {
            var text = Encoding.GetEncoding("shift_jis").GetString(buf)
                .Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.IndexOf("#") == 0);

            List<Tuple<string, string>> tags = new List<Tuple<string, string>>();
            foreach (string v in text)
            {
                try
                {
                    int p = v.IndexOf(' ');
                    string tag = "";
                    string ele = "";
                    if (p >= 0)
                    {
                        tag = v.Substring(1, p).Trim().ToLower();
                        ele = v.Substring(p + 1).Trim();
                        Tuple<string, string> item = new Tuple<string, string>(tag, ele);
                        tags.Add(item);
                    }
                }
                catch { }
            }

            return tags;
        }

    }

}
