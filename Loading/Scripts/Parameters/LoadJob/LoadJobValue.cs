using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Loading.Parameters
{
    [CreateAssetMenu(fileName = nameof(LoadJobValue), menuName = "Celeste/Parameters/Loading/Load Job Value")]
    public class LoadJobValue : ParameterValue<LoadJob>
    {
    }
}
