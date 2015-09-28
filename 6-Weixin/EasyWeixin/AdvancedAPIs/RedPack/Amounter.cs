using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeixin.AdvancedAPIs.RedPack
{
    public class Amounter
    {
        /// <summary>
        /// 1-200元
        /// </summary>
        /// <param name="amount">单位是分</param>
        /// <returns></returns>
        public static int GetAmount(int amount)
        {
            var maxAmount = 20000;
            if (amount == 0)
                return new Random().Next(100, maxAmount);//微信红包必须大于100分。
            else
            {
                return amount > maxAmount ? maxAmount : amount;
            }
        }

        /// <summary>
        /// 单位是分
        /// </summary>
        /// <param name="max">单位是分150分就是1.5元</param>
        /// <param name="min">单位是分</param>
        /// <returns></returns>
        public static int GetAmount(int min, int max)
        {
            var amount = new Random().Next(min, max);
            return GetAmount(amount);
        }
    }
}
