using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Loading.Parameters
{
    [CreateAssetMenu(fileName = nameof(LoadJobReference), menuName = "Celeste/Parameters/Loading/Load Job Reference")]
    public class LoadJobReference : ParameterReference<LoadJob, LoadJobValue, LoadJobReference>
    {
    }
}
