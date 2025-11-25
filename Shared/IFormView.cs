using System;
using Model.Entities;
using System.Collections.Generic;


namespace Shared
{
    public interface IFormView : ICommonView
    {
        // Form-специфичные методы (если нужны)
        int GetSelectedCarId();
        int GetSelectedOwnerId();
    }
}
