using System;
using System.Collections.Generic;
using System.Text;

namespace mucomDotNET.Common {
    public class CommonData {
        /// <summary>
        /// 最大チップ数
        /// </summary>
        public const int MAX_CHIP = 4;

        /// <summary>
        /// チップの最大チャンネル数
        /// </summary>
        public const int MAX_CHIP_CH = 11;


        /// <summary>
        /// 曲全体の最大トラック数
        /// </summary>
        public const int MAX_WORK_CHANNEL = MAX_CHIP_CH * MAX_CHIP;
    }
}
