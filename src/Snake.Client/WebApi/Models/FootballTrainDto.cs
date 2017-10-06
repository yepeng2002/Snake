using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Client.WebApi.Models
{
    public class FootballTrainDto
    {
        /// <summary>
        /// 联赛
        /// </summary>
        public string Div { get; set; }
        /// <summary>
        /// 开赛时间
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 主队
        /// </summary>
        public string HomeTeam { get; set; }
        /// <summary>
        /// 客队
        /// </summary>
        public string AwayTeam { get; set; }

        #region 训练数据

        public float diff_ppg { get; set; }
        public float diff_rfag { get; set; }
        public float f_hfag { get; set; }
        public float aimpro_h { get; set; }
        public float aimpro_d { get; set; }
        [DisplayName("主队得分")]
        public int HomePoint { get; set; }

        #endregion
    }
    public class FootballDataDto
    {
        /// <summary>
        /// 联赛
        /// </summary>
        public string Div { get; set; }
        /// <summary>
        /// 开赛时间
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 主队
        /// </summary>
        public string HomeTeam { get; set; }
        /// <summary>
        /// 客队
        /// </summary>
        public string AwayTeam { get; set; }

        #region 待预测数据

        public float diff_ppg { get; set; }
        public float diff_rfag { get; set; }
        public float f_hfag { get; set; }
        public float aimpro_h { get; set; }
        public float aimpro_d { get; set; }

        #endregion
    }
    public class FootballPredictProbaDto
    {
        /// <summary>
        /// 联赛
        /// </summary>
        public string Div { get; set; }
        /// <summary>
        /// 开赛时间
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 主队
        /// </summary>
        public string HomeTeam { get; set; }
        /// <summary>
        /// 客队
        /// </summary>
        public string AwayTeam { get; set; }

        #region 预测结果

        /// <summary>
        /// 主队负概率
        /// </summary>
        public float proba_home_0 { get; set; }
        public float proba_home_1 { get; set; }
        /// <summary>
        /// 主队获胜概率
        /// </summary>
        public float proba_home_3 { get; set; }

        #endregion
    }
    public class FootballFitData
    {
        public IList<FootballTrainDto> FootballTrainDto { get; set; }
        public IList<FootballDataDto> FootballDataDto { get; set; }
    }
}
