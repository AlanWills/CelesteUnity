using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.UI
{
    public interface IPopupController
    {
        void OnShow();
        void OnHide();

        void OnConfirmPressed();
        void OnClosePressed();
    }
}
