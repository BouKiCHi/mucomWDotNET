using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mucomDotNET.Common;
using musicDriverInterface;

namespace mucomDotNET.Driver
{
    public class Work
    {
        public object lockObj = new object();
        public object SystemInterrupt = new object();

        private int _status = 0;

        public int Status
        {
            get { lock (lockObj) { return _status; } }
            set { lock (lockObj) { _status = value; } }
        }

        public uint mDataAdr { get; internal set; }
        public int idx { get; internal set; }
        public CHDAT cd { get; internal set; }
        public bool carry { get; internal set; }
        public uint hl { get; internal set; }
        public byte A_Reg { get; internal set; }
        public int weight { get; internal set; }
        public object crntMmlDatum { get; internal set; }
        public int maxLoopCount { get; internal set; } = -1;

        public OPNATimer timer = null;
        public ulong timeCounter = 0L;
        public byte[] fmVoice = null;
        public Tuple<string, ushort[]>[] pcmTables = null;
        public MmlDatum[] mData = null;
        public SoundWork CurrentSoundWork = null;
        public byte[] fmVoiceAtMusData = null;
        public bool isDotNET = false;

        public List<SoundWork> SoundWorkList;

        public Work()
        {
            Init();
        }

        /// <summary>
        /// 最大チップ数(初期値は対応最大数)
        /// </summary>
        public int MaxChip = CommonData.MAX_CHIP;

        /// <summary>
        /// 最大チャンネル数
        /// </summary>
        public int MaxChannels;

        /// <summary>
        /// 最大チップ数の設定
        /// </summary>
        /// <param name="chan"></param>
        public void SetMaxChannel(int chan) {
            MaxChip = chan / 11;
            MaxChannels = 11 * MaxChip;
        }

        internal void Init()
        {
            SoundWorkList = new List<SoundWork>();
            for(int i=0; i < CommonData.MAX_CHIP; i++) {
                var sw = new SoundWork();
                sw.Init();
                SoundWorkList.Add(sw);
            }

            CurrentSoundWork = SoundWorkList[0];
        }

        public void SetChipWork(int index) {
            CurrentSoundWork = SoundWorkList[index];
        }
    }
}
