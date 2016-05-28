using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XinTuo.Finance.Models
{
    public class MRegion
    {
        /// <summary>
        /// 行政区划编码。固定长度为6位。
        /// 可作为县/区的编码。
        /// </summary>
        public virtual int RegionId { get; set; }

        /// <summary>
        /// 市级区域编码
        /// </summary>
        public virtual int CityId
        {
            get; set;
        }

        /// <summary>
        /// 省级区域编码
        /// </summary>
        public virtual int ProvinceId
        {
            get; set;
        }

        /// <summary>
        /// 县/城区全称
        /// </summary>
        public virtual string CountyName
        {
            get; set;
        }

        /// <summary>
        /// 城市全称
        /// </summary>
        public virtual string CityName
        {
            get; set;
        }

        /// <summary>
        /// 省名称
        /// </summary>
        public virtual string ProvinceName
        {
            get; set;
        }

        /// <summary>
        /// 区域名称的拼音首字母。
        /// 用于快速搜索定位。
        /// </summary>
        public virtual string RegionNamePinyin
        {
            get; set;
        }
    }
}