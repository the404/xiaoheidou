using Framework.Redis.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Redis.Interfaces
{
    public interface ICommand<TParam, TResult>
        where TParam : CommandParameterBase
    {
        TResult Send(TParam commandParameter = null);

        void SendNonReturn(TParam commandParameter = null);
    }
}
