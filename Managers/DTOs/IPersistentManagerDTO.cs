using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Managers.DTOs
{
    public interface IPersistentManagerDTO<T, TDTO>
        where T : PersistentManager<T, TDTO>
        where TDTO : class, IPersistentManagerDTO<T, TDTO>
    {
    }
}
